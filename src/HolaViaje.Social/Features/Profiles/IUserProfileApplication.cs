using HolaViaje.Social.Features.Profiles.Models;
using HolaViaje.Social.Shared.Models;
using OneOf;

namespace HolaViaje.Social.Features.Profiles;

public interface IUserProfileApplication
{
    Task<OneOf<UserProfileViewModel, ErrorModel>> CreateAsync(long userId);
    Task<OneOf<UserProfileViewModel, ErrorModel>> UpdateAsync(long profileId, UserProfileModel model, long userId);
    Task<OneOf<UserProfileViewModel, ErrorModel>> UpdateAvailabilityAsync(long profileId, AvailabilityModel model, long userId);
    Task<OneOf<UserProfileViewModel, ErrorModel>> UpdatePlaceAsync(long profileId, PlaceInfoModel model, long userId);
    Task<OneOf<UserProfileViewModel, ErrorModel>> UpdateSpokenLanguagesAsync(long profileId, IEnumerable<SpokenLanguageModel> models, long userId);
    Task<OneOf<UserProfileViewModel, ErrorModel>> DeleteAsync(long profileId, long userId);
    Task<OneOf<UserProfileViewModel, ErrorModel>> GetAsync(long profileId, long userId);
}
