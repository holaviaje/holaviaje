namespace HolaViaje.Social.Features.Profiles;

public record ProfilePicture(string Filename, string ImageUrl)
{
    public DateTime LastModifiedAt { get; init; } = DateTime.UtcNow;

    public static string ContainerName = "profile-pictures";
    public static string[] ValidExtensions = [".png"];
    public static int MaxSize = 512 * 1024;
}
