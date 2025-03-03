namespace HolaViaje.Global.Shared.Models;

public static class SharedModelExtensions
{
    public static MapPoint FromModel(this MapPointModel model)
    {
        return new MapPoint
        {
            Name = model.Name,
            Address = model.Address,
            Country = model.Country,
            State = model.State,
            City = model.City,
            ZipCode = model.ZipCode,
            Latitude = model.Latitude,
            Longitude = model.Longitude
        };
    }
}
