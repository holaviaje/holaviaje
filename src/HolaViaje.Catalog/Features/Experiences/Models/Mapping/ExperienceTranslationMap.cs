using AutoMapper;

namespace HolaViaje.Catalog.Features.Experiences.Models.Mapping;

public class ExperienceTranslationMap : Profile
{
    public ExperienceTranslationMap()
    {
        CreateMap<ExperienceTranslation, ExperienceTranslationViewModel>();
        CreateMap<ExperienceTranslationModel, ExperienceTranslation>();
    }
}