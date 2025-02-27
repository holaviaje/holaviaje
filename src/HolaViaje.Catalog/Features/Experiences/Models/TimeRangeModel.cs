namespace HolaViaje.Catalog.Features.Experiences.Models;

public record TimeRangeModel(TimeOnly? StartTime, TimeOnly? EndTime, int? Duration) { }
