using Azure.Storage.Blobs;
using Azure.Storage.Sas;

namespace HolaViaje.Social.BlobStorage;

public class BlobRepository(BlobServiceClient blobServiceClient) : IBlobRepository
{
    public async Task DeleteAsync(string containerName, string blobName)
    {
        var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
        await blobContainerClient.CreateIfNotExistsAsync();
        var blobClient = blobContainerClient.GetBlobClient(blobName);
        await blobClient.DeleteIfExistsAsync();
    }

    public async Task<string> UploadAsync(string containerName, string blobName, Stream stream)
    {
        var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
        await blobContainerClient.CreateIfNotExistsAsync();
        var blobClient = blobContainerClient.GetBlobClient(blobName);
        await blobClient.UploadAsync(stream, true);

        // Create a SAS token for the blob resource with a duration of 50 years
        var startsOn = DateTimeOffset.UtcNow.AddMinutes(-5);
        var expiresOn = DateTimeOffset.UtcNow.AddYears(50);

        return CreateSASUri(blobClient, BlobSasPermissions.Read, startsOn, expiresOn).ToString();
    }

    public async Task<BlobLinkModel> UploadLinkAsync(string containerName, string fileName, TimeSpan linkExpiration)
    {
        var startsOn = DateTimeOffset.UtcNow.AddMinutes(-5);
        var expiresOn = DateTimeOffset.UtcNow.Add(linkExpiration);

        BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
        await blobContainerClient.CreateIfNotExistsAsync();

        var blobClient = blobContainerClient.GetBlobClient(fileName);
        Uri blobSASURI = CreateSASUri(blobClient, BlobSasPermissions.Write, startsOn, expiresOn);
        return new BlobLinkModel(fileName, blobSASURI.AbsoluteUri, expiresOn);
    }

    private static Uri CreateSASUri(BlobClient blobClient, BlobSasPermissions permissions, DateTimeOffset startsOn, DateTimeOffset expiresOn)
    {
        // Create a SAS token for the blob resource
        BlobSasBuilder sasBuilder = new BlobSasBuilder()
        {
            BlobContainerName = blobClient.BlobContainerName,
            BlobName = blobClient.Name,
            Resource = "b",
            StartsOn = startsOn,
            ExpiresOn = expiresOn,
        };

        // Specify the necessary permissions
        sasBuilder.SetPermissions(permissions);

        return blobClient.GenerateSasUri(sasBuilder);
    }
}