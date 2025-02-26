namespace HolaViaje.Global.Shared;

public record PlaceInfo
{
    public string? Country { get; set; }
    public string? State { get; set; }
    public string? City { get; set; }
    public LocationInfo? Location { get; set; }
}
