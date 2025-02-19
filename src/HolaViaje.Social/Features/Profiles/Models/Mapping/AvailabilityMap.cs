using AutoMapper;

namespace HolaViaje.Social.Features.Profiles.Models.Mapping;

internal class AvailabilityMap : Profile
{
    public AvailabilityMap()
    {
        CreateMap<Availability, AvailabilityModel>()
            .ConstructUsing((src, ctx) => new AvailabilityModel(src.IsAvailable, src.AvailableFor));
    }
}
