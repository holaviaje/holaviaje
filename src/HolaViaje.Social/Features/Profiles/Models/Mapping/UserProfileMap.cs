using AutoMapper;

namespace HolaViaje.Social.Features.Profiles.Models.Mapping;

internal class UserProfileMap : Profile
{
    public UserProfileMap()
    {
        CreateMap<UserProfile, UserProfileViewModel>();
        CreateMap<UserProfileModel, UserProfile>();
    }
}
