using HolaViaje.Catalog.Shared;
using HolaViaje.Global.Shared;

namespace HolaViaje.Catalog.Features.Experiences;

public sealed class ExperienceStop
{
    public Guid RecordId { get; set; }
    public int StopOrder { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public AdmissionTicket AdmissionTicket { get; set; } = AdmissionTicket.Free;
    public MapPoint Place { get; set; } = new();
    public Duration Duration { get; set; } = new();
}