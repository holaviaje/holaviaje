using HolaViaje.Catalog.Shared;
using HolaViaje.Global.Shared;

namespace HolaViaje.Catalog.Features.Companies;

public class Company
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? LegalName { get; set; }
    public string? Address1 { get; set; }
    public string? Address2 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }
    public string? Country { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Website { get; set; }
    public string? RegistrationNumber { get; set; }
    public string? RegisteredIn { get; set; }
    public string? VatId { get; set; }
    public BookInfo? BookInfo { get; set; }
    public ICollection<Manager> Managers { get; set; } = [];
    public bool IsActive { get; set; }
    public EntityControl Control { get; set; } = new();
}
