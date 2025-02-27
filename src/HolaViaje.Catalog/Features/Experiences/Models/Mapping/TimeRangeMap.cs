using AutoMapper;

namespace HolaViaje.Catalog.Features.Experiences.Models.Mapping;

public class TimeRangeMap : Profile
{
    public TimeRangeMap()
    {
        CreateMap<TimeRange, TimeRangeModel>().ConstructUsing((src, ctx) => new TimeRangeModel(src.StartTime, src.EndTime, src.Duration));
    }
}
