using AutoMapper;

namespace HolaViaje.Catalog.Features.Experiences.Models.Mapping;

public class ExperienceStopMap : Profile
{
    public ExperienceStopMap()
    {
        CreateMap<ExperienceStop, ExperienceStopModel>();
    }
}
