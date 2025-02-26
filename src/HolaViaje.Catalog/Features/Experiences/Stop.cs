using HolaViaje.Global.Shared;

namespace HolaViaje.Catalog.Features.Experiences;

public record Stop(string Title, string? Description, int Duration, string? AdditionalInfo)
{
    public PlaceInfo Place { get; set; } = new();
}