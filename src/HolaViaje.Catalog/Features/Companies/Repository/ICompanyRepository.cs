namespace HolaViaje.Catalog.Features.Companies.Repository;

public interface ICompanyRepository
{
    Task<Company> CreateAsync(Company company, CancellationToken cancellationToken);
    Task<Company> UpdateAsync(Company company, CancellationToken cancellationToken);
    Task<Company?> GetAsync(Guid id, bool tracking = false, CancellationToken cancellationToken = default);
}