using HolaViaje.Social.Features.Profiles.Models;
using HolaViaje.Social.Shared.Models;
using OneOf;

namespace HolaViaje.Social.Features.Profiles;

/// <summary>
/// User profile application service
/// </summary>
public interface IUserProfileApplication
{
    /// <summary>
    /// Create a new user profile
    /// </summary>
    /// <param name="userId">User identifier that wants access to profile.</param>
    /// <returns></returns>
    Task<OneOf<UserProfileViewModel, ErrorModel>> CreateAsync(long userId);
    /// <summary>
    /// Update an existing user profile
    /// </summary>
    /// <param name="profileId">Profile identifier.</param>
    /// <param name="model"></param>
    /// <param name="userId">User identifier that wants access to profile.</param>
    /// <returns></returns>
    Task<OneOf<UserProfileViewModel, ErrorModel>> UpdateAsync(long profileId, UserProfileModel model, long userId);
    /// <summary>
    /// Update the availability of a user profile
    /// </summary>
    /// <param name="profileId">Profile identifier.</param>
    /// <param name="model"></param>
    /// <param name="userId">User identifier that wants access to profile.</param>
    /// <returns></returns>
    Task<OneOf<UserProfileViewModel, ErrorModel>> UpdateAvailabilityAsync(long profileId, AvailabilityModel model, long userId);
    /// <summary>
    /// Update the place of a user profile
    /// </summary>
    /// <param name="profileId">Profile identifier.</param>
    /// <param name="model"></param>
    /// <param name="userId">User identifier that wants access to profile.</param>
    /// <returns></returns>
    Task<OneOf<UserProfileViewModel, ErrorModel>> UpdatePlaceAsync(long profileId, PlaceInfoModel model, long userId);
    /// <summary>
    /// Update the spoken languages of a user profile
    /// </summary>
    /// <param name="profileId">Profile identifier.</param>
    /// <param name="models"></param>
    /// <param name="userId">User identifier that wants access to profile.</param>
    /// <returns></returns>
    Task<OneOf<UserProfileViewModel, ErrorModel>> UpdateSpokenLanguagesAsync(long profileId, IEnumerable<SpokenLanguageModel> models, long userId);
    /// <summary>
    /// Delete a user profile
    /// </summary>
    /// <param name="profileId">Profile identifier.</param>
    /// <param name="userId">User identifier that wants access to profile.</param>
    /// <returns>Profile info or error.</returns>
    Task<OneOf<UserProfileViewModel, ErrorModel>> DeleteAsync(long profileId, long userId);
    /// <summary>
    /// Get a user profile
    /// </summary>
    /// <param name="profileId">Profile identifier.</param>
    /// <param name="userId">User identifier that wants access to profile.</param>
    /// <returns>Profile info or error.</returns>
    Task<OneOf<UserProfileViewModel, ErrorModel>> GetAsync(long profileId, long userId);
}
