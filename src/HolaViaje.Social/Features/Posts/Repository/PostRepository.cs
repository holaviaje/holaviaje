using HolaViaje.Social.Data;
using Microsoft.EntityFrameworkCore;

namespace HolaViaje.Social.Features.Posts.Repository;

public class PostRepository(ApplicationDbContext dbContext) : IPostRepository
{
    private readonly DbSet<Post> posts = dbContext.Set<Post>();

    private async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        if (dbContext.ChangeTracker.HasChanges())
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<Post> CreateAsync(Post post, CancellationToken cancellationToken = default)
    {
        posts.Add(post);
        await SaveChangesAsync();
        return post;
    }

    public async Task<Post?> GetAsync(long id, bool traking = false, CancellationToken cancellationToken = default)
    {
        var query = posts.Where(x => x.Id == id);

        if (!traking)
        {
            query = query.AsNoTracking();
        }

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Post> UpdateAsync(Post post, CancellationToken cancellationToken = default)
    {
        var entry = dbContext.Entry(post);

        if (entry?.State != EntityState.Modified)
        {
            posts.Update(post);
        }

        await SaveChangesAsync();
        return post;
    }
}
