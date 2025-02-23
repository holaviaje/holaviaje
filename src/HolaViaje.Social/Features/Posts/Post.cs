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

    public Post(int userId, int pageId)
        : this()
    {
        UserId = userId;
        PageId = pageId;
    }

    public PostType Type { get; set; } = PostType.Short;
    public PostStatus Status { get; set; } = PostStatus.Unpublished;
    public string? Content { get; set; }
    public bool IsHtmlContent { get; set; }
    public ICollection<MediaFile> MediaFiles { get; set; } = [];
    public long UserId { get; set; }
    public int PageId { get; set; } = 0;
    /// <summary>
    /// Gets or sets the visibility settings of the post.
    /// </summary>
    public Visibility Visibility { get; set; } = Visibility.Public;

    public ICollection<PostMember> Members { get; set; } = [];

    /// <summary>
    /// Gets or sets the place information.
    /// </summary>
    public PlaceInfo Place { get; set; } = new();
    public bool IsDraft { get; set; }
    public bool EditMode { get; set; }
    /// <summary>
    /// Gets or sets the post control information.
    /// </summary>
    public EntityControl Control { get; set; }

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
        if (Control.IsDeleted)
        {
            throw new InvalidOperationException("The post is deleted.");
        }
    }

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
    public bool IsOwner(long userId) => Id == userId;
}
