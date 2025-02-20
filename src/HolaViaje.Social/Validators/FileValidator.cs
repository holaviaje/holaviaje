namespace HolaViaje.Social.Validators
{
    public static class FileValidator
    {
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
}