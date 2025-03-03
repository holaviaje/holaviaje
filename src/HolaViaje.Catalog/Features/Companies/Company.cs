using HolaViaje.Catalog.Shared;
using HolaViaje.Global.Shared;

namespace HolaViaje.Catalog.Features.Companies;

public class Company
{
    public Company()
    {

    }

    public Company(long userId)
    {
        UserId = userId;
        Control = new() { CreatedAt = DateTime.UtcNow, LastModifiedAt = DateTime.UtcNow };
    }

    /// <summary>
    /// Unique identifier of the company.
    /// </summary>
    public Guid Id { get; set; }
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
    public BookInfo BookInfo { get; set; }
    /// <summary>
    /// Managers of the company.
    /// </summary>
    public ICollection<Manager> Managers { get; set; } = [];
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
    public EntityControl Control { get; set; }

    /// <summary>
    /// Determines whether the user is the owner of the company.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public bool IsOwner(long userId)
    {
        return UserId == userId;
    }

    /// <summary>
    /// Determines whether the user is an admin of the company.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public bool IsAdmin(long userId)
    {
        if (IsOwner(userId))
        {
            return true;
        }

        return Managers.Any(m => m.UserId == userId && m.ManageAll);
    }

    /// <summary>
    /// Updates the last modified date.
    /// </summary>
    public void UpdateLastModified()
    {
        EnsureNotDeleted();
        Control = Control with { LastModifiedAt = DateTime.UtcNow };
    }

    /// <summary>
    /// Deletes the company.
    /// </summary>
    public void Delete()
    {
        EnsureNotDeleted();
        Control = Control with { DeletedAt = DateTime.UtcNow, IsDeleted = true };
    }

    /// <summary>
    /// Ensures that the company is not deleted.
    /// </summary>
    /// <exception cref="InvalidOperationException">If the company is deleted.</exception>
    private void EnsureNotDeleted()
    {
        if (IsDeleted())
        {
            throw new InvalidOperationException("The experience is deleted.");
        }
    }

    /// <summary>
    /// Determines whether the post is deleted.
    /// </summary>
    /// <returns></returns>
    public bool IsDeleted() => Control.IsDeleted;

    /// <summary>
    /// Sets the booking information of the company.
    /// </summary>
    /// <param name="bookInfo"></param>
    public void SetBookInfo(BookInfo bookInfo)
    {
        if (BookInfo == bookInfo) return;
        BookInfo = bookInfo;
        UpdateLastModified();
    }

    /// <summary>
    /// Sets the managers of the company.
    /// </summary>
    /// <param name="managers"></param>
    public void SetManagers(IEnumerable<Manager> managers)
    {
        var managersToRemove = Managers.Where(m => !managers.Any(x => x.UserId == m.UserId)).ToList();
        var managersToAdd = managers.Where(m => !Managers.Any(x => x.UserId == m.UserId)).ToList();

        foreach (var manager in managersToRemove)
        {
            Managers.Remove(manager);
        }

        //Update existing managers
        foreach (var manager in managers)
        {
            var existingManager = Managers.FirstOrDefault(x => x.UserId == manager.UserId);
            if (existingManager != null && existingManager != manager)
            {
                existingManager.ManageAll = manager.ManageAll;
                existingManager.ManageExperiences = manager.ManageExperiences;
            }
        }

        foreach (var manager in managersToAdd)
        {
            Managers.Add(manager);
        }

        UpdateLastModified();
    }
}
