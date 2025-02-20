using AutoMapper;

namespace HolaViaje.Social.Shared.Models.Mapping;

internal class LocationInfoMap : Profile
{
    public LocationInfoMap()
    {
        CreateMap<LocationInfo, LocationInfoModel>()
            .ConstructUsing((src, ctx) => new LocationInfoModel(src.Latitude, src.Longitude));
    }
}
