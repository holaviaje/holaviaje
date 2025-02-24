using HolaViaje.Social.Shared;
using HolaViaje.Social.Shared.Models;

namespace HolaViaje.Social.Features.Posts.Models;

public class PostBasicModel
{
    /// <summary>
    /// Gets or sets the status of the post.
    /// </summary>
    public string? Content { get; set; }
    /// <summary>
    /// Gets or sets the visibility settings of the post.
    /// </summary>
    public Visibility Visibility { get; set; } = Visibility.Public;
    public required PlaceInfoModel Place { get; set; }
    public ICollection<PostMemberModel> Members { get; set; } = [];
    public bool KeepOpen { get; set; }
}
