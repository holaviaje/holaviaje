using AutoMapper;

namespace HolaViaje.Catalog.Shared.Models.Mapping;

public class PhotoMap : Profile
{
    public PhotoMap()
    {
        CreateMap<Photo, PhotoModel>().ConstructUsing((src, ctx) => new PhotoModel(src.FileId, src.ImageUrl));
    }
}
