using AutoMapper;
using HolaViaje.Catalog.Features.Companies.Models;
using HolaViaje.Catalog.Features.Companies.Repository;
using HolaViaje.Catalog.Shared.Models;
using HolaViaje.Global.Shared.Models;
using OneOf;

namespace HolaViaje.Catalog.Features.Companies;

public class CompanyApplication(ICompanyRepository companyRepository, IMapper mapper) : ICompanyApplication
{
    public async Task<OneOf<CompanyViewModel, ErrorModel>> CreateAsync(CompanyModel model, long userId, CancellationToken cancellationToken)
    {
        var company = new Company(userId);

        ApplyCompanyChanges(company, model);

        var dbEntity = await companyRepository.CreateAsync(company, cancellationToken);
        return mapper.Map<CompanyViewModel>(dbEntity);
    }

    public async Task<OneOf<CompanyViewModel, ErrorModel>> UpdateAsync(Guid companyId, CompanyModel model, long userId, CancellationToken cancellationToken)
    {
        var company = await companyRepository.GetAsync(companyId, true, cancellationToken);

        if (company == null)
        {
            return CompanyErrorModelHelper.CompanyNotFoundError();
        }

        if (!company.IsAdmin(userId))
        {
            return CompanyErrorModelHelper.UnauthorizedError();
        }

        ApplyCompanyChanges(company, model);

        var dbEntity = await companyRepository.UpdateAsync(company, cancellationToken);
        return mapper.Map<CompanyViewModel>(dbEntity);
    }

    public async Task<OneOf<CompanyViewModel, ErrorModel>> DeleteAsync(Guid companyId, long userId, CancellationToken cancellationToken)
    {
        var company = await companyRepository.GetAsync(companyId, true, cancellationToken);

        if (company == null)
        {
            return CompanyErrorModelHelper.CompanyNotFoundError();
        }

        if (company.IsDeleted())
        {
            return CompanyErrorModelHelper.CompanyNotFoundError();
        }

        if (!company.IsAdmin(userId))
        {
            return CompanyErrorModelHelper.UnauthorizedError();
        }

        company.Delete();

        var dbEntity = await companyRepository.UpdateAsync(company, cancellationToken);
        return mapper.Map<CompanyViewModel>(dbEntity);
    }

    public async Task<OneOf<CompanyViewModel, ErrorModel>> UpdateBookInfoAsync(Guid companyId, BookInfoModel model, long userId, CancellationToken cancellationToken)
    {
        var company = await companyRepository.GetAsync(companyId, true, cancellationToken);

        if (company == null)
        {
            return CompanyErrorModelHelper.CompanyNotFoundError();
        }

        if (!company.IsAdmin(userId))
        {
            return CompanyErrorModelHelper.UnauthorizedError();
        }

        company.SetBookInfo(model.FromModel());

        var dbEntity = await companyRepository.UpdateAsync(company, cancellationToken);
        return mapper.Map<CompanyViewModel>(dbEntity);
    }

    public async Task<OneOf<CompanyViewModel, ErrorModel>> UpdateManagersAsync(Guid companyId, IEnumerable<ManagerModel> models, long userId, CancellationToken cancellationToken)
    {
        var company = await companyRepository.GetAsync(companyId, true, cancellationToken);

        if (company == null)
        {
            return CompanyErrorModelHelper.CompanyNotFoundError();
        }

        if (!company.IsAdmin(userId))
        {
            return CompanyErrorModelHelper.UnauthorizedError();
        }

        company.SetManagers(models.FromModel());

        var dbEntity = await companyRepository.UpdateAsync(company, cancellationToken);
        return mapper.Map<CompanyViewModel>(dbEntity);
    }

    public async Task<OneOf<CompanyViewModel, ErrorModel>> GetAsync(Guid companyId, CancellationToken cancellationToken)
    {
        var company = await companyRepository.GetAsync(companyId, true, cancellationToken);

        if (company == null)
        {
            return CompanyErrorModelHelper.CompanyNotFoundError();
        }

        return mapper.Map<CompanyViewModel>(company);
    }

    private void ApplyCompanyChanges(Company company, CompanyModel companyModel)
    {
        company.Name = companyModel.Name;
        company.LegalName = companyModel.LegalName;
        company.Address1 = companyModel.Address1;
        company.Address2 = companyModel.Address2;
        company.City = companyModel.City;
        company.State = companyModel.State;
        company.ZipCode = companyModel.ZipCode;
        company.Country = companyModel.Country;
        company.Phone = companyModel.Phone;
        company.Email = companyModel.Email;
        company.Website = companyModel.Website;
        company.RegistrationNumber = companyModel.RegistrationNumber;
        company.RegisteredIn = companyModel.RegisteredIn;
        company.VatId = companyModel.VatId;
        company.UpdateLastModified();
    }
}
