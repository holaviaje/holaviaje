﻿using HolaViaje.Infrastructure.Controllers;
using HolaViaje.Social.Features.Posts.Models;
using HolaViaje.Social.Features.Profiles.Models;
using HolaViaje.Social.Shared.Models;
using HolaViaje.Social.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HolaViaje.Social.Features.Posts;

[Route("[controller]")]
[ApiController]
public class PostController(IPostApplication postApplication) : ControllerCore
{
    [Authorize]
    [HttpGet("{postId}")]
    [ProducesResponseType(typeof(PostViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAsync(long postId, CancellationToken cancellationToken = default)
    {
        var result = await postApplication.GetAsync(postId, UserIdentity, cancellationToken);

        return result.Match<IActionResult>(
            post => Ok(post),
            error => BadRequest(error));
    }

    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(UserProfileViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateAsync(PostModel model, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ErrorModel(400, "Invalid post model."));
        }

        var result = await postApplication.CreateAsync(model, UserIdentity, cancellationToken);

        return result.Match<IActionResult>(
            post => Ok(post),
            error => BadRequest(error));
    }

    [Authorize]
    [HttpPut("{postId}")]
    [ProducesResponseType(typeof(UserProfileViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateAsync(long postId, PostBasicModel model, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ErrorModel(400, "Invalid post model."));
        }

        var result = await postApplication.UpdateAsync(postId, model, UserIdentity, cancellationToken);

        return result.Match<IActionResult>(
            post => Ok(post),
            error => BadRequest(error));
    }

    [Authorize]
    [HttpPost("{postId}/files/upload")]
    [ProducesResponseType(typeof(MediaFileModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UploadMediaFileAsync(long postId, IFormFile mediaFile, CancellationToken cancellationToken = default)
    {
        if (mediaFile is null)
            return BadRequest(new ErrorModel(400, "File is null"));

        var (success, message) = FileValidator.Validate(Path.GetExtension(mediaFile.FileName), mediaFile.Length, MediaFile.ValidExtensions, MediaFile.MaxDirectUploadSize);

        if (!success)
            return BadRequest(new ErrorModel(400, message));

        var model = new UploadMediaFileModel(mediaFile.FileName, mediaFile.ContentType, mediaFile.Length, mediaFile.OpenReadStream());
        var result = await postApplication.UploadMediaFileAsync(postId, model, UserIdentity, cancellationToken);

        return result.Match<IActionResult>(
            file => Created(string.Empty, file),
            error => BadRequest(error));
    }

    [Authorize]
    [HttpPost("{postId}/files/uploadLink")]
    [ProducesResponseType(typeof(MediaFileModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateUploadLinkAsync(long postId, UploadLinkModel model, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ErrorModel(400, "Invalid post model."));
        }

        var extension = Path.GetExtension(model.FileName);
        var maxFileSize = MediaFile.ImageValidExtensions.Contains(extension) ? MediaFile.ImageMaxSize : MediaFile.VideoMaxSize;
        var (success, message) = FileValidator.Validate(extension, model.FileSize, MediaFile.ValidExtensions, maxFileSize);

        if (!success)
            return BadRequest(new ErrorModel(400, message));

        var result = await postApplication.CreateUploadLinkAsync(postId, model, UserIdentity, cancellationToken);

        return result.Match<IActionResult>(
            file => Created(string.Empty, file),
            error => BadRequest(error));
    }

    [Authorize]
    [HttpPost("{postId}/publish")]
    [ProducesResponseType(typeof(UserProfileViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PublishAsync(long postId, CancellationToken cancellationToken = default)
    {
        var result = await postApplication.PublishAsync(postId, UserIdentity, cancellationToken);

        return result.Match<IActionResult>(
            post => Ok(post),
            error => BadRequest(error));
    }

    [Authorize]
    [HttpPost("{postId}/files/{fileId}/uploaded")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> MarkAsUploadedAsync(long postId, string fileId, CancellationToken cancellationToken = default)
    {
        var result = await postApplication.MarkAsUploadedAsync(postId, fileId, UserIdentity, cancellationToken);

        return result.Match<IActionResult>(
            file => Ok(),
            error => BadRequest(error));
    }

    [Authorize]
    [HttpDelete("{postId}/files/{fileId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteFileAsync(long postId, string fileId, CancellationToken cancellationToken = default)
    {
        var result = await postApplication.DeleteMediaFileAsync(postId, fileId, UserIdentity, cancellationToken);

        return result.Match<IActionResult>(
            file => NoContent(),
            error => BadRequest(error));
    }

    [Authorize]
    [HttpDelete("{postId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteAsync(long postId, CancellationToken cancellationToken = default)
    {
        var result = await postApplication.DeleteAsync(postId, UserIdentity, cancellationToken);

        return result.Match<IActionResult>(
            post => NoContent(),
            error => BadRequest(error));
    }
}
