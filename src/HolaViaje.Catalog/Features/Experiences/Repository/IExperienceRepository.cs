namespace HolaViaje.Catalog.Features.Experiences.Repository;

public interface IExperienceRepository
{
    Task<Experience?> GetAsync(Guid id, bool tracking = false, CancellationToken cancellationToken = default);
    Task<Experience?> GetWithTranslationAsync(Guid id, string languageCode, bool tracking = false, CancellationToken cancellationToken = default);
    Task<Experience> CreateAsync(Experience experience, CancellationToken cancellationToken = default);
    Task<Experience> UpdateAsync(Experience experience, CancellationToken cancellationToken = default);
}
