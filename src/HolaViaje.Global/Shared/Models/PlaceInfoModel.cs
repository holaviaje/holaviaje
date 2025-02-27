namespace HolaViaje.Global.Shared.Models;

public record PlaceInfoModel
{
    public string? Country { get; set; }
    public string? State { get; set; }
    public string? City { get; set; }
    public LocationInfoModel? Location { get; set; }
}
