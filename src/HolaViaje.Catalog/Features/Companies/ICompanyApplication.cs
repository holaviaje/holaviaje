using HolaViaje.Catalog.Features.Companies.Models;
using HolaViaje.Catalog.Shared.Models;
using HolaViaje.Global.Shared.Models;
using OneOf;

namespace HolaViaje.Catalog.Features.Companies
{
    public interface ICompanyApplication
    {
        Task<OneOf<CompanyViewModel, ErrorModel>> CreateAsync(CompanyModel model, long userId, CancellationToken cancellationToken);
        Task<OneOf<CompanyViewModel, ErrorModel>> DeleteAsync(Guid companyId, long userId, CancellationToken cancellationToken);
        Task<OneOf<CompanyViewModel, ErrorModel>> GetAsync(Guid companyId, CancellationToken cancellationToken);
        Task<OneOf<CompanyViewModel, ErrorModel>> UpdateAsync(Guid companyId, CompanyModel model, long userId, CancellationToken cancellationToken);
        Task<OneOf<CompanyViewModel, ErrorModel>> UpdateBookInfoAsync(Guid companyId, BookInfoModel? model, long userId, CancellationToken cancellationToken);
        Task<OneOf<CompanyViewModel, ErrorModel>> UpdateManagersAsync(Guid companyId, IEnumerable<ManagerModel> models, long userId, CancellationToken cancellationToken);
    }
}