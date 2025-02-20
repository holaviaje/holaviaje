using AutoMapper;
using HolaViaje.Social.Features.Profiles;
using HolaViaje.Social.Features.Profiles.Models;
using HolaViaje.Social.Features.Profiles.Repository;
using HolaViaje.Social.Shared.Models;
using Moq;

namespace HolaViaje.Social.Tests.Features.Profiles
{
    [TestClass]
    public class UserProfileApplicationTests
    {
        private Mock<IUserProfileRepository> _profileRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private UserProfileApplication _userProfileApplication;

        [TestInitialize]
        public void Setup()
        {
            _profileRepositoryMock = new Mock<IUserProfileRepository>();
            _mapperMock = new Mock<IMapper>();
            _userProfileApplication = new UserProfileApplication(_profileRepositoryMock.Object, _mapperMock.Object);
        }

        [TestMethod]
        public async Task CreateAsync_ProfileAlreadyExists_ReturnsError()
        {
            // Arrange
            var userId = 1L;
            _profileRepositoryMock.Setup(repo => repo.GetAsync(userId, It.IsAny<bool>())).ReturnsAsync(new UserProfile(userId));

            // Act
            var result = await _userProfileApplication.CreateAsync(userId);

            // Assert
            Assert.IsTrue(result.IsT1);
            Assert.AreEqual(UserProfileErrorHelper.ProfileAlreadyExists(), result.AsT1);
        }

        [TestMethod]
        public async Task CreateAsync_ProfileDoesNotExist_CreatesProfile()
        {
            // Arrange
            var userId = 1L;
            var userProfile = new UserProfile(userId);
            var userProfileViewModel = new UserProfileViewModel { Availability = new(true, null), Place = new(), Control = new() };
            _profileRepositoryMock.Setup(repo => repo.GetAsync(userId, It.IsAny<bool>())).ReturnsAsync((UserProfile)null);
            _profileRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<UserProfile>())).ReturnsAsync(userProfile);
            _mapperMock.Setup(mapper => mapper.Map<UserProfileViewModel>(userProfile)).Returns(userProfileViewModel);

            // Act
            var result = await _userProfileApplication.CreateAsync(userId);

            // Assert
            Assert.IsTrue(result.IsT0);
            Assert.AreEqual(userProfileViewModel, result.AsT0);
        }

        [TestMethod]
        public async Task UpdateAsync_ProfileNotFound_ReturnsError()
        {
            // Arrange
            var profileId = 1L;
            var userId = 1L;
            var model = new UserProfileModel();
            _profileRepositoryMock.Setup(repo => repo.GetAsync(profileId, true)).ReturnsAsync((UserProfile)null);

            // Act
            var result = await _userProfileApplication.UpdateAsync(profileId, model, userId);

            // Assert
            Assert.IsTrue(result.IsT1);
            Assert.AreEqual(UserProfileErrorHelper.ProfileNotFound(), result.AsT1);
        }

        [TestMethod]
        public async Task UpdateAsync_ProfileAccessDenied_ReturnsError()
        {
            // Arrange
            var profileId = 1L;
            var userId = 2L;
            var model = new UserProfileModel();
            var userProfile = new UserProfile(profileId);
            _profileRepositoryMock.Setup(repo => repo.GetAsync(profileId, true)).ReturnsAsync(userProfile);

            // Act
            var result = await _userProfileApplication.UpdateAsync(profileId, model, userId);

            // Assert
            Assert.IsTrue(result.IsT1);
            Assert.AreEqual(UserProfileErrorHelper.ProfileAccessDenied(), result.AsT1);
        }

        [TestMethod]
        public async Task UpdateAsync_ProfileUpdated_ReturnsUpdatedProfile()
        {
            // Arrange
            var profileId = 1L;
            var userId = 1L;
            var model = new UserProfileModel();
            var userProfile = new UserProfile(userId);
            var userProfileViewModel = new UserProfileViewModel { Availability = new(true, null), Place = new(), Control = new() };
            _profileRepositoryMock.Setup(repo => repo.GetAsync(profileId, true)).ReturnsAsync(userProfile);
            _profileRepositoryMock.Setup(repo => repo.UpdateAsync(userProfile)).ReturnsAsync(userProfile);
            _mapperMock.Setup(mapper => mapper.Map(model, userProfile)).Returns(userProfile);
            _mapperMock.Setup(mapper => mapper.Map<UserProfileViewModel>(userProfile)).Returns(userProfileViewModel);

            // Act
            var result = await _userProfileApplication.UpdateAsync(profileId, model, userId);

            // Assert
            Assert.IsTrue(result.IsT0);
            Assert.AreEqual(userProfileViewModel, result.AsT0);
        }

        [TestMethod]
        public async Task UpdateAvailabilityAsync_ProfileNotFound_ReturnsError()
        {
            // Arrange
            var profileId = 1L;
            var userId = 1L;
            var model = new AvailabilityModel(true, null);
            _profileRepositoryMock.Setup(repo => repo.GetAsync(profileId, true)).ReturnsAsync((UserProfile)null);

            // Act
            var result = await _userProfileApplication.UpdateAvailabilityAsync(profileId, model, userId);

            // Assert
            Assert.IsTrue(result.IsT1);
            Assert.AreEqual(UserProfileErrorHelper.ProfileNotFound(), result.AsT1);
        }

        [TestMethod]
        public async Task UpdateAvailabilityAsync_ProfileAccessDenied_ReturnsError()
        {
            // Arrange
            var profileId = 1L;
            var userId = 2L;
            var model = new AvailabilityModel(true, null);
            var userProfile = new UserProfile(profileId);
            _profileRepositoryMock.Setup(repo => repo.GetAsync(profileId, true)).ReturnsAsync(userProfile);

            // Act
            var result = await _userProfileApplication.UpdateAvailabilityAsync(profileId, model, userId);

            // Assert
            Assert.IsTrue(result.IsT1);
            Assert.AreEqual(UserProfileErrorHelper.ProfileAccessDenied(), result.AsT1);
        }

        [TestMethod]
        public async Task UpdateAvailabilityAsync_ProfileUpdated_ReturnsUpdatedProfile()
        {
            // Arrange
            var profileId = 1L;
            var userId = 1L;
            var model = new AvailabilityModel(true, null);
            var userProfile = new UserProfile(userId);
            var userProfileViewModel = new UserProfileViewModel { Availability = model, Place = new(), Control = new() };
            _profileRepositoryMock.Setup(repo => repo.GetAsync(profileId, true)).ReturnsAsync(userProfile);
            _profileRepositoryMock.Setup(repo => repo.UpdateAsync(userProfile)).ReturnsAsync(userProfile);
            _mapperMock.Setup(mapper => mapper.Map<UserProfileViewModel>(userProfile)).Returns(userProfileViewModel);

            // Act
            var result = await _userProfileApplication.UpdateAvailabilityAsync(profileId, model, userId);

            // Assert
            Assert.IsTrue(result.IsT0);
            Assert.AreEqual(userProfileViewModel, result.AsT0);
        }

        [TestMethod]
        public async Task UpdatePlaceAsync_ProfileNotFound_ReturnsError()
        {
            // Arrange
            var profileId = 1L;
            var userId = 1L;
            var model = new PlaceInfoModel();
            _profileRepositoryMock.Setup(repo => repo.GetAsync(profileId, true)).ReturnsAsync((UserProfile)null);

            // Act
            var result = await _userProfileApplication.UpdatePlaceAsync(profileId, model, userId);

            // Assert
            Assert.IsTrue(result.IsT1);
            Assert.AreEqual(UserProfileErrorHelper.ProfileNotFound(), result.AsT1);
        }

        [TestMethod]
        public async Task UpdatePlaceAsync_ProfileAccessDenied_ReturnsError()
        {
            // Arrange
            var profileId = 1L;
            var userId = 2L;
            var model = new PlaceInfoModel();
            var userProfile = new UserProfile(profileId);
            _profileRepositoryMock.Setup(repo => repo.GetAsync(profileId, true)).ReturnsAsync(userProfile);

            // Act
            var result = await _userProfileApplication.UpdatePlaceAsync(profileId, model, userId);

            // Assert
            Assert.IsTrue(result.IsT1);
            Assert.AreEqual(UserProfileErrorHelper.ProfileAccessDenied(), result.AsT1);
        }

        [TestMethod]
        public async Task UpdatePlaceAsync_ProfileUpdated_ReturnsUpdatedProfile()
        {
            // Arrange
            var profileId = 1L;
            var userId = 1L;
            var model = new PlaceInfoModel();
            var userProfile = new UserProfile(userId);
            var userProfileViewModel = new UserProfileViewModel { Availability = new(true, null), Place = new(), Control = new() };
            _profileRepositoryMock.Setup(repo => repo.GetAsync(profileId, true)).ReturnsAsync(userProfile);
            _profileRepositoryMock.Setup(repo => repo.UpdateAsync(userProfile)).ReturnsAsync(userProfile);
            _mapperMock.Setup(mapper => mapper.Map<UserProfileViewModel>(userProfile)).Returns(userProfileViewModel);

            // Act
            var result = await _userProfileApplication.UpdatePlaceAsync(profileId, model, userId);

            // Assert
            Assert.IsTrue(result.IsT0);
            Assert.AreEqual(userProfileViewModel, result.AsT0);
        }

        [TestMethod]
        public async Task UpdateSpokenLanguagesAsync_ProfileNotFound_ReturnsError()
        {
            // Arrange
            var profileId = 1L;
            var userId = 1L;
            var models = new List<SpokenLanguageModel>();
            _profileRepositoryMock.Setup(repo => repo.GetAsync(profileId, true)).ReturnsAsync((UserProfile)null);

            // Act
            var result = await _userProfileApplication.UpdateSpokenLanguagesAsync(profileId, models, userId);

            // Assert
            Assert.IsTrue(result.IsT1);
            Assert.AreEqual(UserProfileErrorHelper.ProfileNotFound(), result.AsT1);
        }

        [TestMethod]
        public async Task UpdateSpokenLanguagesAsync_ProfileAccessDenied_ReturnsError()
        {
            // Arrange
            var profileId = 1L;
            var userId = 2L;
            var models = new List<SpokenLanguageModel>();
            var userProfile = new UserProfile(profileId);
            _profileRepositoryMock.Setup(repo => repo.GetAsync(profileId, true)).ReturnsAsync(userProfile);

            // Act
            var result = await _userProfileApplication.UpdateSpokenLanguagesAsync(profileId, models, userId);

            // Assert
            Assert.IsTrue(result.IsT1);
            Assert.AreEqual(UserProfileErrorHelper.ProfileAccessDenied(), result.AsT1);
        }

        [TestMethod]
        public async Task UpdateSpokenLanguagesAsync_ProfileUpdated_ReturnsUpdatedProfile()
        {
            // Arrange
            var profileId = 1L;
            var userId = 1L;
            var models = new List<SpokenLanguageModel>();
            var userProfile = new UserProfile(userId);
            var userProfileViewModel = new UserProfileViewModel { Availability = new(true, null), Place = new(), Control = new() };
            _profileRepositoryMock.Setup(repo => repo.GetAsync(profileId, true)).ReturnsAsync(userProfile);
            _profileRepositoryMock.Setup(repo => repo.UpdateAsync(userProfile)).ReturnsAsync(userProfile);
            _mapperMock.Setup(mapper => mapper.Map<UserProfileViewModel>(userProfile)).Returns(userProfileViewModel);

            // Act
            var result = await _userProfileApplication.UpdateSpokenLanguagesAsync(profileId, models, userId);

            // Assert
            Assert.IsTrue(result.IsT0);
            Assert.AreEqual(userProfileViewModel, result.AsT0);
        }

        [TestMethod]
        public async Task UpdatePictureAsync_ProfileNotFound_ReturnsError()
        {
            // Arrange
            var profileId = 1L;
            var userId = 1L;
            var fileName = "test.png";
            var imageUrl = "http://example.com/test.png";
            _profileRepositoryMock.Setup(repo => repo.GetAsync(profileId, true)).ReturnsAsync((UserProfile)null);

            // Act
            var result = await _userProfileApplication.UpdatePictureAsync(profileId, fileName, imageUrl, userId);

            // Assert
            Assert.IsTrue(result.IsT1);
            Assert.AreEqual(UserProfileErrorHelper.ProfileNotFound(), result.AsT1);
        }

        [TestMethod]
        public async Task UpdatePictureAsync_ProfileAccessDenied_ReturnsError()
        {
            // Arrange
            var profileId = 1L;
            var userId = 2L;
            var fileName = "test.png";
            var imageUrl = "http://example.com/test.png";
            var userProfile = new UserProfile(profileId);
            _profileRepositoryMock.Setup(repo => repo.GetAsync(profileId, true)).ReturnsAsync(userProfile);

            // Act
            var result = await _userProfileApplication.UpdatePictureAsync(profileId, fileName, imageUrl, userId);

            // Assert
            Assert.IsTrue(result.IsT1);
            Assert.AreEqual(UserProfileErrorHelper.ProfileAccessDenied(), result.AsT1);
        }

        [TestMethod]
        public async Task UpdatePictureAsync_ProfileUpdated_ReturnsUpdatedProfile()
        {
            // Arrange
            var profileId = 1L;
            var userId = 1L;
            var fileName = "test.png";
            var imageUrl = "http://example.com/test.png";
            var userProfile = new UserProfile(userId);
            var userProfileViewModel = new UserProfileViewModel { Availability = new(true, null), Place = new(), Control = new() };
            _profileRepositoryMock.Setup(repo => repo.GetAsync(profileId, true)).ReturnsAsync(userProfile);
            _profileRepositoryMock.Setup(repo => repo.UpdateAsync(userProfile)).ReturnsAsync(userProfile);
            _mapperMock.Setup(mapper => mapper.Map<UserProfileViewModel>(userProfile)).Returns(userProfileViewModel);

            // Act
            var result = await _userProfileApplication.UpdatePictureAsync(profileId, fileName, imageUrl, userId);

            // Assert
            Assert.IsTrue(result.IsT0);
            Assert.AreEqual(userProfileViewModel, result.AsT0);
        }

        [TestMethod]
        public async Task DeleteAsync_ProfileNotFound_ReturnsError()
        {
            // Arrange
            var profileId = 1L;
            var userId = 1L;
            _profileRepositoryMock.Setup(repo => repo.GetAsync(profileId, true)).ReturnsAsync((UserProfile)null);

            // Act
            var result = await _userProfileApplication.DeleteAsync(profileId, userId);

            // Assert
            Assert.IsTrue(result.IsT1);
            Assert.AreEqual(UserProfileErrorHelper.ProfileNotFound(), result.AsT1);
        }

        [TestMethod]
        public async Task DeleteAsync_ProfileAccessDenied_ReturnsError()
        {
            // Arrange
            var profileId = 1L;
            var userId = 2L;
            var userProfile = new UserProfile(profileId);
            _profileRepositoryMock.Setup(repo => repo.GetAsync(profileId, true)).ReturnsAsync(userProfile);

            // Act
            var result = await _userProfileApplication.DeleteAsync(profileId, userId);

            // Assert
            Assert.IsTrue(result.IsT1);
            Assert.AreEqual(UserProfileErrorHelper.ProfileAccessDenied(), result.AsT1);
        }

        [TestMethod]
        public async Task DeleteAsync_ProfileDeleted_ReturnsDeletedProfile()
        {
            // Arrange
            var profileId = 1L;
            var userId = 1L;
            var userProfile = new UserProfile(userId);
            var userProfileViewModel = new UserProfileViewModel { Availability = new(true, null), Place = new(), Control = new() };
            _profileRepositoryMock.Setup(repo => repo.GetAsync(profileId, true)).ReturnsAsync(userProfile);
            _profileRepositoryMock.Setup(repo => repo.UpdateAsync(userProfile)).ReturnsAsync(userProfile);
            _mapperMock.Setup(mapper => mapper.Map<UserProfileViewModel>(userProfile)).Returns(userProfileViewModel);

            // Act
            var result = await _userProfileApplication.DeleteAsync(profileId, userId);

            // Assert
            Assert.IsTrue(result.IsT0);
            Assert.AreEqual(userProfileViewModel, result.AsT0);
        }

        [TestMethod]
        public async Task GetAsync_ProfileNotFound_ReturnsError()
        {
            // Arrange
            var profileId = 1L;
            var userId = 1L;
            _profileRepositoryMock.Setup(repo => repo.GetAsync(profileId, It.IsAny<bool>())).ReturnsAsync((UserProfile)null);

            // Act
            var result = await _userProfileApplication.GetAsync(profileId, userId);

            // Assert
            Assert.IsTrue(result.IsT1);
            Assert.AreEqual(UserProfileErrorHelper.ProfileNotFound(), result.AsT1);
        }

        [TestMethod]
        public async Task GetAsync_ProfileAccessDenied_ReturnsError()
        {
            // Arrange
            var profileId = 1L;
            var userId = 2L;
            var userProfile = new UserProfile(profileId) { Visibility = Visibility.Private };
            _profileRepositoryMock.Setup(repo => repo.GetAsync(profileId, It.IsAny<bool>())).ReturnsAsync(userProfile);

            // Act
            var result = await _userProfileApplication.GetAsync(profileId, userId);

            // Assert
            Assert.IsTrue(result.IsT1);
            Assert.AreEqual(UserProfileErrorHelper.ProfileAccessDenied(), result.AsT1);
        }

        [TestMethod]
        public async Task GetAsync_ProfileFound_ReturnsProfile()
        {
            // Arrange
            var profileId = 1L;
            var userId = 1L;
            var userProfile = new UserProfile(userId);
            var userProfileViewModel = new UserProfileViewModel { Availability = new(true, null), Place = new(), Control = new() };
            _profileRepositoryMock.Setup(repo => repo.GetAsync(profileId, It.IsAny<bool>())).ReturnsAsync(userProfile);
            _mapperMock.Setup(mapper => mapper.Map<UserProfileViewModel>(userProfile)).Returns(userProfileViewModel);

            // Act
            var result = await _userProfileApplication.GetAsync(profileId, userId);

            // Assert
            Assert.IsTrue(result.IsT0);
            Assert.AreEqual(userProfileViewModel, result.AsT0);
        }
    }
}
