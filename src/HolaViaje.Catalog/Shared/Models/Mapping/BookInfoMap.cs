using AutoMapper;

namespace HolaViaje.Catalog.Shared.Models.Mapping;

public class BookInfoMap : Profile
{
    public BookInfoMap()
    {
        CreateMap<BookInfo, BookInfoModel>().ConstructUsing((src, ctx) => new BookInfoModel(src.Phone, src.WhatsApp, src.Email));
    }
}
