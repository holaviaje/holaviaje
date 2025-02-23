namespace HolaViaje.Social.Features.Posts.Repository;

public interface IPostRepository
{
    Task<Post?> GetAsync(int id, bool traking = false, CancellationToken cancellationToken = default);
    Task<Post> CreateAsync(Post post, CancellationToken cancellationToken = default);
    Task<Post> UpdateAsync(Post post, CancellationToken cancellationToken = default);
}