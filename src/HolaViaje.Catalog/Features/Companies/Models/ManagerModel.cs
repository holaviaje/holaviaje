namespace HolaViaje.Catalog.Features.Companies.Models;

public record ManagerModel(long UserId)
{
    public bool ManageAll { get; set; }
    public bool ManageExperiences { get; set; }
}
