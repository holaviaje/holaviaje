using AutoMapper;

namespace HolaViaje.Catalog.Features.Experiences.Models.Mapping;

public class PickupMap : Profile
{
    public PickupMap()
    {
        CreateMap<Pickup, PickupModel>().ConstructUsing((src, ctx) => new PickupModel(src.Address1, src.Address2, src.Details));
    }
}
