namespace HolaViaje.Catalog.Features.Companies;

public record Manager(long UserId)
{
    public bool ManageAll { get; set; }
    public bool ManageExperiences { get; set; }
}
