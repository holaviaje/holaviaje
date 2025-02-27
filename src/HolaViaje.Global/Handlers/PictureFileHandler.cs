using HolaViaje.Global.Validators;
using Microsoft.AspNetCore.Http;

namespace HolaViaje.Global.Handlers;

/// <summary>
/// Picture file handler
/// </summary>
public class PictureFileHandler
{
    /// <summary>
    /// Process file
    /// </summary>
    /// <param name="formFile">IFormFile object instance.</param>
    /// <param name="validExtensions">valid extensions list.</param>
    /// <param name="maxSize">Max size</param>
    /// <returns></returns>
    public static async Task<(bool success, string message, MemoryStream? fileStream)> ProcessFile(IFormFile formFile, string[] validExtensions, long maxSize)
    {
        string extension = Path.GetExtension(formFile.FileName);
        long size = formFile.Length;
        var (success, message) = FileValidator.Validate(extension, size, validExtensions, maxSize);

        if (!success)
        {
            return (false, message, null);
        }

        var mStream = new MemoryStream();
        await formFile.CopyToAsync(mStream);
        mStream.Position = 0;
        return (true, string.Empty, mStream);
    }
}
