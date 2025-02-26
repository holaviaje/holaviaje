using HolaViaje.Global.Shared;

namespace HolaViaje.Catalog.Features.Experiences;

public record Pickup
{
    public string? Address1 { get; set; }
    public string? Address2 { get; set; }
    public PlaceInfo? Place { get; set; }
    public string? Details { get; set; }
}