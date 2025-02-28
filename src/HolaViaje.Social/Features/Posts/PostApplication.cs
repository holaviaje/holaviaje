using AutoMapper;
using Ganss.Xss;
using HolaViaje.Infrastructure.BlobStorage;
using HolaViaje.Infrastructure.Messaging;
using HolaViaje.Social.Features.Posts.Events;
using HolaViaje.Social.Features.Posts.Models;
using HolaViaje.Social.Features.Posts.Repository;
using HolaViaje.Social.Shared.Models;
using OneOf;
using System.Text.RegularExpressions;

namespace HolaViaje.Social.Features.Posts;

public class PostApplication(IPostRepository postRepository,
    IBlobRepository blobRepository,
    IEventBus eventBus,
    IMapper mapper) : IPostApplication
{
    private readonly Dictionary<string, string> mimeTypes = new Dictionary<string, string>
    {
        { ".jpg", "image/jpeg" },
        { ".png", "image/png" },
        { ".bmp", "image/bmp" },
        { ".mpg", "video/mpeg" },
        { ".mp4", "video/mp4" },
        { ".mov", "video/quicktime" }
     };

    public async Task<OneOf<PostViewModel, ErrorModel>> CreateAsync(PostModel model, long userId, CancellationToken cancellationToken = default)
    {
        var post = new Post(userId)
        {
            Type = model.Type,
            IsDraft = model.IsDraft,
            Status = PostStatus.Unpublished,
        };

        ApplyPostUpdates(post, model);

        var dbEntity = await postRepository.CreateAsync(post);
        return mapper.Map<PostViewModel>(dbEntity);

    }

    public async Task<OneOf<PostViewModel, ErrorModel>> UpdateAsync(long postId, PostBasicModel model, long userId, CancellationToken cancellationToken = default)
    {
        var post = await postRepository.GetAsync(postId, true);

        if (post is null || post.IsDeleted())
        {
            return PostErrorModelHelper.PostNotFoundError();
        }

        if (!post.IsOwner(userId))
        {
            return PostErrorModelHelper.UpdatePostPermissionError();
        }

        ApplyPostUpdates(post, model);

        var dbEntity = await postRepository.UpdateAsync(post);
        return mapper.Map<PostViewModel>(dbEntity);

    }

    private void ApplyPostUpdates(Post post, PostBasicModel model)
    {
        var (htmlContent, content) = ProcessContent(model.Content ?? string.Empty);

        post.Content = content;
        post.IsHtmlContent = htmlContent;
        post.Visibility = model.Visibility;
        post.EditMode = model.KeepOpen;

        post.SetPlace(model.Place?.FromModel() ?? new());
        post.SetMembers(model.Members.FromModel());
        post.UpdateLastModified();
    }

    public async Task<OneOf<PostViewModel, ErrorModel>> PublishAsync(long postId, long userId, CancellationToken cancellationToken = default)
    {
        var post = await postRepository.GetAsync(postId, true);

        if (post is null || post.IsDeleted())
        {
            return PostErrorModelHelper.PostNotFoundError();
        }

        if (!post.IsOwner(userId))
        {
            return PostErrorModelHelper.UpdatePostPermissionError();
        }

        if (post.UploadsPending())
        {
            return PostErrorModelHelper.UploadsPendingError();
        }

        post.Publish();
        post.UpdateLastModified();

        var dbEntity = await postRepository.UpdateAsync(post);

        var publishedEvent = new PostPublished
        {
            PostId = post.Id,
            UserId = userId
        };

        await eventBus.Publish(publishedEvent);

        return mapper.Map<PostViewModel>(dbEntity);

    }

    public async Task<OneOf<MediaFileModel, ErrorModel>> UploadMediaFileAsync(long postId, UploadMediaFileModel model, long userId, CancellationToken cancellationToken = default)
    {
        var post = await postRepository.GetAsync(postId, true);

        if (post is null || post.IsDeleted())
        {
            return PostErrorModelHelper.PostNotFoundError();
        }

        if (!post.IsOwner(userId))
        {
            return PostErrorModelHelper.UpdatePostPermissionError();
        }

        if (!post.CanAddMediaFile())
        {
            return PostErrorModelHelper.MaxFilesQuantityReachedError();
        }

        var extension = Path.GetExtension(model.FileName);
        var fileId = Guid.NewGuid().ToString("N");
        var fileName = $"{fileId}{extension}";
        var containerName = GetContainerName(extension);
        var accessUrl = await blobRepository.UploadAsync(containerName, fileName, model.FileStream);
        var mediaFile = new MediaFile(fileId, model.FileName, model.FileSize, GetContentType(extension), containerName, accessUrl);

        post.AddMediaFile(mediaFile);
        post.UpdateLastModified();

        var dbEntity = await postRepository.UpdateAsync(post);
        return mapper.Map<MediaFileModel>(dbEntity);

    }

    public async Task<OneOf<MediaFileModel, ErrorModel>> CreateUploadLinkAsync(long postId, UploadLinkModel model, long userId, CancellationToken cancellationToken = default)
    {
        var post = await postRepository.GetAsync(postId, true);

        if (post is null || post.IsDeleted())
        {
            return PostErrorModelHelper.PostNotFoundError();
        }

        if (!post.IsOwner(userId))
        {
            return PostErrorModelHelper.UpdatePostPermissionError();
        }

        if (!post.CanAddMediaFile())
        {
            return PostErrorModelHelper.MaxFilesQuantityReachedError();
        }

        // Is always true because the media file is not uploaded yet.
        post.EditMode = true;

        var extension = Path.GetExtension(model.FileName);
        var fileId = Guid.NewGuid().ToString("N");
        var fileName = $"{fileId}{extension}";
        var containerName = GetContainerName(extension);
        var blobLink = await blobRepository.UploadLinkAsync(containerName, fileName, DetermineLinkExpiration(model.FileSize));
        var mediaFile = new MediaFile(fileId, model.FileName, model.FileSize, GetContentType(extension), containerName, blobLink.AccessUrl);

        post.AddMediaFile(mediaFile);
        post.UpdateLastModified();

        var dbEntity = await postRepository.UpdateAsync(post);
        return mapper.Map<MediaFileModel>(dbEntity);

    }

    public async Task<OneOf<MediaFileModel, ErrorModel>> MarkAsUploadedAsync(long postId, string fileId, long userId, CancellationToken cancellationToken = default)
    {
        var post = await postRepository.GetAsync(postId, true);

        if (post is null || post.IsDeleted())
        {
            return PostErrorModelHelper.PostNotFoundError();
        }

        if (!post.IsOwner(userId))
        {
            return PostErrorModelHelper.UpdatePostPermissionError();
        }

        var mediaFile = post.GetMediaFile(fileId);

        if (mediaFile is null)
        {
            return PostErrorModelHelper.MediaFileNotFoundError();
        }

        mediaFile.Uploaded = true;
        post.UpdateLastModified();

        var dbEntity = await postRepository.UpdateAsync(post);
        return mapper.Map<MediaFileModel>(dbEntity);

    }

    public async Task<OneOf<MediaFileModel, ErrorModel>> DeleteMediaFileAsync(long postId, string fileId, long userId, CancellationToken cancellationToken = default)
    {
        var post = await postRepository.GetAsync(postId, true);

        if (post is null || post.IsDeleted())
        {
            return PostErrorModelHelper.PostNotFoundError();
        }

        if (!post.IsOwner(userId))
        {
            return PostErrorModelHelper.UpdatePostPermissionError();
        }

        var mediaFile = post.GetMediaFile(fileId);

        if (mediaFile is null)
        {
            return PostErrorModelHelper.MediaFileNotFoundError();
        }

        await blobRepository.DeleteAsync(mediaFile.ContainerName, mediaFile.FileName);

        post.RemoveMediaFile(mediaFile);
        post.UpdateLastModified();

        var dbEntity = await postRepository.UpdateAsync(post, cancellationToken);
        return mapper.Map<MediaFileModel>(dbEntity);

    }

    public async Task<OneOf<PostViewModel, ErrorModel>> DeleteAsync(long postId, long userId, CancellationToken cancellationToken = default)
    {
        var post = await postRepository.GetAsync(postId, true);

        if (post is null || post.IsDeleted())
        {
            return PostErrorModelHelper.PostNotFoundError();
        }

        if (!post.IsOwner(userId))
        {
            return PostErrorModelHelper.UpdatePostPermissionError();
        }

        foreach (var mediaFile in post.MediaFiles)
        {
            await blobRepository.DeleteAsync(mediaFile.ContainerName, mediaFile.FileName);
        }

        post.Delete();

        var dbEntity = await postRepository.UpdateAsync(post, cancellationToken);

        var unpublishedEvent = new UnpublishedPost
        {
            PostId = post.Id,
            UserId = userId
        };

        await eventBus.Publish(unpublishedEvent);

        return mapper.Map<PostViewModel>(dbEntity);

    }

    public async Task<OneOf<PostViewModel, ErrorModel>> GetAsync(long postId, long userId, CancellationToken cancellationToken = default)
    {
        var post = await postRepository.GetAsync(postId, false, cancellationToken);

        if (post is null || post.IsDeleted())
        {
            return PostErrorModelHelper.PostNotFoundError();
        }

        if (!post.IsVisibleFor(userId))
        {
            return PostErrorModelHelper.AccessDeniedError();
        }

        return mapper.Map<PostViewModel>(post);

    }

    private string GetContentType(string extension)
    {
        if (mimeTypes.TryGetValue(extension, out string mimeType))
        {
            return mimeType;
        }

        return "application/octet-stream";
    }

    /// <summary>
    /// Determines the expiration time of the link based on the file size.
    /// </summary>
    /// <param name="fileSize">file size</param>
    /// <returns>timespan</returns>
    private static TimeSpan DetermineLinkExpiration(long fileSize)
    {
        var sizeInMb = fileSize / 1024 / 1024;
        const double speedConnectionFactor = 2.1;
        var totalTime = sizeInMb * speedConnectionFactor;

        if (totalTime < 60)
        {
            return TimeSpan.FromMinutes(1);
        }

        return TimeSpan.FromSeconds(totalTime);
    }

    /// <summary>
    /// Gets the container name based on the file extension.
    /// </summary>
    /// <param name="extension">file extension</param>
    /// <returns>container name</returns>
    /// <exception cref="ArgumentException">if container can't be determinated.</exception>
    private static string GetContainerName(string extension)
    {
        if (MediaFile.ImageValidExtensions.Contains(extension))
        {
            return MediaFile.ImageContainerName;
        }

        if (MediaFile.VideoValidExtensions.Contains(extension))
        {
            return MediaFile.VideoContainerName;
        }

        throw new ArgumentException("Invalid file extension.");
    }

    /// <summary>
    /// Processes the content of the post.
    /// </summary>
    /// <param name="inputContent">content</param>
    /// <returns>true or false and content santatize if is html</returns>
    private (bool htmlContent, string content) ProcessContent(string inputContent)
    {
        var htmlContent = ContainsHtml(inputContent);

        if (htmlContent)
        {
            return (true, SanatizeHtml(inputContent));
        }

        return (false, inputContent);
    }

    /// <summary>
    /// Determines if the content is HTML.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    private static bool ContainsHtml(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return false;

        // regular expression for HTML tags
        string pattern = "<\\s*\\/?\\s*(\\w+)(\\s+[^>]*)?>";
        return Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase);
    }

    /// <summary>
    /// Sanitizes the input HTML content.
    /// </summary>
    /// <param name="input">content</param>
    /// <returns>content sanatized</returns>
    private static string SanatizeHtml(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return string.Empty;

        var sanitizer = new HtmlSanitizer();

        sanitizer.AllowedTags.Remove("script");
        sanitizer.AllowedTags.Remove("iframe");
        sanitizer.AllowedTags.Remove("object");
        sanitizer.AllowedTags.Remove("embed");
        sanitizer.AllowedTags.Remove("form");
        sanitizer.AllowedTags.Remove("input");

        return sanitizer.Sanitize(input);
    }
}