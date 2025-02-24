namespace HolaViaje.Social.Features.Posts;

public record MediaFile(string FileId, string FileName, long FileSize, string ContentType, string ContainerName, string Url)
{
    /// <summary>
    /// Gets or sets if the file was uploaded.
    /// </summary>
    public bool Uploaded { get; set; }

    public static readonly string ImageContainerName = "post-images-1";
    public static readonly long ImageMaxSize = 50 * 1024 * 1024;
    public static readonly string[] ImageValidExtensions = [".jpg", ".png", ".bmp", ".gif"];

    public static readonly string VideoContainerName = "post-videos-1";
    public static readonly long VideoMaxSize = 1 * 1024 * 1024 * 1024;
    public static readonly string[] VideoValidExtensions = [".mp4", ".mov"];
}
