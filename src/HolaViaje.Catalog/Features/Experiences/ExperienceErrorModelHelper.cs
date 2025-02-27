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
}