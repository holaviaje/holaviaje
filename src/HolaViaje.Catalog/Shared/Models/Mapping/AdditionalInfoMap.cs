using AutoMapper;

namespace HolaViaje.Catalog.Shared.Models.Mapping;

public class AdditionalInfoMap : Profile
{
    public AdditionalInfoMap()
    {
        CreateMap<AdditionalInfo, AdditionalInfoModel>().ConstructUsing((src, ctx) => new AdditionalInfoModel(src.Description));
    }
}