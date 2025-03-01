namespace HolaViaje.Catalog.Features.Companies;

public record Manager(long UserId)
{
    public bool ManageExperiences { get; set; }
}
