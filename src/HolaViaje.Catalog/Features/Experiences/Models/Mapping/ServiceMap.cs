using AutoMapper;

namespace HolaViaje.Catalog.Features.Experiences.Models.Mapping;

public class ServiceMap : Profile
{
    public ServiceMap()
    {
        CreateMap<Service, ServiceModel>().ConstructUsing((src, ctx) => new ServiceModel(src.Title, src.Included));
    }
}