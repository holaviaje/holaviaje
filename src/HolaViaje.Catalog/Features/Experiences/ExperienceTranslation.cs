using HolaViaje.Catalog.Shared;
using HolaViaje.Global.Shared;
using Throw;

namespace HolaViaje.Catalog.Features.Experiences;

public class ExperienceTranslation
{
    public ExperienceTranslation()
    {

    }

    public ExperienceTranslation(Experience experience, string languageCode)
    {
        experience.ThrowIfNull(nameof(experience));
        languageCode.ThrowIfNull(nameof(languageCode)).IfEmpty();

        Experience = experience;
        ExperienceId = experience.Id;
        LanguageCode = languageCode;
    }

    public Guid Id { get; set; }
    public Guid ExperienceId { get; set; }
    public required Experience Experience { get; set; }
    public string LanguageCode { get; set; } = "EN";
    public string? Title { get; set; }
    public string? Description { get; set; }
    public ICollection<ExperienceService> Services { get; set; } = [];
    public ICollection<ExperienceMapPoint> PickupPoints { get; set; } = [];
    public ICollection<ExperienceMapPoint> MeetingPoints { get; set; } = [];
    public ExperienceMapPoint? EndPoint { get; set; }
    public ExperienceMapPoint? TicketRedemptionPoint { get; set; }
    public string? CancellationPolicyName { get; set; }
    public string? CancellationPolicyDetails { get; set; }
    public string? ImportantInformation { get; set; }
    public string? PetsPolicyDetails { get; set; }
    public string? WhatToExpect { get; set; }
    public ICollection<ExperienceStop> Stops { get; set; } = [];
    public ICollection<AdditionalInfo> AdditionalInfos { get; set; } = [];
    public string? LiveGuide { get; set; }
    public MapPoint Place { get; set; } = new();

    /// <summary>
    /// Sets the services.
    /// </summary>
    /// <param name="services"></param>
    public void SetServices(IEnumerable<ExperienceService> services)
    {
        var servicesToAdd = services.Except(Services);
        var servicesToRemove = Services.Except(services);

        // Remove services
        foreach (var service in servicesToRemove)
        {
            Services.Remove(service);
        }

        // Add new services
        foreach (var service in servicesToAdd)
        {
            Services.Add(service);
        }

        Experience?.UpdateLastModified();
    }

    /// <summary>
    /// Sets the pickup points.
    /// </summary>
    /// <param name="pickupPoints"></param>
    public void SetPickupPoints(IEnumerable<ExperienceMapPoint> pickupPoints)
    {
        var pickupPointsToAdd = pickupPoints.ExceptBy(PickupPoints.Select(x => x.RecordId), x => x.RecordId);
        var pickupPointsToRemove = PickupPoints.ExceptBy(pickupPoints.Select(x => x.RecordId), x => x.RecordId);

        // Remove pickup points
        foreach (var pickupPoint in pickupPointsToRemove)
        {
            PickupPoints.Remove(pickupPoint);
        }

        // Update existing pickup points
        foreach (var pickupPoint in pickupPoints)
        {
            var existingPoint = PickupPoints.FirstOrDefault(x => x.RecordId == pickupPoint.RecordId);
            if (existingPoint != null)
            {
                existingPoint.Name = pickupPoint.Name;
                existingPoint.Address = pickupPoint.Address;
                existingPoint.Country = pickupPoint.Country;
                existingPoint.State = pickupPoint.State;
                existingPoint.City = pickupPoint.City;
                existingPoint.ZipCode = pickupPoint.ZipCode;
                existingPoint.Latitude = pickupPoint.Latitude;
                existingPoint.Longitude = pickupPoint.Longitude;
                existingPoint.Time = pickupPoint.Time;
                existingPoint.Details = pickupPoint.Details;
            }
        }

        // Add new pickup points
        foreach (var pickupPoint in pickupPointsToAdd)
        {
            PickupPoints.Add(pickupPoint);
        }

        Experience?.UpdateLastModified();
    }

    /// <summary>
    /// Sets the meeting points.
    /// </summary>
    /// <param name="meetingPoints"></param>
    public void SetMeetingPoints(IEnumerable<ExperienceMapPoint> meetingPoints)
    {
        var meetingPointsToAdd = meetingPoints.ExceptBy(MeetingPoints.Select(x => x.RecordId), x => x.RecordId);
        var meetingPointsToRemove = MeetingPoints.ExceptBy(meetingPoints.Select(x => x.RecordId), x => x.RecordId);

        // Remove meeting points
        foreach (var meetingPoint in meetingPointsToRemove)
        {
            MeetingPoints.Remove(meetingPoint);
        }

        // Update existing meeting points
        foreach (var meetingPoint in meetingPoints)
        {
            var existingPoint = MeetingPoints.FirstOrDefault(x => x.RecordId == meetingPoint.RecordId);
            if (existingPoint != null)
            {
                existingPoint.Name = meetingPoint.Name;
                existingPoint.Address = meetingPoint.Address;
                existingPoint.Country = meetingPoint.Country;
                existingPoint.State = meetingPoint.State;
                existingPoint.City = meetingPoint.City;
                existingPoint.ZipCode = meetingPoint.ZipCode;
                existingPoint.Latitude = meetingPoint.Latitude;
                existingPoint.Longitude = meetingPoint.Longitude;
                existingPoint.Time = meetingPoint.Time;
                existingPoint.Details = meetingPoint.Details;
            }
        }

        // Add new meeting points
        foreach (var meetingPoint in meetingPointsToAdd)
        {
            MeetingPoints.Add(meetingPoint);
        }

        Experience?.UpdateLastModified();
    }

    /// <summary>
    /// Sets the ticket redemption point.
    /// </summary>
    /// <param name="ticketRedemptionPoint"></param>
    public void SetTicketRedemptionPoint(ExperienceMapPoint ticketRedemptionPoint)
    {
        if (TicketRedemptionPoint == ticketRedemptionPoint) return;
        TicketRedemptionPoint = ticketRedemptionPoint;
        Experience?.UpdateLastModified();
    }

    /// <summary>
    /// Sets the end point.
    /// </summary>
    /// <param name="endPoint"></param>
    public void SetEndPoint(ExperienceMapPoint endPoint)
    {
        if (EndPoint == endPoint) return;
        EndPoint = endPoint;
        Experience?.UpdateLastModified();
    }

    /// <summary>
    /// Sets the stops.
    /// </summary>
    /// <param name="stops"></param>
    public void SetStops(IEnumerable<ExperienceStop> stops)
    {
        var stopsToAdd = stops.ExceptBy(Stops.Select(x => x.RecordId), x => x.RecordId);
        var stopsToRemove = Stops.ExceptBy(stops.Select(x => x.RecordId), x => x.RecordId);

        // Remove stops
        foreach (var stop in stopsToRemove)
        {
            Stops.Remove(stop);
        }

        // Update existing stops
        foreach (var stop in stops)
        {
            var existingStop = Stops.FirstOrDefault(x => x.RecordId == stop.RecordId);
            if (existingStop != null)
            {
                existingStop.Title = stop.Title;
                existingStop.Description = stop.Description;
                existingStop.AdmissionTicket = stop.AdmissionTicket;

                if (existingStop.Place != stop.Place)
                    existingStop.Place = stop.Place;

                if (existingStop.Duration != stop.Duration)
                    existingStop.Duration = stop.Duration;
            }
        }

        // Add new stops
        foreach (var stop in stopsToAdd)
        {
            Stops.Add(stop);
        }

        Experience?.UpdateLastModified();
    }

    /// <summary>
    /// Sets the additional infos.
    /// </summary>
    /// <param name="additionalInfos"></param>
    public void SetAdditionalInfos(IEnumerable<AdditionalInfo> additionalInfos)
    {
        var additionalInfosToAdd = additionalInfos.ExceptBy(AdditionalInfos.Select(x => x.RecordId), x => x.RecordId);
        var additionalInfosToRemove = AdditionalInfos.ExceptBy(additionalInfos.Select(x => x.RecordId), x => x.RecordId);

        // Remove additional infos
        foreach (var additionalInfo in additionalInfosToRemove)
        {
            AdditionalInfos.Remove(additionalInfo);
        }

        // Update existing additional infos
        foreach (var additionalInfo in additionalInfos)
        {
            var existingInfo = AdditionalInfos.FirstOrDefault(x => x.RecordId == additionalInfo.RecordId);
            if (existingInfo != null)
            {
                existingInfo.Description = additionalInfo.Description;
            }
        }

        // Add new additional infos
        foreach (var additionalInfo in additionalInfosToAdd)
        {
            AdditionalInfos.Add(additionalInfo);
        }

        Experience?.UpdateLastModified();
    }

    /// <summary>
    /// Sets the place.
    /// </summary>
    /// <param name="place"></param>
    public void SetPlace(MapPoint place)
    {
        if (Place == place) return;
        Place = place;
        Experience?.UpdateLastModified();
    }
}
