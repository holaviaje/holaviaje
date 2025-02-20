namespace HolaViaje.Social.BlobStorage;

public record BlobLinkModel(string FileName, string AccessUrl, DateTimeOffset ExpiresOn) { }
