namespace HolaViaje.Global.Shared.Models;

public static class SharedModelExtensions
{
    public static PlaceInfo FromModel(this PlaceInfoModel model)
    {
        return new PlaceInfo
        {
            Country = model.Country,
            State = model.State,
            City = model.City,
            Location = model.Location != null ? model.Location.FromModel() : null
        };
    }

    public static LocationInfo FromModel(this LocationInfoModel model)
    {
        return new LocationInfo(model.Latitude, model.Longitude);
    }
}
