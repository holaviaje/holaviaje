using AutoMapper;

namespace HolaViaje.Social.Features.Posts.Models.Mapping
{
    public class PostMap : Profile
    {
        public PostMap()
        {
            CreateMap<Post, PostViewModel>();
        }
    }
}