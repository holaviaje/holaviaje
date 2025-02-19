using HolaViaje.Social.Shared;

namespace HolaViaje.Social.Features.Profiles;

public class UserProfile : EntityBase
{
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
    public TravelCompanion TravelCompanion { get; set; } = TravelCompanion.None;
    /// <summary>
    /// Gets or sets the user's profile picture.
    /// </summary>
    public ProfilePicture? Picture { get; set; }
    public PlaceInfo Place { get; set; } = new();
    /// <summary>
    /// Gets or sets the user's control information.
    /// </summary>
    public required EntityControl Control { get; init; }
}
