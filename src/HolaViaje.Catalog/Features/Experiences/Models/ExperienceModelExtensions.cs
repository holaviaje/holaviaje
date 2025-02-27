using HolaViaje.Global.Shared;
using HolaViaje.Global.Shared.Models;

namespace HolaViaje.Catalog.Features.Experiences.Models;

public static class ExperienceModelExtensions
{

    public static TimeRange FromModel(this TimeRangeModel model)
    {
        return new TimeRange(model.StartTime, model.EndTime, model.Duration);
    }

    public static Pickup FromModel(this PickupModel model)
    {
        return new()
        {
            Address1 = model.Address1,
            Address2 = model.Address2,
            Place = model.Place?.FromModel() ?? new PlaceInfo(),
            Details = model.Details
        };
    }

    public static IEnumerable<Service> FromModel(this IEnumerable<ServiceModel> models)
    {
        return models.Select(model => new Service(model.Title, model.Included));
    }

    public static IEnumerable<Stop> FromModel(this IEnumerable<StopModel> models)
    {
        return models.Select(model => new Stop(model.Title, model.Description, model.Duration, model.AdditionalInfo) { Place = model.Place?.FromModel() ?? new PlaceInfo() });
    }
}