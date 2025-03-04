using HolaViaje.Catalog.Data;
using Microsoft.EntityFrameworkCore;

namespace HolaViaje.Catalog.Features.Companies.Repository;

public class CompanyRepository(ApplicationDbContext dbContext) : ICompanyRepository
{
    private readonly DbSet<Company> companies = dbContext.Set<Company>();

    private async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        if (dbContext.ChangeTracker.HasChanges())
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<Company> CreateAsync(Company company, CancellationToken cancellationToken)
    {
        companies.Add(company);
        await SaveChangesAsync();
        return company;
    }

    public async Task<Company?> GetAsync(Guid id, bool tracking = false, CancellationToken cancellationToken = default)
    {
        var query = companies.Where(x => x.Id == id);

        if (!tracking)
        {
            query = query.AsNoTracking();
        }

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Company> UpdateAsync(Company company, CancellationToken cancellationToken)
    {
        var entry = dbContext.Entry(company);

        if (entry?.State != EntityState.Modified)
        {
            companies.Update(company);
        }

        await SaveChangesAsync();
        return company;
    }
}
