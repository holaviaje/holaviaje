using AutoMapper;

namespace HolaViaje.Catalog.Features.Companies.Models.Mapping;

public class CompanyMap : Profile
{
    public CompanyMap()
    {
        CreateMap<Company, CompanyViewModel>();
    }
}
