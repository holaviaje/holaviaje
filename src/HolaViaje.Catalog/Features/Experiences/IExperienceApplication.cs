using HolaViaje.Catalog.Features.Experiences.Models;
using HolaViaje.Global.Shared.Models;
using OneOf;

namespace HolaViaje.Catalog.Features.Experiences
{
    public interface IExperienceApplication
    {
        Task<OneOf<ExperienceViewModel, ErrorModel>> AddPhotoAsync(Guid experienceId, string FileId, string ImageUrl, long userId, CancellationToken cancellationToken = default);
        Task<OneOf<ExperienceViewModel, ErrorModel>> AddTransalationAsync(Guid experienceId, ExperienceTranslationModel model, long userId, CancellationToken cancellationToken = default);
        Task<OneOf<ExperienceViewModel, ErrorModel>> CreateAsync(ExperienceModel model, long userId, CancellationToken cancellationToken = default);
        Task<OneOf<ExperienceViewModel, ErrorModel>> DeleteAsync(Guid experienceId, long userId, CancellationToken cancellationToken = default);
        Task<OneOf<ExperienceViewModel, ErrorModel>> DeleteTransalationAsync(Guid experienceId, string languageCode, long userId, CancellationToken cancellationToken = default);
        Task<OneOf<ExperienceViewModel, ErrorModel>> RemovePhotoAsync(Guid experienceId, string FileId, long userId, CancellationToken cancellationToken = default);
        Task<OneOf<ExperienceViewModel, ErrorModel>> UpdateAsync(Guid experienceId, ExperienceModel model, long userId, CancellationToken cancellationToken = default);
        Task<OneOf<ExperienceViewModel, ErrorModel>> UpdateTransalationAsync(Guid experienceId, ExperienceTranslationModel model, long userId, CancellationToken cancellationToken = default);
        Task<OneOf<ExperienceViewModel, ErrorModel>> GetAsync(Guid experienceId, string languageCode, CancellationToken cancellationToken = default);
        Task<OneOf<ExperienceViewModel, ErrorModel>> GetAsync(Guid experienceId, string[] languageCodes, CancellationToken cancellationToken = default);
    }
}