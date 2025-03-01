using HolaViaje.Catalog.Features.Companies.Models;
using HolaViaje.Catalog.Shared.Models;
using HolaViaje.Global.Shared.Models;
using HolaViaje.Infrastructure.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HolaViaje.Catalog.Features.Companies;

[Route("[controller]")]
[ApiController]
public class CompanyController(ICompanyApplication companyApplication) : ControllerCore
{
    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(CompanyViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateAsync(CompanyModel model, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ErrorModel(400, "Invalid company model."));
        }

        var result = await companyApplication.CreateAsync(model, UserIdentity, cancellationToken);

        return result.Match<IActionResult>(
            company => Ok(company),
            error => BadRequest(error));
    }

    [Authorize]
    [HttpPut("{companyId}")]
    [ProducesResponseType(typeof(CompanyViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateAsync(Guid companyId, CompanyModel model, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ErrorModel(400, "Invalid company model."));
        }

        var result = await companyApplication.UpdateAsync(companyId, model, UserIdentity, cancellationToken);

        return result.Match<IActionResult>(
            company => Ok(company),
            error => BadRequest(error));
    }

    [Authorize]
    [HttpDelete("{companyId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteAsync(Guid companyId, CancellationToken cancellationToken = default)
    {
        var result = await companyApplication.DeleteAsync(companyId, UserIdentity, cancellationToken);

        return result.Match<IActionResult>(
            company => NoContent(),
            error => BadRequest(error));
    }

    [Authorize]
    [HttpPut("{companyId}/bookInfo")]
    [ProducesResponseType(typeof(CompanyViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateBookInfoAsync(Guid companyId, BookInfoModel? model, CancellationToken cancellationToken = default)
    {
        var result = await companyApplication.UpdateBookInfoAsync(companyId, model, UserIdentity, cancellationToken);

        return result.Match<IActionResult>(
            company => Ok(company),
            error => BadRequest(error));
    }

    [Authorize]
    [HttpPut("{companyId}/managers")]
    [ProducesResponseType(typeof(CompanyViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateBookInfoAsync(Guid companyId, List<ManagerModel> models, CancellationToken cancellationToken = default)
    {
        var result = await companyApplication.UpdateManagersAsync(companyId, models, UserIdentity, cancellationToken);

        return result.Match<IActionResult>(
            company => Ok(company),
            error => BadRequest(error));
    }

    [HttpGet("{companyId}")]
    [ProducesResponseType(typeof(CompanyViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAsync(Guid companyId, CancellationToken cancellationToken = default)
    {
        var result = await companyApplication.GetAsync(companyId, cancellationToken);

        return result.Match<IActionResult>(
            company => Ok(company),
            error => BadRequest(error));
    }
}
