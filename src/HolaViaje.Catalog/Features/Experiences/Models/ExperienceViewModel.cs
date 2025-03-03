using HolaViaje.Catalog.Shared;
using HolaViaje.Catalog.Shared.Models;
using HolaViaje.Global.Shared.Models;

namespace HolaViaje.Catalog.Features.Experiences.Models;

public record ExperienceViewModel
{
    public Guid ExperienceId { get; set; }
    public Guid CompanyId { get; set; }
    public ICollection<ExperienceTranslationViewModel> Translations { get; set; } = [];
    public CancellationPolicy CancellationPolicy { get; set; }
    public DurationModel? Duration { get; set; }
    public bool PickupAvailable { get; set; }
    public bool InstantTicketDelivery { get; set; }
    public bool MobileTicket { get; set; }
    public bool WheelchairAccessible { get; set; }
    public bool PetsFrendly { get; set; }
    public int MaxGuests { get; set; }
    public ICollection<PhotoModel> Photos { get; set; } = [];
    public bool IsAvailable { get; set; } = true;
    public EntityControlModel Control { get; set; }
}
