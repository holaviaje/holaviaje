namespace HolaViaje.Social.Features.Profiles.Repository;

public interface IUserProfileRepository
{
    Task<UserProfile?> GetAsync(int id, bool traking = false);
    Task<UserProfile> CreateAsync(UserProfile userProfile);
    Task<UserProfile> UpdateAsync(UserProfile userProfile);
}
