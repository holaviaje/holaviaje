using HolaViaje.Global.Shared.Models;

namespace HolaViaje.Catalog.Features.Experiences.Models;

public record StopModel(string Title, string? Description, int? Duration, string? AdditionalInfo, PlaceInfoModel? Place) { }