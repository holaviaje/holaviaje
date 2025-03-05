using HolaViaje.Global.Shared.Models;

namespace HolaViaje.Catalog.Features.Experiences;

public static class ExperienceErrorModelHelper
{
    public static ErrorModel TranslationMissingError()
    {
        return new ErrorModel(400, "Translation not found");
    }

    public static ErrorModel ExperienceNotFoundError()
    {
        return new ErrorModel(400, "Experience not found");
    }

    public static ErrorModel TranslationAlreadyExistsError()
    {
        return new ErrorModel(400, "Translation already exists.");
    }

    public static ErrorModel TranslationLanguageCodeMismatchError()
    {
        return new ErrorModel(400, "Translation language code mismatch.");
    }

    public static ErrorModel TranslationCannotBeDeletedError()
    {
        return new ErrorModel(400, "The experience must have at least one translation.");
    }

    public static ErrorModel LanguageCodeMissingError()
    {
        return new ErrorModel(400, "The language code is required.");
    }
}