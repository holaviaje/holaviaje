using HolaViaje.Catalog.Shared;
using HolaViaje.Global.Shared;
using Throw;

namespace HolaViaje.Catalog.Features.Experiences;

public class Experience
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public ICollection<ExperienceTranslation> Translations { get; set; } = [];
    public CancellationPolicy CancellationPolicy { get; set; }
    public Duration Duration { get; set; } = new();
    public bool PickupAvailable { get; set; }
    public bool InstantTicketDelivery { get; set; }
    public bool MobileTicket { get; set; }
    public bool WheelchairAccessible { get; set; }
    public bool PetsFrendly { get; set; }
    public int MaxGuests { get; set; }
    public ICollection<Photo> Photos { get; set; } = [];
    public bool IsAvailable { get; set; } = true;
    public EntityControl Control { get; set; }

    public void SetCancellationPolicy(CancellationPolicy cancellationPolicy)
    {
        if (CancellationPolicy == cancellationPolicy) return;
        CancellationPolicy = cancellationPolicy;
        UpdateLastModified();
    }

    public void SetDuration(Duration duration)
    {
        if (Duration == duration) return;
        Duration = duration;
        UpdateLastModified();
    }

    public void AddTranslation(ExperienceTranslation translation)
    {
        translation.ThrowIfNull(nameof(translation));
        Translations.Add(translation);
        UpdateLastModified();
    }

    /// <summary>
    /// Updates the last modified date.
    /// </summary>
    public void UpdateLastModified()
    {
        EnsureNotDeleted();
        Control = Control with { LastModifiedAt = DateTime.UtcNow };
    }

    /// <summary>
    /// Deletes the user profile.
    /// </summary>
    public void Delete()
    {
        EnsureNotDeleted();
        Control = Control with { DeletedAt = DateTime.UtcNow, IsDeleted = true };
    }

    /// <summary>
    /// Ensures that the user profile is not deleted.
    /// </summary>
    /// <exception cref="InvalidOperationException">If the user profile is deleted.</exception>
    private void EnsureNotDeleted()
    {
        if (IsDeleted())
        {
            throw new InvalidOperationException("The experience is deleted.");
        }
    }

    /// <summary>
    /// Determines whether the post is deleted.
    /// </summary>
    /// <returns></returns>
    public bool IsDeleted() => Control.IsDeleted;

    public void RemoveTranslation(ExperienceTranslation translation)
    {
        Translations.Remove(translation);
        UpdateLastModified();
    }

    public void AddPhoto(Photo photo)
    {
        photo.ThrowIfNull(nameof(photo));
        Photos.Add(photo);
        UpdateLastModified();
    }

    public void RemovePhoto(string fileId)
    {
        fileId.ThrowIfNull(nameof(fileId)).IfEmpty();
        var photo = Photos.FirstOrDefault(x => x.FileId.Equals(fileId));
        if (photo != null)
        {
            Photos.Remove(photo);
            UpdateLastModified();
        }
    }
}