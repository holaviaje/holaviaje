using HolaViaje.Global.Shared.Models;

namespace HolaViaje.Catalog.Features.Experiences.Models;

public record PickupModel(string? Address1, string? Address2, string? Details)
{
    public PlaceInfoModel? Place { get; set; }
}