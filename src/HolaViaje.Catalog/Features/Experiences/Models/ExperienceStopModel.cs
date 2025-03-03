using HolaViaje.Catalog.Shared.Models;
using HolaViaje.Global.Shared.Models;

namespace HolaViaje.Catalog.Features.Experiences.Models;

public record ExperienceStopModel
{
    public Guid RecordId { get; set; }
    public int StopOrder { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public AdmissionTicket AdmissionTicket { get; set; }
    public MapPointModel Place { get; set; } = new();
    public DurationModel Duration { get; set; } = new();
}