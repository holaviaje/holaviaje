using AutoMapper;

namespace HolaViaje.Catalog.Shared.Models.Mapping;

public class CancellationPolicyMap : Profile
{
    public CancellationPolicyMap()
    {
        CreateMap<CancellationPolicy, CancellationPolicyModel>().ConstructUsing((src, ctx) => new CancellationPolicyModel(src.Policy, src.DaysToCancel, src.RefundPercentage));
    }
}
