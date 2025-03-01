using AutoMapper;

namespace HolaViaje.Catalog.Features.Companies.Models.Mapping;

public class ManagerMap : Profile
{
    public ManagerMap()
    {
        CreateMap<Manager, ManagerModel>().ConstructUsing((src, ctx) => new ManagerModel(src.UserId));
    }
}
