namespace HolaViaje.Social.BlobStorage;

public interface IBlobRepository
{
    Task<BlobLinkModel> UploadLinkAsync(string containerName, string fileName, TimeSpan linkExpiration);
    Task<string> UploadAsync(string containerName, string blobName, Stream stream);
    Task DeleteAsync(string containerName, string blobName);
}