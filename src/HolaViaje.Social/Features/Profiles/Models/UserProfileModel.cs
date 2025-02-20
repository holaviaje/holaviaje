using System.Text.Json.Serialization;

namespace HolaViaje.Social.Features.Profiles.Models;

public class UserProfileModel
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
    [JsonConverter(typeof(JsonStringEnumConverter<Gender>))]
    public Gender Gender { get; set; } = Gender.Unknown;

    /// <summary>
    /// Gets the visibility settings of the user's profile.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<Visibility>))]
    public Visibility Visibility { get; set; }

    /// <summary>
    /// Gets or sets the "About Me" section of the user's profile.
    /// </summary>
    public string? AboutMe { get; set; }

    /// <summary>
    /// Gets or sets the user's travel companion preference.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<TravelCompanion>))]
    public TravelCompanion TravelCompanion { get; set; } = TravelCompanion.None;
}
