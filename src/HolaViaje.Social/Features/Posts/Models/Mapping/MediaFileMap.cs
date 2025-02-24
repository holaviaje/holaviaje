using AutoMapper;

namespace HolaViaje.Social.Features.Posts.Models.Mapping;

public class MediaFileMap : Profile
{
    public MediaFileMap()
    {
        CreateMap<MediaFile, MediaFileModel>().ConstructUsing((src, ctx) => new MediaFileModel(src.FileId, src.FileName, src.ContentType, src.Url, src.Uploaded));
    }
}
