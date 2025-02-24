using AutoMapper;

namespace HolaViaje.Social.Features.Posts.Models.Mapping;

public class PostMemberMap : Profile
{
    public PostMemberMap()
    {
        CreateMap<PostMember, PostMemberModel>().ConstructUsing((src, ctx) => new PostMemberModel(src.UserId));
    }
}
