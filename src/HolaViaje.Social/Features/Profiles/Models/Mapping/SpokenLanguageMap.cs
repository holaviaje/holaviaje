using AutoMapper;

namespace HolaViaje.Social.Features.Profiles.Models.Mapping;

internal class SpokenLanguageMap : Profile
{
    public SpokenLanguageMap()
    {
        CreateMap<SpokenLanguage, SpokenLanguageModel>()
            .ConstructUsing((src, ctx) => new SpokenLanguageModel(src.Code, src.Language));
    }
}
