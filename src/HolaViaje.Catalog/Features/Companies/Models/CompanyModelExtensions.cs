namespace HolaViaje.Catalog.Features.Companies.Models;

public static class CompanyModelExtensions
{
    public static IEnumerable<Manager> FromModel(this IEnumerable<ManagerModel> models)
    {
        return models.Select(x => new Manager(x.UserId)
        {
            ManageAll = x.ManageAll,
            ManageExperiences = x.ManageExperiences
        });
    }
}