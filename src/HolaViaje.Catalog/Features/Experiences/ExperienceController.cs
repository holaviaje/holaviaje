using HolaViaje.Catalog.Features.Experiences.Models;
using HolaViaje.Catalog.Shared;
using HolaViaje.Global.Handlers;
using HolaViaje.Global.Shared.Models;
using HolaViaje.Infrastructure.BlobStorage;
using HolaViaje.Infrastructure.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HolaViaje.Catalog.Features.Experiences;

[Route("[controller]")]
[ApiController]
public class ExperienceController(IExperienceApplication experienceApplication, IBlobRepository blobRepository) : ControllerCore
{
    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(ExperienceViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateAsync(ExperienceModel model, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ErrorModel(400, "Invalid experience model."));
        }

        var result = await experienceApplication.CreateAsync(model, UserIdentity, cancellationToken);

        return result.Match<IActionResult>(
            experience => Ok(experience),
            error => BadRequest(error));
    }

    [Authorize]
    [HttpPut("{experienceId}")]
    [ProducesResponseType(typeof(ExperienceViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateAsync(Guid experienceId, ExperienceModel model, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ErrorModel(400, "Invalid experience model."));
        }

        var result = await experienceApplication.UpdateAsync(experienceId, model, UserIdentity, cancellationToken);

        return result.Match<IActionResult>(
            experience => Ok(experience),
            error => BadRequest(error));
    }

    [Authorize]
    [HttpDelete("{experienceId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteAsync(Guid experienceId, CancellationToken cancellationToken = default)
    {
        var result = await experienceApplication.DeleteAsync(experienceId, UserIdentity, cancellationToken);

        return result.Match<IActionResult>(
            experience => NoContent(),
            error => BadRequest(error));
    }

    [HttpGet("{experienceId}")]
    [ProducesResponseType(typeof(ExperienceViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAsync(Guid experienceId, CancellationToken cancellationToken = default)
    {
        var result = await experienceApplication.GetAsync(experienceId, string.Empty, cancellationToken);

        return result.Match<IActionResult>(
            experience => Ok(experience),
            error => BadRequest(error));
    }

    [Authorize]
    [HttpPost("{experienceId}/translations")]
    [ProducesResponseType(typeof(ExperienceViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddTranslationAsync(Guid experienceId, ExperienceTranslationModel model, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ErrorModel(400, "Invalid experience translation model."));
        }

        var result = await experienceApplication.AddTransalationAsync(experienceId, model, UserIdentity, cancellationToken);

        return result.Match<IActionResult>(
            experience => Ok(experience),
            error => BadRequest(error));
    }

    [Authorize]
    [HttpPut("{experienceId}/translations/{languageCode}")]
    [ProducesResponseType(typeof(ExperienceViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateTranslationAsync(Guid experienceId, string languageCode, ExperienceTranslationModel model, CancellationToken cancellationToken = default)
    {
        if (languageCode != model.LanguageCode)
        {
            return BadRequest(new ErrorModel(400, "Language code in the URL does not match the language code in the model."));
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(new ErrorModel(400, "Invalid experience model."));
        }

        var result = await experienceApplication.UpdateTransalationAsync(experienceId, model, UserIdentity, cancellationToken);

        return result.Match<IActionResult>(
            experience => Ok(experience),
            error => BadRequest(error));
    }

    [Authorize]
    [HttpDelete("{experienceId}/translations/{languageCode}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteTranslationAsync(Guid experienceId, string languageCode, CancellationToken cancellationToken = default)
    {
        var result = await experienceApplication.DeleteTransalationAsync(experienceId, languageCode, UserIdentity, cancellationToken);

        return result.Match<IActionResult>(
            experience => NoContent(),
            error => BadRequest(error));
    }

    [HttpGet("{experienceId}/translatations/{languageCode}")]
    [ProducesResponseType(typeof(ExperienceViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAsync(Guid experienceId, string languageCode, CancellationToken cancellationToken = default)
    {
        var result = await experienceApplication.GetAsync(experienceId, languageCode, cancellationToken);

        return result.Match<IActionResult>(
            experience => Ok(experience),
            error => BadRequest(error));
    }

    [Authorize]
    [HttpPost("{experienceId}/photos")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddPhotoAsync(Guid experienceId, IFormFile photoFile)
    {
        if (photoFile is null)
            return BadRequest(new ErrorModel(400, "File is null"));

        var fileModel = await PictureFileHandler.ProcessFile(photoFile, Photo.ValidExtensions, Photo.MaxSize);

        if (!fileModel.success)
            return Conflict(new ErrorModel(409, fileModel.message));
        if (fileModel.fileStream == null)
            return Conflict(new ErrorModel(409, "Stream is null"));

        var filename = $"{Guid.NewGuid().ToString("N")}{Path.GetExtension(photoFile.FileName)}";
        var imageUrl = await blobRepository.UploadAsync(Photo.ContainerName, filename, fileModel.fileStream);

        var result = await experienceApplication.AddPhotoAsync(experienceId, filename, imageUrl, UserIdentity);

        return result.Match<IActionResult>(
            profile => Ok(profile),
            error => BadRequest(error));
    }

    [Authorize]
    [HttpDelete("{experienceId}/photos/{fileId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeletePhotoAsync(Guid experienceId, string fileId, CancellationToken cancellationToken = default)
    {
        var result = await experienceApplication.RemovePhotoAsync(experienceId, fileId, UserIdentity, cancellationToken);

        return result.Match<IActionResult>(
            experience => NoContent(),
            error => BadRequest(error));
    }
}
