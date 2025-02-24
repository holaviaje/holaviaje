using HolaViaje.Infrastructure.Controllers;
using HolaViaje.Social.Features.Posts.Models;
using HolaViaje.Social.Features.Profiles.Models;
using HolaViaje.Social.Shared.Models;
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
    [HttpPost("{postId}/files/uploadLink")]
    [ProducesResponseType(typeof(MediaFileModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateUploadLinkAsync(long postId, UploadLinkModel model, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ErrorModel(400, "Invalid post model."));
        }

        var result = await postApplication.CreateUploadLinkAsync(postId, model, UserIdentity, cancellationToken);

        return result.Match<IActionResult>(
            post => Ok(post),
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
    [HttpDelete("{postId}/files/{fileId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteFileAsyn(long postId, string fileId, CancellationToken cancellationToken = default)
    {
        var result = await postApplication.DeleteMediaFileAsync(postId, fileId, UserIdentity, cancellationToken);

        return result.Match<IActionResult>(
            file => NoContent(),
            error => BadRequest(error));
    }

    [Authorize]
    [HttpDelete("{postId}/")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteAsyn(long postId, CancellationToken cancellationToken = default)
    {
        var result = await postApplication.DeleteAsync(postId, UserIdentity, cancellationToken);

        return result.Match<IActionResult>(
            post => NoContent(),
            error => BadRequest(error));
    }
}
