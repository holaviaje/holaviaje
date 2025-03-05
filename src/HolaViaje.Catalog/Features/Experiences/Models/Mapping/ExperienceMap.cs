using AutoMapper;

namespace HolaViaje.Catalog.Features.Experiences.Models.Mapping;

public class ExperienceMap : Profile
{
    public ExperienceMap()
    {
        CreateMap<Experience, ExperienceViewModel>();
        CreateMap<ExperienceModel, Experience>();
    }
}
