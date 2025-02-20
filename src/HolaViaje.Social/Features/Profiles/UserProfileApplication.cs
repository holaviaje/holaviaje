using AutoMapper;
using HolaViaje.Social.Features.Profiles.Models;
using HolaViaje.Social.Features.Profiles.Repository;
using HolaViaje.Social.Shared;
using HolaViaje.Social.Shared.Models;
using OneOf;

namespace HolaViaje.Social.Features.Profiles;

/// <summary>
/// UserProfileApplication
/// </summary>
/// <param name="profileRepository">User Profile Repository</param>
/// <param name="mapper">AutoMapper</param>
public class UserProfileApplication(IUserProfileRepository profileRepository, IMapper mapper) : IUserProfileApplication
{
    /// <summary>
    /// Create a new user profile
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<OneOf<UserProfileViewModel, ErrorModel>> CreateAsync(long userId)
    {
        if (profileRepository.GetAsync(userId) != null)
        {
            return UserProfileErrorHelper.ProfileAlreadyExists();
        }

        var userProfile = new UserProfile(userId) { Control = new() };
        var dbEntity = await profileRepository.CreateAsync(userProfile);

        return mapper.Map<UserProfileViewModel>(dbEntity);
    }

    /// <summary>
    /// Update an existing user profile
    /// </summary>
    /// <param name="profileId"></param>
    /// <param name="model"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<OneOf<UserProfileViewModel, ErrorModel>> UpdateAsync(long profileId, UserProfileModel model, long userId)
    {
        var userProfile = await profileRepository.GetAsync(profileId, true);
        if (userProfile is null)
        {
            return UserProfileErrorHelper.ProfileNotFound();
        }

        if (!userProfile.IsOwner(userId))
        {
            return UserProfileErrorHelper.ProfileAccessDenied();
        }

        var mergedEntity = mapper.Map(model, userProfile);
        var dbEntity = await profileRepository.UpdateAsync(userProfile);
        return mapper.Map<UserProfileViewModel>(dbEntity);
    }

    /// <summary>
    /// Update the availability of a user profile
    /// </summary>
    /// <param name="profileId"></param>
    /// <param name="model"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<OneOf<UserProfileViewModel, ErrorModel>> UpdateAvailabilityAsync(long profileId, AvailabilityModel model, long userId)
    {
        var userProfile = await profileRepository.GetAsync(profileId, true);
        if (userProfile is null)
        {
            return UserProfileErrorHelper.ProfileNotFound();
        }

        if (!userProfile.IsOwner(userId))
        {
            return UserProfileErrorHelper.ProfileAccessDenied();
        }

        var availability = new Availability(model.IsAvailable, model.AvailableFor);
        userProfile.SetAvailability(availability);
        var dbEntity = await profileRepository.UpdateAsync(userProfile);
        return mapper.Map<UserProfileViewModel>(dbEntity);
    }

    /// <summary>
    /// Update the place of a user profile
    /// </summary>
    /// <param name="profileId"></param>
    /// <param name="model"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<OneOf<UserProfileViewModel, ErrorModel>> UpdatePlaceAsync(long profileId, PlaceInfoModel model, long userId)
    {
        var userProfile = await profileRepository.GetAsync(profileId, true);
        if (userProfile is null)
        {
            return UserProfileErrorHelper.ProfileNotFound();
        }

        if (!userProfile.IsOwner(userId))
        {
            return UserProfileErrorHelper.ProfileAccessDenied();
        }

        var place = new PlaceInfo
        {
            Country = model.Country,
            State = model.State,
            City = model.City,
            Location = model.Location != null ? new LocationInfo(model.Location.Latitude, model.Location.Longitude) : null
        };

        userProfile.SetPlace(place);
        var dbEntity = await profileRepository.UpdateAsync(userProfile);
        return mapper.Map<UserProfileViewModel>(dbEntity);
    }

    /// <summary>
    /// Update the spoken languages of a user profile
    /// </summary>
    /// <param name="profileId"></param>
    /// <param name="models"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<OneOf<UserProfileViewModel, ErrorModel>> UpdateSpokenLanguagesAsync(long profileId, IEnumerable<SpokenLanguageModel> models, long userId)
    {
        var userProfile = await profileRepository.GetAsync(profileId, true);
        if (userProfile is null)
        {
            return UserProfileErrorHelper.ProfileNotFound();
        }

        if (!userProfile.IsOwner(userId))
        {
            return UserProfileErrorHelper.ProfileAccessDenied();
        }

        var spokenLanguages = models.Select(x => new SpokenLanguage(x.Code, x.Language)).ToList();

        userProfile.SetSpokenLanguages(spokenLanguages);
        var dbEntity = await profileRepository.UpdateAsync(userProfile);
        return mapper.Map<UserProfileViewModel>(dbEntity);
    }

    /// <summary>
    /// Update the spoken languages of a user profile
    /// </summary>
    /// <param name="profileId"></param>
    /// <param name="models"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<OneOf<UserProfileViewModel, ErrorModel>> UpdatePictureAsync(long profileId, string fileName, string imageUrl, long userId)
    {
        var userProfile = await profileRepository.GetAsync(profileId, true);
        if (userProfile is null)
        {
            return UserProfileErrorHelper.ProfileNotFound();
        }

        if (!userProfile.IsOwner(userId))
        {
            return UserProfileErrorHelper.ProfileAccessDenied();
        }

        var picture = new ProfilePicture(fileName, imageUrl) { LastModifiedAt = DateTime.UtcNow };

        userProfile.SetPicture(picture);
        var dbEntity = await profileRepository.UpdateAsync(userProfile);
        return mapper.Map<UserProfileViewModel>(dbEntity);
    }

    /// <summary>
    /// Delete a user profile
    /// </summary>
    /// <param name="profileId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<OneOf<UserProfileViewModel, ErrorModel>> DeleteAsync(long profileId, long userId)
    {
        var userProfile = await profileRepository.GetAsync(profileId, true);
        if (userProfile is null)
        {
            return UserProfileErrorHelper.ProfileNotFound();
        }

        if (!userProfile.IsOwner(userId))
        {
            return UserProfileErrorHelper.ProfileAccessDenied();
        }

        userProfile.Delete();
        var dbEntity = await profileRepository.UpdateAsync(userProfile);
        return mapper.Map<UserProfileViewModel>(dbEntity);
    }

    /// <summary>
    /// Get a user profile
    /// </summary>
    /// <param name="profileId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<OneOf<UserProfileViewModel, ErrorModel>> GetAsync(long profileId, long userId)
    {
        var userProfile = await profileRepository.GetAsync(profileId);

        if (userProfile is null)
        {
            return UserProfileErrorHelper.ProfileNotFound();
        }

        if (!userProfile.IsVisibleFor(userId))
        {
            return UserProfileErrorHelper.ProfileAccessDenied();
        }

        return mapper.Map<UserProfileViewModel>(userProfile);
    }
}