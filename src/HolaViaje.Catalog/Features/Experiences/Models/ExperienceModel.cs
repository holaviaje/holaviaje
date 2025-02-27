using HolaViaje.Catalog.Shared.Models;

namespace HolaViaje.Catalog.Features.Experiences.Models;

public class ExperienceModel
{
    public Guid PageId { get; set; }
    public CancellationPolicyModel? CancellationPolicy { get; set; }
    public TimeRangeModel? TimeRange { get; set; }
    public bool PickupAvailable { get; set; }
    public bool InstantTicketDelivery { get; set; }
    public bool MobileTicket { get; set; }
    public bool WheelchairAccessible { get; set; }
    public int MaxGuests { get; set; }
    public BookInfoModel? BookInfirmation { get; set; }
    public bool IsAvailable { get; set; }
    public ICollection<ExperienceTransalationModel> Translations { get; set; } = [];
}
