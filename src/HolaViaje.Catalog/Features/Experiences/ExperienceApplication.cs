using AutoMapper;
using HolaViaje.Catalog.Features.Experiences.Models;
using HolaViaje.Catalog.Features.Experiences.Repository;
using HolaViaje.Catalog.Shared;
using HolaViaje.Catalog.Shared.Models;
using HolaViaje.Global.Shared;
using HolaViaje.Global.Shared.Models;
using OneOf;

namespace HolaViaje.Catalog.Features.Experiences;

public class ExperienceApplication(IExperienceRepository experienceRepository, IMapper mapper)
{
    public async Task<OneOf<ExperienceViewModel, ErrorModel>> CreateAsync(ExperienceModel model, long userId, CancellationToken cancellationToken = default)
    {

        if (!model.Translations.Any())
        {
            return ExperienceErrorModelHelper.TranslationMissingError();
        }

        if (!IsLanguageCodeUnique(model.Translations.Select(t => t.LanguageCode).ToArray()))
        {
            return ExperienceErrorModelHelper.TranslationLanguageCodeMismatchError();
        }

        var experience = new Experience
        {
            PageId = model.PageId,
        };

        ApplyExperienceUpdates(experience, model);

        foreach (var translationModel in model.Translations)
        {
            var translation = new ExperienceTranslation
            {
                Experience = experience,
                LanguageCode = translationModel.LanguageCode
            };

            ApplyTranslationUpdates(translation, translationModel);
            experience.AddTranslation(translation);
        }

        var result = await experienceRepository.CreateAsync(experience);

        return mapper.Map<ExperienceViewModel>(result);
    }

    private bool IsLanguageCodeUnique(params string[] languageCodes)
    {
        return languageCodes.Distinct().Count() == languageCodes.Length;
    }

    public async Task<OneOf<ExperienceViewModel, ErrorModel>> UpdateAsync(Guid experienceId, ExperienceModel model, long userId, CancellationToken cancellationToken = default)
    {
        var experience = await experienceRepository.GetAsync(experienceId, model.Translations.Select(t => t.LanguageCode).ToArray(), true, cancellationToken);

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

        var result = await experienceRepository.UpdateAsync(experience);

        return mapper.Map<ExperienceViewModel>(result);
    }

    private void ApplyExperienceUpdates(Experience experience, ExperienceModel model)
    {
        experience.BookInfirmation = model.BookInfirmation?.FromModel();
        experience.CancellationPolicy = model.CancellationPolicy?.FromModel();
        experience.PickupAvailable = model.PickupAvailable;
        experience.InstantTicketDelivery = model.InstantTicketDelivery;
        experience.MobileTicket = model.MobileTicket;
        experience.WheelchairAccessible = model.WheelchairAccessible;
        experience.MaxGuests = model.MaxGuests;
        experience.IsAvailable = model.IsAvailable;

        experience.SetTimeRange(model.TimeRange?.FromModel());
        experience.SetCancellationPolicy(model.CancellationPolicy?.FromModel());
        experience.SetBookInformation(model.BookInfirmation?.FromModel());
    }


    private void ApplyTranslationUpdates(ExperienceTranslation translation, ExperienceTransalationModel model)
    {
        translation.Title = model.Title;
        translation.Description = model.Description;
        translation.CancellationPolicyName = model.CancellationPolicyName;
        translation.CancellationPolicyDetails = model.CancellationPolicyDetails;
        translation.ImportantInformation = model.ImportantInformation;
        translation.WhatToExpect = model.WhatToExpect;

        translation.SetServices(model.Services.FromModel());
        translation.SetAdditionalInfos(model.AdditionalInfos.FromModel());
        translation.SetStops(model.Stops.FromModel());
        translation.SetPlace(model.Place?.FromModel() ?? new PlaceInfo());
        translation.SetPickup(model.Pickup?.FromModel() ?? new Pickup());
    }

    public async Task<OneOf<ExperienceViewModel, ErrorModel>> AddTransalationAsync(Guid experienceId, ExperienceTransalationModel model, long userId, CancellationToken cancellationToken = default)
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

    public async Task<OneOf<ExperienceViewModel, ErrorModel>> UpdateTransalationAsync(Guid experienceId, ExperienceTransalationModel model, long userId, CancellationToken cancellationToken = default)
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

    public async Task<OneOf<ExperienceViewModel, ErrorModel>> DeleteTransalationAsync(Guid experienceId, string languageCode, long userId, CancellationToken cancellationToken = default)
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
