namespace HolaViaje.Social.Validators;

/// <summary>
/// File validator
/// </summary>
public static class FileValidator
{
    /// <summary>
    /// Validate file extension and size
    /// </summary>
    /// <param name="extension">file extension</param>
    /// <param name="size">file size</param>
    /// <param name="validExtensions">valid extension list.</param>
    /// <param name="maxSize">max size valid</param>
    /// <returns>True if valid, else false.</returns>
    public static (bool success, string message) Validate(string extension, long size, string[] validExtensions, long maxSize)
    {
        if (!validExtensions.Contains(extension))
        {
            return (false, $"Extension is not valid ({string.Join(',', validExtensions)})");
        }

        if (size > maxSize)
        {
            return (false, $"File is {size} bytes, Maximum size is {maxSize} bytes");
        }

        return (true, string.Empty);
    }
}