namespace HolaViaje.Infrastructure.BlobStorage;

public record BlobLinkModel(string FileName, string AccessUrl, DateTimeOffset ExpiresOn) { }
