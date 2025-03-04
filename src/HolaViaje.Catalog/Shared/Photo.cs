namespace HolaViaje.Catalog.Shared;

public sealed class Photo
{
    public Photo()
    {
        FileId = string.Empty;
        ImageUrl = string.Empty;
    }

    public Photo(string fileId, string imageUrl)
    {
        FileId = fileId;
        ImageUrl = imageUrl;
    }

    public string FileId { get; set; }
    public string ImageUrl { get; set; }
    public static string ContainerName = "catalog-photos";
    public static string[] ValidExtensions = [".png"];
    public static int MaxSize = 4 * 1024 * 1024;
}