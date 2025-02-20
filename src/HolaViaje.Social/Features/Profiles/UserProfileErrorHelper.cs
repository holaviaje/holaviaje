using HolaViaje.Social.Shared.Models;

namespace HolaViaje.Social.Features.Profiles;

public static class UserProfileErrorHelper
{
    public static ErrorModel ProfileAlreadyExists() => new ErrorModel(400, "Profile already exists.");
    public static ErrorModel ProfileNotFound() => new ErrorModel(404, "Profile not found.");
    public static ErrorModel ProfileAccessDenied() => new ErrorModel(403, "You can't access to the profile.");
}