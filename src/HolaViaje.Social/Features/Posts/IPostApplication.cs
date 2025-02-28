using HolaViaje.Social.Features.Posts.Models;
using HolaViaje.Social.Shared.Models;
using OneOf;

namespace HolaViaje.Social.Features.Posts;

/// <summary>
/// Post Application Service.
/// </summary>
public interface IPostApplication
{
    /// <summary>
    /// Creates a new post.
    /// </summary>
    /// <param name="model">Post Model.</param>
    /// <param name="userId">User Identifier.</param>
    /// <param name="cancellationToken">cancellation token</param>
    /// <returns></returns>
    Task<OneOf<PostViewModel, ErrorModel>> CreateAsync(PostModel model, long userId, CancellationToken cancellationToken = default);
    /// <summary>
    /// Updates a post.
    /// </summary>
    /// <param name="postId"></param>
    /// <param name="model"></param>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<OneOf<PostViewModel, ErrorModel>> UpdateAsync(long postId, PostBasicModel model, long userId, CancellationToken cancellationToken = default);
    /// <summary>
    /// Publishes a post.
    /// </summary>
    /// <param name="postId"></param>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<OneOf<PostViewModel, ErrorModel>> PublishAsync(long postId, long userId, CancellationToken cancellationToken = default);
    Task<OneOf<MediaFileModel, ErrorModel>> UploadMediaFileAsync(long postId, UploadMediaFileModel model, long userId, CancellationToken cancellationToken = default);
    /// <summary>
    /// Creates an upload link for a media file.
    /// </summary>
    /// <param name="postId"></param>
    /// <param name="model"></param>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<OneOf<MediaFileModel, ErrorModel>> CreateUploadLinkAsync(long postId, UploadLinkModel model, long userId, CancellationToken cancellationToken = default);
    /// <summary>
    /// Marks a media file as uploaded.
    /// </summary>
    /// <param name="postId"></param>
    /// <param name="fileId"></param>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<OneOf<MediaFileModel, ErrorModel>> MarkAsUploadedAsync(long postId, string fileId, long userId, CancellationToken cancellationToken = default);
    /// <summary>
    /// Deletes a media file.
    /// </summary>
    /// <param name="postId"></param>
    /// <param name="fileId"></param>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<OneOf<MediaFileModel, ErrorModel>> DeleteMediaFileAsync(long postId, string fileId, long userId, CancellationToken cancellationToken = default);
    /// <summary>
    /// Deletes a post.
    /// </summary>
    /// <param name="postId"></param>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<OneOf<PostViewModel, ErrorModel>> DeleteAsync(long postId, long userId, CancellationToken cancellationToken = default);
    /// <summary>
    /// Gets a post.
    /// </summary>
    /// <param name="postId"></param>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<OneOf<PostViewModel, ErrorModel>> GetAsync(long postId, long userId, CancellationToken cancellationToken = default);
}
