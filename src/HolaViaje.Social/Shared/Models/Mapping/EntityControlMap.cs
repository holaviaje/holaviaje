using AutoMapper;

namespace HolaViaje.Social.Shared.Models.Mapping;

public class EntityControlMap : Profile
{
    public EntityControlMap()
    {
        CreateMap<EntityControl, EntityControlModel>()
            .ConstructUsing((src, ctx) => new EntityControlModel
            {
                CreatedAt = src.CreatedAt,
                LastModifiedAt = src.LastModifiedAt,
                DeletedAt = src.DeletedAt,
                IsDeleted = src.IsDeleted
            });
    }
}
