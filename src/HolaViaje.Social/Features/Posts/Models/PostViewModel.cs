using HolaViaje.Social.Shared;
using HolaViaje.Social.Shared.Models;

namespace HolaViaje.Social.Features.Posts.Models;

public class PostViewModel
{
    /// <summary>
    /// Gets or sets the identifier of the post.
    /// </summary>
    public long Id { get; set; }
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
    public ICollection<MediaFileModel> MediaFiles { get; set; } = [];
    /// <summary>
    /// Gets or sets the user identifier.
    /// </summary>
    public long UserId { get; set; }
    /// <summary>
    /// Gets or sets the page identifier.
    /// </summary>
    public int PageId { get; set; } = 0;
    /// <summary>
    /// Gets or sets the visibility settings of the post.
    /// </summary>
    public Visibility Visibility { get; set; } = Visibility.Public;
    /// <summary>
    /// Gets or sets the members of the post.
    /// </summary>
    public ICollection<PostMemberModel> Members { get; set; } = [];

    /// <summary>
    /// Gets or sets the place information.
    /// </summary>
    public PlaceInfoModel Place { get; set; } = new();
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
    public required EntityControlModel Control { get; set; }
}
