using HolaViaje.Infrastructure.BlobStorage;
using HolaViaje.Social.Controllers;
using HolaViaje.Social.Features.Profiles.Models;
using HolaViaje.Social.Handlers;
using HolaViaje.Social.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HolaViaje.Social.Features.Profiles;

[Route("[controller]")]
[ApiController]
public class UserProfileController(IUserProfileApplication profileApplication, IBlobRepository blobRepository) : ControllerCore
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
        if (profileId != UserIdentity)
        {
            return Forbid(UserProfileErrorHelper.ProfileAccessDenied().Message);
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(new ErrorModel(400, "Invalid profile model."));
        }

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
        if (profileId != UserIdentity)
        {
            return Forbid(UserProfileErrorHelper.ProfileAccessDenied().Message);
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(new ErrorModel(400, "Invalid availability model."));
        }

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
        if (profileId != UserIdentity)
        {
            return Forbid(UserProfileErrorHelper.ProfileAccessDenied().Message);
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(new ErrorModel(400, "Invalid place info model."));
        }

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
        if (profileId != UserIdentity)
        {
            return Forbid(UserProfileErrorHelper.ProfileAccessDenied().Message);
        }

        var result = await profileApplication.UpdateSpokenLanguagesAsync(profileId, models, UserIdentity);

        return result.Match<IActionResult>(
            profile => Ok(profile),
            error => BadRequest(error));
    }

    [HttpPut("{profileId}/picture")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> UploadPicture(long profileId, IFormFile pictureFile)
    {
        if (profileId != UserIdentity)
        {
            return Forbid(UserProfileErrorHelper.ProfileAccessDenied().Message);
        }

        if (pictureFile is null)
            return BadRequest(new ErrorModel(400, "File is null"));

        var fileModel = await PictureFileHandler.ProcessFile(pictureFile, ProfilePicture.ValidExtensions, ProfilePicture.MaxSize);

        if (!fileModel.success)
            return Conflict(new ErrorModel(409, fileModel.message));
        if (fileModel.fileStream == null)
            return Conflict(new ErrorModel(409, "Stream is null"));

        var filename = $"{Guid.NewGuid().ToString("N")}{Path.GetExtension(pictureFile.FileName)}";
        var imageUrl = await blobRepository.UploadAsync(ProfilePicture.ContainerName, filename, fileModel.fileStream);

        var result = await profileApplication.UpdatePictureAsync(profileId, filename, imageUrl, UserIdentity);

        return result.Match<IActionResult>(
            profile => Ok(profile),
            error => BadRequest(error));
    }
}