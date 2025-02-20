using HolaViaje.Social.Shared;

namespace HolaViaje.Social.Features.Profiles;

public class UserProfile : EntityBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserProfile"/> class.
    /// </summary>
    public UserProfile()
    {
        Control = new()
        {
            CreatedAt = DateTime.UtcNow,
            LastModifiedAt = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserProfile"/> class.
    /// </summary>
    /// <param name="userId">User identifier owner of this profile.</param>
    public UserProfile(long userId)
        : this()
    {
        Id = userId;
    }

    /// <summary>
    /// Gets or sets the first name of the user.
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// Gets or sets the last name of the user.
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// Gets or sets the birthday of the user.
    /// </summary>
    public DateTime? Birthday { get; set; }

    /// <summary>
    /// Gets or sets the gender of the user.
    /// </summary>
    public Gender Gender { get; set; } = Gender.Unknown;

    /// <summary>
    /// Gets or sets the languages spoken by the user.
    /// </summary>
    public ICollection<SpokenLanguage> SpokenLanguages { get; set; } = [];

    /// <summary>
    /// Gets or sets the availability of the user.
    /// </summary>
    public Availability Availability { get; set; } = new(true, null);

    /// <summary>
    /// Gets or sets the visibility settings of the user's profile.
    /// </summary>
    public Visibility Visibility { get; set; } = Visibility.Public;

    /// <summary>
    /// Gets or sets the "About Me" section of the user's profile.
    /// </summary>
    public string? AboutMe { get; set; }

    /// <summary>
    /// Gets or sets the user's travel companion preference.
    /// </summary>
    public TravelCompanion TravelCompanion { get; set; } = TravelCompanion.None;

    /// <summary>
    /// Gets or sets the user's profile picture.
    /// </summary>
    public ProfilePicture? Picture { get; set; }

    /// <summary>
    /// Gets or sets the user's place information.
    /// </summary>
    public PlaceInfo Place { get; set; } = new();

    /// <summary>
    /// Gets or sets the user's control information.
    /// </summary>
    public EntityControl Control { get; set; }

    /// <summary>
    /// Determines whether the profile is visible for the specified user.
    /// </summary>
    /// <param name="userId">User that wants to view the profile.</param>
    /// <returns>True if have access, else false.</returns>
    public bool IsVisibleFor(long userId)
    {
        if (IsOwner(userId)) return true; // is the owner.
        if (Visibility == Visibility.Public) return true; // is public.
        if (Visibility == Visibility.Friends)
        {
            // TODO: Check if the user is a friend.
        }

        if (Visibility == Visibility.Custom)
        {
            // TODO: Check if the user is in the custom list.
        }

        return false;
    }

    /// <summary>
    /// Determines whether the specified user is the owner of the profile.
    /// </summary>
    /// <param name="userId">User Identifier.</param>
    /// <returns>True if is the owner, else false.</returns>
    public bool IsOwner(long userId) => Id == userId;

    /// <summary>
    /// Sets the availability of the user.
    /// </summary>
    /// <param name="availability"></param>
    public void SetAvailability(Availability availability)
    {
        if (Availability == availability) return;

        Availability = availability;
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
        if (Control.IsDeleted)
        {
            throw new InvalidOperationException("The user profile is deleted.");
        }
    }

    /// <summary>
    /// Sets the place information of the user.
    /// </summary>
    /// <param name="place"></param>
    public void SetPlace(PlaceInfo place)
    {
        if (Place == place) return;

        Place = place;
        UpdateLastModified();
    }

    public void SetSpokenLanguages(IEnumerable<SpokenLanguage> spokenLanguages)
    {
        // Remove languages that are not in the new list.
        var removedLanguages = SpokenLanguages.Where(x => !spokenLanguages.Any(y => y.Code == x.Code)).ToList();

        foreach (var language in removedLanguages)
        {
            SpokenLanguages.Remove(language);
        }

        // Add new languages.
        var addedLanguages = spokenLanguages.Where(x => !SpokenLanguages.Any(y => y.Code == x.Code)).ToList();

        foreach (var language in addedLanguages)
        {
            SpokenLanguages.Add(language);
        }

        UpdateLastModified();
    }

    /// <summary>
    /// Sets the profile picture of the user.
    /// </summary>
    /// <param name="picture"></param>
    public void SetPicture(ProfilePicture picture)
    {
        if (Picture == picture) return;
        Picture = picture;
        UpdateLastModified();
    }
}
