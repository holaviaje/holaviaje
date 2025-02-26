using HolaViaje.Catalog.Shared;
using HolaViaje.Global.Shared;

namespace HolaViaje.Catalog.Features.Experiences;

public class ExperienceTranslation
{
    public Guid Id { get; set; }
    public Guid ExperienceId { get; set; }
    public required Experience Experience { get; set; }
    public string LanguageCode { get; set; } = "EN";
    public string? Title { get; set; }
    public string? Description { get; set; }
    public ICollection<Service> Services { get; set; } = [];
    public Pickup Pickup { get; set; } = new();
    public string? CancellationPolicyName { get; set; }
    public string? CancellationPolicyDetails { get; set; }
    public string? ImportantInformation { get; set; }
    public string? WhatToExpect { get; set; }
    public ICollection<Stop> Stops { get; set; } = [];
    public ICollection<AddionalInfo> AddionalInfos { get; set; } = [];
    public string? LiveGuide { get; set; }
    public PlaceInfo Place { get; set; } = new();
}
