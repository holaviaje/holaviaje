namespace HolaViaje.Catalog.Shared;

public record Photo(string FileId, string ImageUrl)
{
    public static string ContainerName = "catalog-photos";
    public static string[] ValidExtensions = [".png"];
    public static int MaxSize = 4 * 1024 * 1024;
}