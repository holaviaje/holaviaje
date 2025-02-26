using HolaViaje.Catalog.Shared;
using HolaViaje.Global.Shared;

namespace HolaViaje.Catalog.Features.Experiences;

public class Experience
{
    public Guid Id { get; set; }
    public Guid PageId { get; set; }
    public ICollection<ExperienceTranslation> Translations { get; set; } = [];
    public CancellationPolicy? CancellationPolicy { get; set; }
    public TimeRange? TimeRange { get; set; }
    public bool PickupAvailable { get; set; }
    public bool InstantTicketDelivery { get; set; }
    public bool MobileTicket { get; set; }
    public bool WheelchairAccessible { get; set; }
    public int MaxGuests { get; set; }
    public ICollection<Photo> Photos { get; set; } = [];
    public BookInfo? BookInfirmation { get; set; }
    public bool IsAvailable { get; set; } = true;
    public EntityControl Control { get; set; } = new();
}