using AutoMapper;

namespace HolaViaje.Catalog.Features.Experiences.Models.Mapping;

public class StopMap : Profile
{
    public StopMap()
    {
        CreateMap<Stop, StopModel>().ConstructUsing((src, ctx) => new StopModel(src.Title, src.Description, src.Duration, src.AdditionalInfo));
    }
}
