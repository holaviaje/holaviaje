using AutoMapper;

namespace HolaViaje.Global.Shared.Models.Mapping;

internal class PlaceInfoMap : Profile
{
    public PlaceInfoMap()
    {
        CreateMap<PlaceInfo, PlaceInfoModel>()
            .ConstructUsing((src, ctx) => new PlaceInfoModel
            {
                Country = src.Country,
                State = src.State,
                City = src.City,
                Location = src.Location != null ? ctx.Mapper.Map<LocationInfoModel>(src.Location) : null
            });
    }
}
