namespace HolaViaje.Catalog.Features.Experiences;

public sealed class ExperienceService
{
    public Guid RecordId { get; set; }
    public string? Title { get; set; }
    public bool Included { get; set; }
}