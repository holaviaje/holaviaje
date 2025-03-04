using AutoMapper;
using HolaViaje.Catalog.Features.Experiences.Models;
using HolaViaje.Catalog.Features.Experiences.Repository;
using HolaViaje.Catalog.Shared;
using HolaViaje.Catalog.Shared.Models;
using HolaViaje.Global.Helpers;
using HolaViaje.Global.Shared;
using HolaViaje.Global.Shared.Models;
using OneOf;

namespace HolaViaje.Catalog.Features.Experiences;

public class ExperienceApplication(IExperienceRepository experienceRepository, IMapper mapper) : IExperienceApplication
{
    public async Task<OneOf<ExperienceViewModel, ErrorModel>> CreateAsync(ExperienceModel model, long userId, CancellationToken cancellationToken = default)
    {

        if (!model.Translations.Any())
        {
            return ExperienceErrorModelHelper.TranslationMissingError();
        }

        var languageCodes = model.Translations.Select(t => t.LanguageCode).ToList();
        if (!IsLanguageCodeUnique(languageCodes))
        {
            return ExperienceErrorModelHelper.TranslationLanguageCodeMismatchError();
        }

        var experience = new Experience
        {
            CompanyId = model.PageId,
        };

        ApplyExperienceUpdates(experience, model);

        foreach (var translationModel in model.Translations)
        {
            var translation = new ExperienceTranslation
            {
                Experience = experience,
                LanguageCode = translationModel.LanguageCode ?? LanguageCodeHelper.DefaultLanguageCode
            };

            ApplyTranslationUpdates(translation, translationModel);
            experience.AddTranslation(translation);
        }

        var result = await experienceRepository.CreateAsync(experience);

        return mapper.Map<ExperienceViewModel>(result);
    }

    private bool IsLanguageCodeUnique(IEnumerable<string?> languageCodes)
    {
        return languageCodes.Distinct().Count() == languageCodes.Count();
    }

    public async Task<OneOf<ExperienceViewModel, ErrorModel>> UpdateAsync(Guid experienceId, ExperienceModel model, long userId, CancellationToken cancellationToken = default)
    {
        var experience = await experienceRepository.GetAsync(experienceId, [.. model.Translations.Select(t => t.LanguageCode)], true, cancellationToken);

        if (experience == null)
        {
            return ExperienceErrorModelHelper.ExperienceNotFoundError();
        }

        ApplyExperienceUpdates(experience, model);

        foreach (var translationModel in model.Translations)
        {
            var translation = experience.Translations.FirstOrDefault(x => x.LanguageCode == translationModel.LanguageCode);

            if (translation != null)
            {
                ApplyTranslationUpdates(translation, translationModel);
            }
        }

        var result = await experienceRepository.UpdateAsync(experience, cancellationToken);

        return mapper.Map<ExperienceViewModel>(result);
    }

    private void ApplyExperienceUpdates(Experience experience, ExperienceModel model)
    {
        experience.CancellationPolicy = model.CancellationPolicy.FromModel();
        experience.InstantTicketDelivery = model.InstantTicketDelivery;
        experience.MobileTicket = model.MobileTicket;
        experience.WheelchairAccessible = model.WheelchairAccessible;
        experience.MaxGuests = model.MaxGuests;
        experience.IsAvailable = model.IsAvailable;

        experience.SetCancellationPolicy(model.CancellationPolicy.FromModel());
    }


    private void ApplyTranslationUpdates(ExperienceTranslation translation, ExperienceTranslationModel model)
    {
        translation.Title = model.Title;
        translation.Description = model.Description;
        translation.CancellationPolicyName = model.CancellationPolicyName;
        translation.CancellationPolicyDetails = model.CancellationPolicyDetails;
        translation.ImportantInformation = model.ImportantInformation;
        translation.WhatToExpect = model.WhatToExpect;

        translation.SetServices(model.Services.FromModel());
        translation.SetPickupPoints(model.PickupPoints.FromModel());
        translation.SetPickupPoints(model.MeetingPoints.FromModel());
        translation.SetEndPoint(model.EndPoint?.FromModel() ?? translation.EndPoint ?? new());
        translation.SetTicketRedemptionPoint(model.TicketRedemptionPoint?.FromModel() ?? translation.TicketRedemptionPoint ?? new());
        translation.SetAdditionalInfos(model.AdditionalInfos.FromModel());
        translation.SetStops(model.Stops.FromModel());
        translation.SetPlace(model.Place?.FromModel() ?? translation.Place ?? new MapPoint());
    }

    public async Task<OneOf<ExperienceViewModel, ErrorModel>> AddTranslationAsync(Guid experienceId, ExperienceTranslationModel model, long userId, CancellationToken cancellationToken = default)
    {

        if (model == null)
        {
            return ExperienceErrorModelHelper.TranslationMissingError();
        }

        if (string.IsNullOrEmpty(model.LanguageCode))
        {
            return ExperienceErrorModelHelper.LanguageCodeMissingError();
        }

        var experience = await experienceRepository.GetAsync(experienceId, model.LanguageCode, true, cancellationToken);

        if (experience == null)
        {
            return ExperienceErrorModelHelper.ExperienceNotFoundError();
        }

        if (experience.Translations.Any())
        {
            return ExperienceErrorModelHelper.TranslationAlreadyExistsError();
        }

        var translation = new ExperienceTranslation
        {
            Experience = experience,
            LanguageCode = model.LanguageCode

        };

        ApplyTranslationUpdates(translation, model);

        experience.AddTranslation(translation);

        var result = await experienceRepository.UpdateAsync(experience);

        return mapper.Map<ExperienceViewModel>(result);
    }

    public async Task<OneOf<ExperienceViewModel, ErrorModel>> UpdateTranslationAsync(Guid experienceId, ExperienceTranslationModel model, long userId, CancellationToken cancellationToken = default)
    {

        if (model == null)
        {
            return ExperienceErrorModelHelper.TranslationMissingError();
        }

        var experience = await experienceRepository.GetAsync(experienceId, model.LanguageCode, true, cancellationToken);

        if (experience == null)
        {
            return ExperienceErrorModelHelper.ExperienceNotFoundError();
        }

        if (!experience.Translations.Any())
        {
            return ExperienceErrorModelHelper.TranslationMissingError();
        }

        var translation = experience.Translations.FirstOrDefault();

        if (translation == null)
        {
            return ExperienceErrorModelHelper.TranslationMissingError();
        }

        if (translation.LanguageCode != model.LanguageCode)
        {
            return ExperienceErrorModelHelper.TranslationLanguageCodeMismatchError();
        }

        ApplyTranslationUpdates(translation, model);
        experience.UpdateLastModified();

        var result = await experienceRepository.UpdateAsync(experience);

        return mapper.Map<ExperienceViewModel>(result);
    }

    public async Task<OneOf<ExperienceViewModel, ErrorModel>> DeleteTranslationAsync(Guid experienceId, string languageCode, long userId, CancellationToken cancellationToken = default)
    {

        var experience = await experienceRepository.GetAsync(experienceId, languageCode, true, cancellationToken);

        if (experience == null)
        {
            return ExperienceErrorModelHelper.ExperienceNotFoundError();
        }

        if (!experience.Translations.Any())
        {
            return ExperienceErrorModelHelper.TranslationMissingError();
        }

        var translationsCount = await experienceRepository.CountTranslationsAsync(experienceId, cancellationToken);

        if (translationsCount <= 1)
        {
            return ExperienceErrorModelHelper.TranslationCannotBeDeletedError();
        }

        var translation = experience.Translations.FirstOrDefault();

        if (translation == null)
        {
            return ExperienceErrorModelHelper.TranslationMissingError();
        }

        if (translation.LanguageCode != languageCode)
        {
            return ExperienceErrorModelHelper.TranslationLanguageCodeMismatchError();
        }

        experience.RemoveTranslation(translation);

        var result = await experienceRepository.UpdateAsync(experience);

        return mapper.Map<ExperienceViewModel>(result);
    }

    public async Task<OneOf<ExperienceViewModel, ErrorModel>> DeleteAsync(Guid experienceId, long userId, CancellationToken cancellationToken = default)
    {

        var experience = await experienceRepository.GetAsync(experienceId, string.Empty, true, cancellationToken);

        if (experience == null)
        {
            return ExperienceErrorModelHelper.ExperienceNotFoundError();
        }

        experience.Delete();

        var result = await experienceRepository.UpdateAsync(experience);

        return mapper.Map<ExperienceViewModel>(result);
    }

    public async Task<OneOf<ExperienceViewModel, ErrorModel>> GetAsync(Guid experienceId, string languageCode, CancellationToken cancellationToken = default)
    {

        var experience = await experienceRepository.GetAsync(experienceId, languageCode, true, cancellationToken);

        if (experience == null)
        {
            return ExperienceErrorModelHelper.ExperienceNotFoundError();
        }

        return mapper.Map<ExperienceViewModel>(experience);
    }

    public async Task<OneOf<ExperienceViewModel, ErrorModel>> GetAsync(Guid experienceId, string[] languageCodes, CancellationToken cancellationToken = default)
    {

        var experience = await experienceRepository.GetAsync(experienceId, languageCodes, true, cancellationToken);

        if (experience == null)
        {
            return ExperienceErrorModelHelper.ExperienceNotFoundError();
        }

        return mapper.Map<ExperienceViewModel>(experience);
    }

    public async Task<OneOf<ExperienceViewModel, ErrorModel>> AddPhotoAsync(Guid experienceId, string FileId, string ImageUrl, long userId, CancellationToken cancellationToken = default)
    {

        var experience = await experienceRepository.GetAsync(experienceId, string.Empty, true, cancellationToken);

        if (experience == null)
        {
            return ExperienceErrorModelHelper.ExperienceNotFoundError();
        }

        var photo = new Photo(FileId, ImageUrl);
        experience.AddPhoto(photo);

        var result = await experienceRepository.UpdateAsync(experience);

        return mapper.Map<ExperienceViewModel>(result);
    }

    public async Task<OneOf<ExperienceViewModel, ErrorModel>> RemovePhotoAsync(Guid experienceId, string FileId, long userId, CancellationToken cancellationToken = default)
    {

        var experience = await experienceRepository.GetAsync(experienceId, string.Empty, true, cancellationToken);

        if (experience == null)
        {
            return ExperienceErrorModelHelper.ExperienceNotFoundError();
        }

        experience.RemovePhoto(FileId);

        var result = await experienceRepository.UpdateAsync(experience);

        return mapper.Map<ExperienceViewModel>(result);
    }
}
