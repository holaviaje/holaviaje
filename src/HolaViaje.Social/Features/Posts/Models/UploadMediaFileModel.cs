
namespace HolaViaje.Social.Features.Posts.Models;

public record UploadMediaFileModel(string FileName, string ContentType, long FileSize, Stream FileStream) { }
