using HolaViaje.Social.Features.Posts.Models;
using HolaViaje.Social.Shared.Models;
using OneOf;

namespace HolaViaje.Social.Features.Posts;

public interface IPostApplication
{
    Task<OneOf<PostViewModel, ErrorModel>> CreateAsync(PostModel model, long userId, CancellationToken cancellationToken = default);
    Task<OneOf<PostViewModel, ErrorModel>> UpdateAsync(long postId, PostBasicModel model, long userId, CancellationToken cancellationToken = default);
    Task<OneOf<PostViewModel, ErrorModel>> PublishAsync(long postId, long userId, CancellationToken cancellationToken = default);
    Task<OneOf<MediaFileModel, ErrorModel>> CreateUploadLinkAsync(long postId, UploadLinkModel model, long userId, CancellationToken cancellationToken = default);
    Task<OneOf<MediaFileModel, ErrorModel>> DeleteMediaFileAsync(long postId, string fileId, long userId, CancellationToken cancellationToken = default);
    Task<OneOf<PostViewModel, ErrorModel>> DeleteAsync(long postId, long userId, CancellationToken cancellationToken = default);
    Task<OneOf<PostViewModel, ErrorModel>> GetAsync(long postId, long userId, CancellationToken cancellationToken = default);
}
