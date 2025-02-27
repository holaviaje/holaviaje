namespace HolaViaje.Catalog.Features.Experiences;

public record TimeRange(TimeOnly? StartTime, TimeOnly? EndTime, int? Duration) { }
