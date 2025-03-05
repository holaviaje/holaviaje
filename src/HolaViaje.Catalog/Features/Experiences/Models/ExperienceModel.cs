using HolaViaje.Catalog.Shared.Models;

namespace HolaViaje.Catalog.Features.Experiences.Models;

public record ExperienceModel
{
    public Guid PageId { get; set; }
    public CancellationPolicyModel CancellationPolicy { get; set; }
    public DurationModel? Duration { get; set; }
    public bool InstantTicketDelivery { get; set; }
    public bool MobileTicket { get; set; }
    public bool WheelchairAccessible { get; set; }
    public bool PetsFrendly { get; set; }
    public int MaxGuests { get; set; }
    public bool IsAvailable { get; set; }
    public ICollection<ExperienceTranslationModel> Translations { get; set; } = [];
}
