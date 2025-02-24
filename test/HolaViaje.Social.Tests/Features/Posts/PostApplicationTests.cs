using AutoMapper;
using HolaViaje.Infrastructure.BlobStorage;
using HolaViaje.Infrastructure.Messaging;
using HolaViaje.Social.Features.Posts;
using HolaViaje.Social.Features.Posts.Models;
using HolaViaje.Social.Features.Posts.Repository;
using HolaViaje.Social.Shared;
using Moq;

namespace HolaViaje.Social.Tests.Features.Posts
{
    [TestClass]
    public class PostApplicationTests
    {
        private Mock<IPostRepository> _postRepositoryMock;
        private Mock<IBlobRepository> _blobRepositoryMock;
        private Mock<IEventBus> _eventBusMock;
        private Mock<IMapper> _mapperMock;
        private PostApplication _postApplication;

        [TestInitialize]
        public void Setup()
        {
            _postRepositoryMock = new Mock<IPostRepository>();
            _blobRepositoryMock = new Mock<IBlobRepository>();
            _eventBusMock = new Mock<IEventBus>();
            _mapperMock = new Mock<IMapper>();
            _postApplication = new PostApplication(_postRepositoryMock.Object, _blobRepositoryMock.Object, _eventBusMock.Object, _mapperMock.Object);
        }

        [TestMethod]
        public async Task CreateAsync_ShouldReturnError_WhenPageIdIsGreaterThanZero()
        {
            var model = new PostModel { PageId = 1 };
            var result = await _postApplication.CreateAsync(model, 1);

            Assert.IsTrue(result.IsT1);
            Assert.AreEqual(PostErrorModelHelper.ErrorPublishOnPage(), result.AsT1);
        }

        [TestMethod]
        public async Task CreateAsync_ShouldReturnPostViewModel()
        {
            // Arrange
            var postModel = new PostModel { Content = "Test content", Type = PostType.Event, Visibility = Visibility.Public };
            var post = new Post(1, 0) { Content = "Test content", Type = PostType.Event, Visibility = Visibility.Public };
            var postViewModel = new PostViewModel { Content = "Test content", Type = PostType.Event, Visibility = Visibility.Public, Control = new() };

            _postRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<Post>(), It.IsAny<CancellationToken>())).ReturnsAsync(post);
            _mapperMock.Setup(mapper => mapper.Map<PostViewModel>(It.IsAny<Post>())).Returns(postViewModel);

            // Act
            var result = await _postApplication.CreateAsync(postModel, 1);

            // Assert
            Assert.IsTrue(result.IsT0);
            Assert.AreEqual(postViewModel, result.AsT0);
        }

        [TestMethod]
        public async Task UpdateAsync_ShouldReturnError_WhenPostNotFound()
        {
            _postRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<long>(), It.IsAny<bool>(), It.IsAny<CancellationToken>())).ReturnsAsync((Post)null);

            var result = await _postApplication.UpdateAsync(1, new PostBasicModel(), 1);

            Assert.IsTrue(result.IsT1);
            Assert.AreEqual(PostErrorModelHelper.ErrorPostNotFound(), result.AsT1);
        }

        [TestMethod]
        public async Task UpdateAsync_ShouldReturnPostViewModel()
        {
            // Arrange
            var postBasicModel = new PostBasicModel { Content = "Updated content", Visibility = Visibility.Public };
            var post = new Post(1, 0) { Content = "Test content", Type = PostType.Event, Visibility = Visibility.Public };
            var postViewModel = new PostViewModel { Content = "Updated content", Type = PostType.Event, Visibility = Visibility.Public, Control = new() };

            _postRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<long>(), It.IsAny<bool>(), It.IsAny<CancellationToken>())).ReturnsAsync(post);
            _postRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Post>(), It.IsAny<CancellationToken>())).ReturnsAsync(post);
            _mapperMock.Setup(mapper => mapper.Map<PostViewModel>(It.IsAny<Post>())).Returns(postViewModel);

            // Act
            var result = await _postApplication.UpdateAsync(1, postBasicModel, 1);

            // Assert
            Assert.IsTrue(result.IsT0);
            Assert.AreEqual(postViewModel, result.AsT0);
        }

        [TestMethod]
        public async Task PublishAsync_ShouldReturnError_WhenPostNotFound()
        {
            _postRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<long>(), It.IsAny<bool>(), It.IsAny<CancellationToken>())).ReturnsAsync((Post)null);

            var result = await _postApplication.PublishAsync(1, 1);

            Assert.IsTrue(result.IsT1);
            Assert.AreEqual(PostErrorModelHelper.ErrorPostNotFound(), result.AsT1);
        }

        [TestMethod]
        public async Task PublishAsync_ShouldReturnPostViewModel()
        {
            // Arrange
            var post = new Post(1, 0) { Content = "Test content", Type = PostType.Event, Visibility = Visibility.Public };
            var postViewModel = new PostViewModel { Content = "Test content", Type = PostType.Event, Visibility = Visibility.Public, Control = new() };

            _postRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<long>(), It.IsAny<bool>(), It.IsAny<CancellationToken>())).ReturnsAsync(post);
            _postRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Post>(), It.IsAny<CancellationToken>())).ReturnsAsync(post);
            _mapperMock.Setup(mapper => mapper.Map<PostViewModel>(It.IsAny<Post>())).Returns(postViewModel);

            // Act
            var result = await _postApplication.PublishAsync(1, 1);

            // Assert
            Assert.IsTrue(result.IsT0);
            Assert.AreEqual(postViewModel, result.AsT0);
        }

        [TestMethod]
        public async Task CreateUploadLinkAsync_ShouldReturnError_WhenPostNotFound()
        {
            _postRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<long>(), It.IsAny<bool>(), It.IsAny<CancellationToken>())).ReturnsAsync((Post)null);

            var result = await _postApplication.CreateUploadLinkAsync(1, new UploadLinkModel("hello.png", 1024), 1);

            Assert.IsTrue(result.IsT1);
            Assert.AreEqual(PostErrorModelHelper.ErrorPostNotFound(), result.AsT1);
        }

        [TestMethod]
        public async Task CreateUploadLinkAsync_ShouldReturnMediaFileModel()
        {
            // Arrange
            var userId = 1;
            var uploadLinkModel = new UploadLinkModel("hello.png", 1024);
            var post = new Post(userId, 0) { Content = "Test content", Type = PostType.Event, Visibility = Visibility.Public };
            var mediaFileModel = new MediaFileModel("test123", "test.jpg", "application/jpg", "localhost", false);
            var blobLinkModel = new BlobLinkModel("test123", "test.jpg", DateTimeOffset.Now.AddDays(1));

            _postRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<long>(), It.IsAny<bool>(), It.IsAny<CancellationToken>())).ReturnsAsync(post);
            _postRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Post>(), It.IsAny<CancellationToken>())).ReturnsAsync(post);
            _blobRepositoryMock.Setup(repo => repo.UploadLinkAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<TimeSpan>())).ReturnsAsync(blobLinkModel);
            _mapperMock.Setup(mapper => mapper.Map<MediaFileModel>(It.IsAny<Post>())).Returns(mediaFileModel);

            // Act
            var result = await _postApplication.CreateUploadLinkAsync(1, uploadLinkModel, userId);

            // Assert
            Assert.IsTrue(result.IsT0);
            Assert.AreEqual(mediaFileModel, result.AsT0);
        }

        [TestMethod]
        public async Task MarkAsUploadedAsync_ShouldReturnError_WhenPostNotFound()
        {
            _postRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<long>(), It.IsAny<bool>(), It.IsAny<CancellationToken>())).ReturnsAsync((Post)null);

            var result = await _postApplication.MarkAsUploadedAsync(1, "fileId", 1);

            Assert.IsTrue(result.IsT1);
            Assert.AreEqual(PostErrorModelHelper.ErrorPostNotFound(), result.AsT1);
        }

        [TestMethod]
        public async Task MarkAsUploadedAsync_ShouldReturnMediaFileModel()
        {
            // Arrange
            var post = new Post(1, 0) { Content = "Test content", Type = PostType.Event, Visibility = Visibility.Public };
            post.AddMediaFile(new MediaFile("fileId", "test.jpg", 1024, "application/jpg", MediaFile.ImageContainerName, "localhost"));

            var mediaFileModel = new MediaFileModel("test123", "test.jpg", "application/jpg", "localhost", true);

            _postRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<long>(), It.IsAny<bool>(), It.IsAny<CancellationToken>())).ReturnsAsync(post);
            _postRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Post>(), It.IsAny<CancellationToken>())).ReturnsAsync(post);
            _mapperMock.Setup(mapper => mapper.Map<MediaFileModel>(It.IsAny<Post>())).Returns(mediaFileModel);

            // Act
            var result = await _postApplication.MarkAsUploadedAsync(1, "fileId", 1);

            // Assert
            Assert.IsTrue(result.IsT0);
            Assert.AreEqual(mediaFileModel, result.AsT0);
        }

        [TestMethod]
        public async Task DeleteMediaFileAsync_ShouldReturnError_WhenPostNotFound()
        {
            _postRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<long>(), It.IsAny<bool>(), It.IsAny<CancellationToken>())).ReturnsAsync((Post)null);

            var result = await _postApplication.DeleteMediaFileAsync(1, "fileId", 1);

            Assert.IsTrue(result.IsT1);
            Assert.AreEqual(PostErrorModelHelper.ErrorPostNotFound(), result.AsT1);
        }

        [TestMethod]
        public async Task DeleteMediaFileAsync_ShouldReturnMediaFileModel()
        {
            // Arrange
            var post = new Post(1, 0) { Content = "Test content", Type = PostType.Event, Visibility = Visibility.Public };
            post.AddMediaFile(new MediaFile("fileId", "test.jpg", 1024, "application/jpg", MediaFile.ImageContainerName, "localhost"));
            var mediaFileModel = new MediaFileModel("test123", "test.jpg", "application/jpg", "localhost", true);

            _postRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<long>(), It.IsAny<bool>(), It.IsAny<CancellationToken>())).ReturnsAsync(post);
            _postRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Post>(), It.IsAny<CancellationToken>())).ReturnsAsync(post);
            _mapperMock.Setup(mapper => mapper.Map<MediaFileModel>(It.IsAny<Post>())).Returns(mediaFileModel);

            // Act
            var result = await _postApplication.DeleteMediaFileAsync(1, "fileId", 1);

            // Assert
            Assert.IsTrue(result.IsT0);
            Assert.AreEqual(mediaFileModel, result.AsT0);
        }

        [TestMethod]
        public async Task DeleteAsync_ShouldReturnError_WhenPostNotFound()
        {
            _postRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<long>(), It.IsAny<bool>(), It.IsAny<CancellationToken>())).ReturnsAsync((Post)null);

            var result = await _postApplication.DeleteAsync(1, 1);

            Assert.IsTrue(result.IsT1);
            Assert.AreEqual(PostErrorModelHelper.ErrorPostNotFound(), result.AsT1);
        }

        [TestMethod]
        public async Task DeleteAsync_ShouldReturnPostViewModel()
        {
            // Arrange
            var post = new Post(1, 0) { Content = "Test content", Type = PostType.Event, Visibility = Visibility.Public };
            var postViewModel = new PostViewModel { Content = "Test content", Type = PostType.Event, Visibility = Visibility.Public, Control = new() };

            _postRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<long>(), It.IsAny<bool>(), It.IsAny<CancellationToken>())).ReturnsAsync(post);
            _postRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Post>(), It.IsAny<CancellationToken>())).ReturnsAsync(post);
            _mapperMock.Setup(mapper => mapper.Map<PostViewModel>(It.IsAny<Post>())).Returns(postViewModel);

            // Act
            var result = await _postApplication.DeleteAsync(1, 1);

            // Assert
            Assert.IsTrue(result.IsT0);
            Assert.AreEqual(postViewModel, result.AsT0);
        }

        [TestMethod]
        public async Task GetAsync_ShouldReturnError_WhenPostNotFound()
        {
            _postRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<long>(), It.IsAny<bool>(), It.IsAny<CancellationToken>())).ReturnsAsync((Post)null);

            var result = await _postApplication.GetAsync(1, 1);

            Assert.IsTrue(result.IsT1);
            Assert.AreEqual(PostErrorModelHelper.ErrorPostNotFound(), result.AsT1);
        }

        [TestMethod]
        public async Task GetAsync_ShouldReturnPostViewModel()
        {
            // Arrange
            var post = new Post(1, 0) { Content = "Test content", Type = PostType.Event, Visibility = Visibility.Public };
            var postViewModel = new PostViewModel { Content = "Test content", Type = PostType.Event, Visibility = Visibility.Public, Control = new() };

            _postRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<long>(), false, It.IsAny<CancellationToken>())).ReturnsAsync(post);
            _mapperMock.Setup(mapper => mapper.Map<PostViewModel>(It.IsAny<Post>())).Returns(postViewModel);

            // Act
            var result = await _postApplication.GetAsync(1, 1);

            // Assert
            Assert.IsTrue(result.IsT0);
            Assert.AreEqual(postViewModel, result.AsT0);
        }
    }
}
