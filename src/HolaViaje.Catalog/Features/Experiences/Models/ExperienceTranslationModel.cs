using HolaViaje.Catalog.Shared.Models;
using HolaViaje.Global.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace HolaViaje.Catalog.Features.Experiences.Models;

public record ExperienceTranslationModel
{
    [Required]
    public required string LanguageCode { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public ICollection<ExperienceServiceModel> Services { get; set; } = [];
    public ICollection<ExperienceMapPointModel> PickupPoints { get; set; } = [];
    public ICollection<ExperienceMapPointModel> MeetingPoints { get; set; } = [];
    public ExperienceMapPointModel? EndPoint { get; set; }
    public ExperienceMapPointModel? TicketRedemptionPoint { get; set; }
    public string? CancellationPolicyName { get; set; }
    public string? CancellationPolicyDetails { get; set; }
    public string? ImportantInformation { get; set; }
    public string? PetsPolicyDetails { get; set; }
    public string? WhatToExpect { get; set; }
    public ICollection<ExperienceStopModel> Stops { get; set; } = [];
    public ICollection<AdditionalInfoModel> AdditionalInfos { get; set; } = [];
    public string? LiveGuide { get; set; }
    [Required]
    public MapPointModel Place { get; set; } = new();
}
