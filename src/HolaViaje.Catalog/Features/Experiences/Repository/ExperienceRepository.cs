
using HolaViaje.Catalog.Data;
using Microsoft.EntityFrameworkCore;

namespace HolaViaje.Catalog.Features.Experiences.Repository;

public class ExperienceRepository(ApplicationDbContext dbContext) : IExperienceRepository
{
    private readonly DbSet<Experience> experiences = dbContext.Set<Experience>();

    private async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        if (dbContext.ChangeTracker.HasChanges())
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<Experience> CreateAsync(Experience experience, CancellationToken cancellationToken = default)
    {
        experiences.Add(experience);
        await SaveChangesAsync();
        return experience;
    }

    public async Task<Experience?> GetAsync(Guid id, bool tracking = false, CancellationToken cancellationToken = default)
    {
        var query = experiences.Where(x => x.Id == id);

        if (!tracking)
        {
            query = query.AsNoTracking();
        }

        return await query.Include(e => e.Translations).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Experience?> GetWithTranslationAsync(Guid id, string languageCode, bool tracking = false, CancellationToken cancellationToken = default)
    {
        var query = experiences.Where(x => x.Id == id);

        if (!tracking)
        {
            query = query.AsNoTracking();
        }

        var experience = await query.Include(e => e.Translations.Where(t => t.LanguageCode == languageCode)).FirstOrDefaultAsync(cancellationToken);

        if (experience == null || !experience.Translations.Any())
        {
            experience = await query.FirstOrDefaultAsync(cancellationToken);
        }

        return experience;
    }

    public async Task<Experience> UpdateAsync(Experience experience, CancellationToken cancellationToken = default)
    {
        var entry = dbContext.Entry(experience);

        if (entry?.State != EntityState.Modified)
        {
            experiences.Update(experience);
        }

        await SaveChangesAsync();
        return experience;
    }
}
