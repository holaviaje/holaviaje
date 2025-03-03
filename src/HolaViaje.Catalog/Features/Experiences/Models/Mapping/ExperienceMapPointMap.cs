using AutoMapper;

namespace HolaViaje.Catalog.Features.Experiences.Models.Mapping;

public class ExperienceMapPointMap : Profile
{
    public ExperienceMapPointMap()
    {
        CreateMap<ExperienceMapPoint, ExperienceMapPointModel>();
    }
}
