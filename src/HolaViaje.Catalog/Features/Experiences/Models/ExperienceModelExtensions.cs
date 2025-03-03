using HolaViaje.Catalog.Shared.Models;
using HolaViaje.Global.Helpers;
using HolaViaje.Global.Shared.Models;

namespace HolaViaje.Catalog.Features.Experiences.Models;

public static class ExperienceModelExtensions
{
    public static ExperienceMapPoint FromModel(this ExperienceMapPointModel model)
    {
        return new()
        {
            RecordId = ModelHelper.GetRecordId(model.RecordId),
            Name = model.Name,
            Address = model.Address,
            Country = model.Country,
            State = model.State,
            City = model.City,
            ZipCode = model.ZipCode,
            Latitude = model.Latitude,
            Longitude = model.Longitude,
            Time = model.Time,
            Details = model.Details
        };
    }

    public static IEnumerable<ExperienceMapPoint> FromModel(this IEnumerable<ExperienceMapPointModel> models)
    {
        return models.Select(model => model.FromModel());
    }

    public static IEnumerable<ExperienceService> FromModel(this IEnumerable<ExperienceServiceModel> models)
    {
        return models.Select(model => new ExperienceService
        {
            RecordId = ModelHelper.GetRecordId(model.RecordId),
            Title = model.Title,
            Included = model.Included
        });
    }

    public static IEnumerable<ExperienceStop> FromModel(this IEnumerable<ExperienceStopModel> models)
    {
        return models.Select(model => new ExperienceStop
        {
            RecordId = ModelHelper.GetRecordId(model.RecordId),
            StopOrder = model.StopOrder,
            Title = model.Title,
            Description = model.Description,
            AdmissionTicket = model.AdmissionTicket,
            Place = model.Place.FromModel(),
            Duration = model.Duration.FromModel()
        });
    }
}