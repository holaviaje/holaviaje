namespace HolaViaje.Catalog.Features.Experiences.Models;

public record ExperienceMapPointModel
{
    public Guid? RecordId { get; set; }
    public string? Name { get; set; }
    public string? Address { get; set; }
    public string? Country { get; set; }
    public string? State { get; set; }
    public string? City { get; set; }
    public string? ZipCode { get; set; }
    public string? Latitude { get; set; }
    public string? Longitude { get; set; }
    public string? Time { get; set; }
    public string? Details { get; set; }
}