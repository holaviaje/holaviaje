using HolaViaje.Global.Shared.Models;

namespace HolaViaje.Catalog.Features.Companies;

public static class CompanyErrorModelHelper
{
    public static ErrorModel CompanyNotFoundError()
    {
        return new ErrorModel(400, "Company not found.");
    }

    public static ErrorModel UnauthorizedError()
    {
        return new ErrorModel(403, "Unauthorized operation.");
    }
}
