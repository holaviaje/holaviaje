namespace HolaViaje.Social.Features.Posts.Models;

public class PostModel : PostBasicModel
{
    /// <summary>
    /// Gets or sets the type of the post.
    /// </summary>
    public PostType Type { get; set; } = PostType.Short;
    /// <summary>
    /// Gets or sets the page identifier.
    /// </summary>
    public int PageId { get; set; } = 0;
    /// <summary>
    /// Gets or sets if the post is a draft.
    /// </summary>
    public bool IsDraft { get; set; }
}
