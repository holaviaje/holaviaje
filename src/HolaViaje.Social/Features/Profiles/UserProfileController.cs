using HolaViaje.Social.Controllers;
using HolaViaje.Social.Features.Profiles.Models;
using HolaViaje.Social.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HolaViaje.Social.Features.Profiles;

[Route("[controller]")]
[ApiController]
public class UserProfileController(IUserProfileApplication profileApplication) : ControllerCore
{
    [Authorize]
    [HttpGet]
    [ProducesResponseType(typeof(UserProfileViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAsync()
    {
        var result = await profileApplication.GetAsync(UserIdentity, UserIdentity);

        return result.Match<IActionResult>(
            profile => Ok(profile),
            error => BadRequest(error));
    }

    [Authorize]
    [HttpGet("{profileId}")]
    [ProducesResponseType(typeof(UserProfileViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAsync(long profileId)
    {
        var result = await profileApplication.GetAsync(profileId, UserIdentity);

        return result.Match<IActionResult>(
            profile => Ok(profile),
            error => BadRequest(error));
    }

    [Authorize]
    [HttpPut("{profileId}")]
    [ProducesResponseType(typeof(UserProfileViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateAsync(long profileId, UserProfileModel model)
    {
        var result = await profileApplication.UpdateAsync(profileId, model, UserIdentity);

        return result.Match<IActionResult>(
            profile => Ok(profile),
            error => BadRequest(error));
    }

    [Authorize]
    [HttpPut("{profileId}/Availability")]
    [ProducesResponseType(typeof(UserProfileViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateAvailabilityAsync(long profileId, AvailabilityModel model)
    {
        var result = await profileApplication.UpdateAvailabilityAsync(profileId, model, UserIdentity);

        return result.Match<IActionResult>(
            profile => Ok(profile),
            error => BadRequest(error));
    }

    [Authorize]
    [HttpPut("{profileId}/Place")]
    [ProducesResponseType(typeof(UserProfileViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdatePlaceAsync(long profileId, PlaceInfoModel model)
    {
        var result = await profileApplication.UpdatePlaceAsync(profileId, model, UserIdentity);

        return result.Match<IActionResult>(
            profile => Ok(profile),
            error => BadRequest(error));
    }

    [Authorize]
    [HttpPut("{profileId}/SpokenLanguages")]
    [ProducesResponseType(typeof(UserProfileViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateSpokenLanguagesAsync(long profileId, List<SpokenLanguageModel> models)
    {
        var result = await profileApplication.UpdateSpokenLanguagesAsync(profileId, models, UserIdentity);

        return result.Match<IActionResult>(
            profile => Ok(profile),
            error => BadRequest(error));
    }
}