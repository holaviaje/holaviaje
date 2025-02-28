using HolaViaje.Social.Shared;

namespace HolaViaje.Social.Features.Posts;

public class Post : EntityBase
{
    public Post()
    {
        Control = new()
        {
            CreatedAt = DateTime.UtcNow,
            LastModifiedAt = DateTime.UtcNow
        };
    }

    public Post(long userId)
        : this()
    {
        UserId = userId;
    }

    /// <summary>
    /// Gets or sets the type of the post.
    /// </summary>
    public PostType Type { get; set; } = PostType.Short;
    /// <summary>
    /// Gets or sets the status of the post.
    /// </summary>
    public PostStatus Status { get; set; } = PostStatus.Unpublished;
    /// <summary>
    /// Gets or sets the content of the post.
    /// </summary>
    public string? Content { get; set; }
    /// <summary>
    /// Gets or sets if the content is HTML.
    /// </summary>
    public bool IsHtmlContent { get; set; }
    /// <summary>
    /// Gets or sets the media files of the post.
    /// </summary>
    public ICollection<MediaFile> MediaFiles { get; set; } = [];
    /// <summary>
    /// Gets or sets the user identifier.
    /// </summary>
    public long UserId { get; set; }
    /// <summary>
    /// Gets or sets the visibility settings of the post.
    /// </summary>
    public Visibility Visibility { get; set; } = Visibility.Public;
    /// <summary>
    /// Gets or sets the members of the post.
    /// </summary>
    public ICollection<PostMember> Members { get; set; } = [];

    /// <summary>
    /// Gets or sets the place information.
    /// </summary>
    public PlaceInfo Place { get; set; } = new();
    /// <summary>
    /// Gets or sets if the post is a draft.
    /// </summary>
    public bool IsDraft { get; set; }
    /// <summary>
    /// Gets or sets if the post is in edit mode.
    /// </summary>
    public bool EditMode { get; set; }
    /// <summary>
    /// Gets or sets the post control information.
    /// </summary>
    public EntityControl Control { get; set; }
    /// <summary>
    /// Gets the allowed file quantity based on the post type.
    /// </summary>
    /// <returns>Quantity of files allowed.</returns>
    public short AllowedFileQuantity
    {
        get
        {
            return Type switch
            {
                PostType.Short => 1,
                PostType.Live => 1,
                _ => 6
            };
        }
    }

    /// <summary>
    /// Updates the last modified date.
    /// </summary>
    public void UpdateLastModified()
    {
        EnsureNotDeleted();
        Control = Control with { LastModifiedAt = DateTime.UtcNow };
    }

    /// <summary>
    /// Deletes the user profile.
    /// </summary>
    public void Delete()
    {
        EnsureNotDeleted();
        Control = Control with { DeletedAt = DateTime.UtcNow, IsDeleted = true };
    }

    /// <summary>
    /// Ensures that the user profile is not deleted.
    /// </summary>
    /// <exception cref="InvalidOperationException">If the user profile is deleted.</exception>
    private void EnsureNotDeleted()
    {
        if (IsDeleted())
        {
            throw new InvalidOperationException("The post is deleted.");
        }
    }

    /// <summary>
    /// Determines whether the post is deleted.
    /// </summary>
    /// <returns></returns>
    public bool IsDeleted() => Control.IsDeleted;

    /// <summary>
    /// Sets the place information of the user.
    /// </summary>
    /// <param name="place"></param>
    public void SetPlace(PlaceInfo place)
    {
        if (Place == place) return;

        Place = place;
        UpdateLastModified();
    }

    /// <summary>
    /// Sets the members of the post.
    /// </summary>
    /// <param name="members"></param>
    public void SetMembers(IEnumerable<PostMember> members)
    {
        if (!members.Any()) return;

        UpdateLastModified();

        var membersToRemove = Members.Except(members).ToList();
        var membersToAdd = members.Except(Members).ToList();

        foreach (var member in membersToRemove)
        {
            Members.Remove(member);
        }

        foreach (var member in membersToAdd)
        {
            Members.Add(member);
        }
    }

    /// <summary>
    /// Determines whether the profile is visible for the specified user.
    /// </summary>
    /// <param name="userId">User that wants to view the profile.</param>
    /// <returns>True if have access, else false.</returns>
    public bool IsVisibleFor(long userId)
    {
        if (IsOwner(userId)) return true; // is the owner.
        if (Visibility == Visibility.Public) return true; // is public.
        if (Visibility == Visibility.Friends)
        {
            // TODO: Check if the user is a friend.
        }

        if (Visibility == Visibility.Custom)
        {
            return Members.Any(m => m.UserId == userId);
        }

        return false;
    }

    /// <summary>
    /// Determines whether the specified user is the owner of the post.
    /// </summary>
    /// <param name="userId">User Identifier.</param>
    /// <returns>True if is the owner, else false.</returns>
    public bool IsOwner(long userId) => UserId == userId;

    /// <summary>
    /// Gets if the post can add a media file.
    /// </summary>
    /// <returns></returns>
    public bool CanAddMediaFile() => MediaFiles.Count < AllowedFileQuantity;

    /// <summary>
    /// Adds a media file to the post.
    /// </summary>
    /// <param name="mediaFile"></param>
    public void AddMediaFile(MediaFile mediaFile)
    {
        MediaFiles.Add(mediaFile);
        UpdateLastModified();
    }

    /// <summary>
    /// Removes a media file from the post.
    /// </summary>
    /// <param name="mediaFile"></param>
    public void RemoveMediaFile(MediaFile mediaFile)
    {
        MediaFiles.Remove(mediaFile);
        UpdateLastModified();
    }

    /// <summary>
    /// Gets a media file by its identifier.
    /// </summary>
    /// <param name="fileId">File Identifier.</param>
    /// <returns>MediaFile or null.</returns>
    public MediaFile? GetMediaFile(string fileId) => MediaFiles.FirstOrDefault(m => m.FileId == fileId);

    /// <summary>
    /// Publishes the post.
    /// </summary>
    public void Publish()
    {
        if (EditMode)
        {
            EditMode = false;
        }

        if (IsDraft)
        {
            IsDraft = false;
        }

        if (Status != PostStatus.Published)
        {
            Status = PostStatus.Published;
        }

        UpdateLastModified();
    }

    /// <summary>
    /// Gets if the post has pending uploads.
    /// </summary>
    /// <returns></returns>
    public bool UploadsPending() => MediaFiles.Any(m => !m.Uploaded);
}
