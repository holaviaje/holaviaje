using HolaViaje.Catalog.Shared.Models;
using HolaViaje.Global.Shared.Models;

namespace HolaViaje.Catalog.Features.Experiences.Models;

public class ExperienceViewModel
{
    public Guid ExperienceId { get; set; }
    public Guid PageId { get; set; }
    public CancellationPolicyModel? CancellationPolicy { get; set; }
    public TimeRangeModel? TimeRange { get; set; }
    public bool PickupAvailable { get; set; }
    public bool InstantTicketDelivery { get; set; }
    public bool MobileTicket { get; set; }
    public bool WheelchairAccessible { get; set; }
    public int MaxGuests { get; set; }
    public ICollection<PhotoModel> Photos { get; set; } = [];
    public BookInfoModel? BookInfirmation { get; set; }
    public bool IsAvailable { get; set; } = true;
    public EntityControlModel? Control { get; set; }
    public ICollection<ExperienceTranslationViewModel> Transalations { get; set; } = [];
}
