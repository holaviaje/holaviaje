namespace HolaViaje.Catalog.Features.Experiences.Models;

public record ExperienceServiceModel
{
    public Guid RecordId { get; set; }
    public string? Title { get; set; }
    public bool Included { get; set; }
}