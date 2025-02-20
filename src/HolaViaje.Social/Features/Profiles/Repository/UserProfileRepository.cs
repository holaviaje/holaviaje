
using HolaViaje.Social.Data;
using Microsoft.EntityFrameworkCore;

namespace HolaViaje.Social.Features.Profiles.Repository;

public class UserProfileRepository(ApplicationDbContext dbContext) : IUserProfileRepository
{
    private readonly DbSet<UserProfile> userProfiles = dbContext.Set<UserProfile>();

    private async Task SaveChangesAsync()
    {
        if (dbContext.ChangeTracker.HasChanges())
        {
            await dbContext.SaveChangesAsync();
        }
    }

    public async Task<UserProfile> CreateAsync(UserProfile userProfile)
    {
        userProfiles.Add(userProfile);
        await SaveChangesAsync();
        return userProfile;
    }

    public async Task<UserProfile?> GetAsync(long id, bool traking = false)
    {
        var query = userProfiles.Where(x => x.Id == id);

        if (!traking)
        {
            query = query.AsNoTracking();
        }

        return await query.FirstOrDefaultAsync();
    }

    public async Task<UserProfile> UpdateAsync(UserProfile userProfile)
    {
        var entry = dbContext.Entry(userProfile);

        if (entry?.State != EntityState.Modified)
        {
            userProfiles.Update(userProfile);
        }

        await SaveChangesAsync();
        return userProfile;
    }
}
