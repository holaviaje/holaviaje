using HolaViaje.Social.Shared.Models;

namespace HolaViaje.Social.Features.Posts;

public static class PostErrorModelHelper
{
    public static ErrorModel ErrorPublishOnPage() => new(400, "You don't have permission to post on this page.");
    public static ErrorModel ErrorUpdatePost() => new(400, "You don't have permission to update this post.");
    public static ErrorModel ErrorPostNotFound() => new(400, "Post not found.");
    public static ErrorModel ErrorUploadsPending() => new(400, "The're some uploads pending, complete the uploads or remove the files first.");
    public static ErrorModel ErrorMediaFileNotFound() => new(400, "Post not found.");
    public static ErrorModel ErrorMaxFilesQuantityReached() => new(400, "The maximum number of files has been reached.");
    public static ErrorModel ErrorAccessDenied() => new ErrorModel(403, "You can't access to the this post.");
}