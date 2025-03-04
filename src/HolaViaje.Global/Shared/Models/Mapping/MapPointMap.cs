using AutoMapper;

namespace HolaViaje.Global.Shared.Models.Mapping;

internal class MapPointMap : Profile
{
    public MapPointMap()
    {
        CreateMap<MapPoint, MapPointModel>().ReverseMap();
    }
}
