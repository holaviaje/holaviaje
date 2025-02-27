using HolaViaje.Catalog.Shared.Models;
using HolaViaje.Global.Shared.Models;

namespace HolaViaje.Catalog.Features.Experiences.Models;

public class ExperienceTranslationViewModel
{
    public Guid TranslationId { get; set; }
    public required string LanguageCode { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public ICollection<ServiceModel> Services { get; set; } = [];
    public PickupModel? Pickup { get; set; }
    public string? ImportantInformation { get; set; }
    public string? WhatToExpect { get; set; }
    public ICollection<StopModel> Stops { get; set; } = [];
    public ICollection<AdditionalInfoModel> AdditionalInfos { get; set; } = [];
    public string? LiveGuide { get; set; }
    public PlaceInfoModel? Place { get; set; }
}
