namespace HolaViaje.Social.Features.Posts.Models;

public static class PostModelExtensions
{
    public static IEnumerable<PostMember> FromModel(this IEnumerable<PostMemberModel> models)
    {
        return models.Select(model => new PostMember(model.UserId)).ToList();
    }
}