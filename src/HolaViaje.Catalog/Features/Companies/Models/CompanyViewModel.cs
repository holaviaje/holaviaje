using HolaViaje.Catalog.Shared.Models;
using HolaViaje.Global.Shared.Models;

namespace HolaViaje.Catalog.Features.Companies.Models;

public class CompanyViewModel
{
    /// <summary>
    /// Name of the company.
    /// </summary>
    public string? Name { get; set; }
    /// <summary>
    /// Legal name of the company.
    /// </summary>
    public string? LegalName { get; set; }
    /// <summary>
    /// Address line 1 of the company.
    /// </summary>
    public string? Address1 { get; set; }
    /// <summary>
    /// Address line 2 of the company.
    /// </summary>
    public string? Address2 { get; set; }
    /// <summary>
    /// City of the company.
    /// </summary>
    public string? City { get; set; }
    /// <summary>
    /// State of the company.
    /// </summary>
    public string? State { get; set; }
    /// <summary>
    /// Zip code of the company.
    /// </summary>
    public string? ZipCode { get; set; }
    /// <summary>
    /// Country of the company.
    /// </summary>
    public string? Country { get; set; }
    /// <summary>
    /// Phone number of the company.
    /// </summary>
    public string? Phone { get; set; }
    /// <summary>
    /// Email address of the company.
    /// </summary>
    public string? Email { get; set; }
    /// <summary>
    /// Website of the company.
    /// </summary>
    public string? Website { get; set; }
    /// <summary>
    /// Registration number of the company.
    /// </summary>
    public string? RegistrationNumber { get; set; }
    /// <summary>
    /// Country where the company is registered.
    /// </summary>
    public string? RegisteredIn { get; set; }
    /// <summary>
    /// VAT identification number of the company.
    /// </summary>
    public string? VatId { get; set; }
    /// <summary>
    /// Booking information of the company.
    /// </summary>
    public BookInfoModel? BookInfo { get; set; }
    /// <summary>
    /// Managers of the company.
    /// </summary>
    public ICollection<ManagerModel> Managers { get; set; } = [];
    /// <summary>
    /// Indicates if the company is active.
    /// </summary>
    public bool IsActive { get; set; }
    /// <summary>
    /// Identifier of the user that created the company.
    /// </summary>
    public long UserId { get; set; }
    /// <summary>
    /// Control information of the company.
    /// </summary>
    public EntityControlModel Control { get; set; } = new();
}
