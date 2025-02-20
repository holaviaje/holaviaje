using HolaViaje.Social.Validators;

namespace HolaViaje.Social.Tests.Validators
{
    [TestClass]
    public class FileValidatorTests
    {
        [TestMethod]
        public void Validate_InvalidExtension_ReturnsFalse()
        {
            // Arrange
            string extension = ".exe";
            long size = 1024;
            string[] validExtensions = { ".jpg", ".png", ".gif" };
            long maxSize = 2048;

            // Act
            var result = FileValidator.Validate(extension, size, validExtensions, maxSize);

            // Assert
            Assert.IsFalse(result.success);
            Assert.AreEqual("Extension is not valid (.jpg,.png,.gif)", result.message);
        }

        [TestMethod]
        public void Validate_ValidExtensionAndSize_ReturnsTrue()
        {
            // Arrange
            string extension = ".jpg";
            long size = 1024;
            string[] validExtensions = { ".jpg", ".png", ".gif" };
            long maxSize = 2048;

            // Act
            var result = FileValidator.Validate(extension, size, validExtensions, maxSize);

            // Assert
            Assert.IsTrue(result.success);
            Assert.AreEqual(string.Empty, result.message);
        }

        [TestMethod]
        public void Validate_SizeExceedsMaxSize_ReturnsFalse()
        {
            // Arrange
            string extension = ".jpg";
            long size = 4096;
            string[] validExtensions = { ".jpg", ".png", ".gif" };
            long maxSize = 2048;

            // Act
            var result = FileValidator.Validate(extension, size, validExtensions, maxSize);

            // Assert
            Assert.IsFalse(result.success);
            Assert.AreEqual("File is 4096 bytes, Maximum size is 2048 bytes", result.message);
        }
    }
}
