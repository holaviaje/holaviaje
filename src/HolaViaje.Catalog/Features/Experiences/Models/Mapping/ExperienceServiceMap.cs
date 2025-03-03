using AutoMapper;

namespace HolaViaje.Catalog.Features.Experiences.Models.Mapping;

public class ExperienceServiceMap : Profile
{
    public ExperienceServiceMap()
    {
        CreateMap<ExperienceService, ExperienceServiceModel>();
    }
}