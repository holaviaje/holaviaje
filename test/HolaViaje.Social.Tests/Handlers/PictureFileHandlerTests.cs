using HolaViaje.Social.Handlers;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Text;

namespace HolaViaje.Social.Tests.Handlers
{
    [TestClass]
    public class PictureFileHandlerTests
    {
        [TestMethod]
        public async Task ProcessFile_ValidFile_ReturnsSuccess()
        {
            // Arrange
            var validExtensions = new[] { ".jpg", ".png" };
            long maxSize = 1024 * 1024; // 1MB
            var fileMock = new Mock<IFormFile>();
            var content = "Fake file content";
            var fileName = "test.jpg";
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(content));
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.CopyToAsync(It.IsAny<Stream>(), default)).Callback<Stream, System.Threading.CancellationToken>((stream, token) => ms.CopyTo(stream)).Returns(Task.CompletedTask);

            // Act
            var result = await PictureFileHandler.ProcessFile(fileMock.Object, validExtensions, maxSize);

            // Assert
            Assert.IsTrue(result.success);
            Assert.AreEqual(string.Empty, result.message);
            Assert.IsNotNull(result.fileStream);
        }

        [TestMethod]
        public async Task ProcessFile_InvalidExtension_ReturnsFailure()
        {
            // Arrange
            var validExtensions = new[] { ".jpg", ".png" };
            long maxSize = 1024 * 1024; // 1MB
            var fileMock = new Mock<IFormFile>();
            var content = "Fake file content";
            var fileName = "test.txt";
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(content));
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.CopyToAsync(It.IsAny<Stream>(), default)).Callback<Stream, System.Threading.CancellationToken>((stream, token) => ms.CopyTo(stream)).Returns(Task.CompletedTask);

            // Act
            var result = await PictureFileHandler.ProcessFile(fileMock.Object, validExtensions, maxSize);

            // Assert
            Assert.IsFalse(result.success);
            Assert.AreEqual("Extension is not valid (.jpg,.png)", result.message);
        }

        [TestMethod]
        public async Task ProcessFile_ExceedsMaxSize_ReturnsFailure()
        {
            // Arrange
            var validExtensions = new[] { ".jpg", ".png" };
            long maxSize = 1024; // 1KB
            var fileMock = new Mock<IFormFile>();
            var content = "Fake file content";
            var fileName = "test.jpg";
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(content));
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(5 * 1024 * 1024);
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.CopyToAsync(It.IsAny<Stream>(), default)).Callback<Stream, System.Threading.CancellationToken>((stream, token) => ms.CopyTo(stream)).Returns(Task.CompletedTask);

            // Act
            var result = await PictureFileHandler.ProcessFile(fileMock.Object, validExtensions, maxSize);

            // Assert
            Assert.IsFalse(result.success);
        }
    }
}
