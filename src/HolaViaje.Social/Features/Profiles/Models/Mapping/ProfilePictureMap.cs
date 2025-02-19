using AutoMapper;

namespace HolaViaje.Social.Features.Profiles.Models.Mapping;

public class ProfilePictureMap : Profile
{
    public ProfilePictureMap()
    {
        CreateMap<ProfilePicture, ProfilePictureModel>()
            .ConstructUsing((src, ctx) => new ProfilePictureModel(src.Filename, src.ImageUrl, src.LastModifiedAt));
    }
}
