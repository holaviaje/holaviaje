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
    public ICollection<AdditionalInfo> AdditionalInfos { get; set; } = [];
    public string? LiveGuide { get; set; }
    public PlaceInfo Place { get; set; } = new();

    public void SetServices(IEnumerable<Service> services)
    {
        var servicesToAdd = services.Except(Services);
        var servicesToRemove = Services.Except(services);

        foreach (var service in servicesToAdd)
        {
            Services.Add(service);
        }

        foreach (var service in servicesToRemove)
        {
            Services.Remove(service);
        }
    }

    public void SetStops(IEnumerable<Stop> stops)
    {
        var stopsToAdd = stops.Except(Stops);
        var stopsToRemove = Stops.Except(stops);

        foreach (var stop in stopsToAdd)
        {
            Stops.Add(stop);
        }

        foreach (var stop in stopsToRemove)
        {
            Stops.Remove(stop);
        }
    }

    public void SetAdditionalInfos(IEnumerable<AdditionalInfo> additionalInfos)
    {
        var additionalInfosToAdd = additionalInfos.Except(AdditionalInfos);
        var additionalInfosToRemove = AdditionalInfos.Except(additionalInfos);

        foreach (var additionalInfo in additionalInfosToAdd)
        {
            AdditionalInfos.Add(additionalInfo);
        }

        foreach (var additionalInfo in additionalInfosToRemove)
        {
            AdditionalInfos.Remove(additionalInfo);
        }
    }

    public void SetPickup(Pickup pickup)
    {
        if (Pickup == pickup) return;
        Pickup = pickup;
    }

    public void SetPlace(PlaceInfo place)
    {
        if (Place == place) return;
        Place = place;
    }
}
