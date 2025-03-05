using AutoMapper;
using HolaViaje.Catalog.Features.Experiences;
using HolaViaje.Catalog.Features.Experiences.Models;
using HolaViaje.Catalog.Features.Experiences.Repository;
using Moq;

namespace HolaViaje.Catalog.Tests.Features.Experiences
{
    [TestClass]
    public class ExperienceApplicationTests
    {
        private Mock<IExperienceRepository> _experienceRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private IExperienceApplication _experienceApplication;

        [TestInitialize]
        public void Setup()
        {
            _experienceRepositoryMock = new Mock<IExperienceRepository>();
            _mapperMock = new Mock<IMapper>();
            _experienceApplication = new ExperienceApplication(_experienceRepositoryMock.Object, _mapperMock.Object);
        }

        [TestMethod]
        public async Task CreateAsync_ShouldReturnTranslationMissingError_WhenNoTranslations()
        {
            var model = new ExperienceModel { Translations = [] };
            var result = await _experienceApplication.CreateAsync(model, 1);

            Assert.IsTrue(result.IsT1);
            Assert.AreEqual(ExperienceErrorModelHelper.TranslationMissingError(), result.AsT1);
        }

        [TestMethod]
        public async Task CreateAsync_ShouldReturnTranslationLanguageCodeMismatchError_WhenLanguageCodesNotUnique()
        {
            var model = new ExperienceModel
            {
                Translations =
                [
                    new ExperienceTranslationModel { LanguageCode = "en" },
                    new ExperienceTranslationModel { LanguageCode = "en" }
                ]
            };
            var result = await _experienceApplication.CreateAsync(model, 1);

            Assert.IsTrue(result.IsT1);
            Assert.AreEqual(ExperienceErrorModelHelper.TranslationLanguageCodeMismatchError(), result.AsT1);
        }

        [TestMethod]
        public async Task CreateAsync_ShouldReturnExperienceViewModel_WhenValidModel()
        {
            var model = new ExperienceModel
            {
                Translations =
                [
                    new ExperienceTranslationModel { LanguageCode = "en" }
                ]
            };
            var experience = new Experience();
            _experienceRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<Experience>(), It.IsAny<CancellationToken>())).ReturnsAsync(experience);
            _mapperMock.Setup(x => x.Map<ExperienceViewModel>(It.IsAny<Experience>())).Returns(new ExperienceViewModel());

            var result = await _experienceApplication.CreateAsync(model, 1);

            Assert.IsTrue(result.IsT0);
        }

        [TestMethod]
        public async Task UpdateAsync_ShouldReturnExperienceNotFoundError_WhenExperienceNotFound()
        {
            var model = new ExperienceModel();
            _experienceRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<string>(), true, It.IsAny<CancellationToken>())).ReturnsAsync((Experience)null);

            var result = await _experienceApplication.UpdateAsync(Guid.NewGuid(), model, 1);

            Assert.IsTrue(result.IsT1);
            Assert.AreEqual(ExperienceErrorModelHelper.ExperienceNotFoundError(), result.AsT1);
        }

        [TestMethod]
        public async Task UpdateAsync_ShouldReturnExperienceViewModel_WhenValidModel()
        {
            var model = new ExperienceModel
            {
                Translations =
                [
                    new ExperienceTranslationModel { LanguageCode = "en" }
                ]
            };
            var experience = new Experience();
            _experienceRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<string[]>(), It.IsAny<bool>(), It.IsAny<CancellationToken>())).ReturnsAsync(experience);
            _experienceRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Experience>(), It.IsAny<CancellationToken>())).ReturnsAsync(experience);
            _mapperMock.Setup(x => x.Map<ExperienceViewModel>(It.IsAny<Experience>())).Returns(new ExperienceViewModel());

            var result = await _experienceApplication.UpdateAsync(Guid.NewGuid(), model, 1);

            Assert.IsTrue(result.IsT0);
        }

        [TestMethod]
        public async Task AddTranslationAsync_ShouldReturnTranslationMissingError_WhenModelIsNull()
        {
            var result = await _experienceApplication.AddTranslationAsync(Guid.NewGuid(), null, 1);

            Assert.IsTrue(result.IsT1);
            Assert.AreEqual(ExperienceErrorModelHelper.TranslationMissingError(), result.AsT1);
        }

        [TestMethod]
        public async Task AddTranslationAsync_ShouldReturnLanguageCodeMissingError_WhenLanguageCodeIsEmpty()
        {
            var model = new ExperienceTranslationModel { LanguageCode = string.Empty };
            var result = await _experienceApplication.AddTranslationAsync(Guid.NewGuid(), model, 1);

            Assert.IsTrue(result.IsT1);
            Assert.AreEqual(ExperienceErrorModelHelper.LanguageCodeMissingError(), result.AsT1);
        }

        [TestMethod]
        public async Task AddTranslationAsync_ShouldReturnExperienceNotFoundError_WhenExperienceNotFound()
        {
            var model = new ExperienceTranslationModel { LanguageCode = "en" };
            _experienceRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<string>(), true, It.IsAny<CancellationToken>())).ReturnsAsync((Experience)null);

            var result = await _experienceApplication.AddTranslationAsync(Guid.NewGuid(), model, 1);

            Assert.IsTrue(result.IsT1);
            Assert.AreEqual(ExperienceErrorModelHelper.ExperienceNotFoundError(), result.AsT1);
        }

        [TestMethod]
        public async Task AddTranslationAsync_ShouldReturnTranslationAlreadyExistsError_WhenTranslationExists()
        {
            var model = new ExperienceTranslationModel { LanguageCode = "en" };
            var experience = new Experience { Translations = [new ExperienceTranslation()] };
            _experienceRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<string>(), true, It.IsAny<CancellationToken>())).ReturnsAsync(experience);

            var result = await _experienceApplication.AddTranslationAsync(Guid.NewGuid(), model, 1);

            Assert.IsTrue(result.IsT1);
            Assert.AreEqual(ExperienceErrorModelHelper.TranslationAlreadyExistsError(), result.AsT1);
        }

        [TestMethod]
        public async Task AddTranslationAsync_ShouldReturnExperienceViewModel_WhenValidModel()
        {
            var model = new ExperienceTranslationModel { LanguageCode = "en" };
            var experience = new Experience();
            _experienceRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<string>(), true, It.IsAny<CancellationToken>())).ReturnsAsync(experience);
            _experienceRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Experience>(), It.IsAny<CancellationToken>())).ReturnsAsync(experience);
            _mapperMock.Setup(x => x.Map<ExperienceViewModel>(It.IsAny<Experience>())).Returns(new ExperienceViewModel());

            var result = await _experienceApplication.AddTranslationAsync(Guid.NewGuid(), model, 1);

            Assert.IsTrue(result.IsT0);
        }

        [TestMethod]
        public async Task UpdateTranslationAsync_ShouldReturnTranslationMissingError_WhenModelIsNull()
        {
            var result = await _experienceApplication.UpdateTranslationAsync(Guid.NewGuid(), null, 1);

            Assert.IsTrue(result.IsT1);
            Assert.AreEqual(ExperienceErrorModelHelper.TranslationMissingError(), result.AsT1);
        }

        [TestMethod]
        public async Task UpdateTranslationAsync_ShouldReturnExperienceNotFoundError_WhenExperienceNotFound()
        {
            var model = new ExperienceTranslationModel { LanguageCode = "en" };
            _experienceRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<string>(), true, It.IsAny<CancellationToken>())).ReturnsAsync((Experience)null);

            var result = await _experienceApplication.UpdateTranslationAsync(Guid.NewGuid(), model, 1);

            Assert.IsTrue(result.IsT1);
            Assert.AreEqual(ExperienceErrorModelHelper.ExperienceNotFoundError(), result.AsT1);
        }

        [TestMethod]
        public async Task UpdateTranslationAsync_ShouldReturnTranslationMissingError_WhenNoTranslations()
        {
            var model = new ExperienceTranslationModel { LanguageCode = "en" };
            var experience = new Experience { Translations = [] };
            _experienceRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<string>(), true, It.IsAny<CancellationToken>())).ReturnsAsync(experience);

            var result = await _experienceApplication.UpdateTranslationAsync(Guid.NewGuid(), model, 1);

            Assert.IsTrue(result.IsT1);
            Assert.AreEqual(ExperienceErrorModelHelper.TranslationMissingError(), result.AsT1);
        }

        [TestMethod]
        public async Task UpdateTranslationAsync_ShouldReturnTranslationLanguageCodeMismatchError_WhenLanguageCodesMismatch()
        {
            var model = new ExperienceTranslationModel { LanguageCode = "en" };
            var experience = new Experience { Translations = [new ExperienceTranslation { LanguageCode = "fr" }] };
            _experienceRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<string>(), true, It.IsAny<CancellationToken>())).ReturnsAsync(experience);

            var result = await _experienceApplication.UpdateTranslationAsync(Guid.NewGuid(), model, 1);

            Assert.IsTrue(result.IsT1);
            Assert.AreEqual(ExperienceErrorModelHelper.TranslationLanguageCodeMismatchError(), result.AsT1);
        }

        [TestMethod]
        public async Task UpdateTranslationAsync_ShouldReturnExperienceViewModel_WhenValidModel()
        {
            var model = new ExperienceTranslationModel { LanguageCode = "en" };
            var experience = new Experience { Translations = [new ExperienceTranslation { LanguageCode = "en" }] };
            _experienceRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<string>(), true, It.IsAny<CancellationToken>())).ReturnsAsync(experience);
            _experienceRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Experience>(), It.IsAny<CancellationToken>())).ReturnsAsync(experience);
            _mapperMock.Setup(x => x.Map<ExperienceViewModel>(It.IsAny<Experience>())).Returns(new ExperienceViewModel());

            var result = await _experienceApplication.UpdateTranslationAsync(Guid.NewGuid(), model, 1);

            Assert.IsTrue(result.IsT0);
        }

        [TestMethod]
        public async Task DeleteTranslationAsync_ShouldReturnExperienceNotFoundError_WhenExperienceNotFound()
        {
            _experienceRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<string>(), true, It.IsAny<CancellationToken>())).ReturnsAsync((Experience)null);

            var result = await _experienceApplication.DeleteTranslationAsync(Guid.NewGuid(), "en", 1);

            Assert.IsTrue(result.IsT1);
            Assert.AreEqual(ExperienceErrorModelHelper.ExperienceNotFoundError(), result.AsT1);
        }

        [TestMethod]
        public async Task DeleteTranslationAsync_ShouldReturnTranslationMissingError_WhenNoTranslations()
        {
            var experience = new Experience { Translations = [] };
            _experienceRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<string>(), true, It.IsAny<CancellationToken>())).ReturnsAsync(experience);

            var result = await _experienceApplication.DeleteTranslationAsync(Guid.NewGuid(), "en", 1);

            Assert.IsTrue(result.IsT1);
            Assert.AreEqual(ExperienceErrorModelHelper.TranslationMissingError(), result.AsT1);
        }

        [TestMethod]
        public async Task DeleteTranslationAsync_ShouldReturnTranslationCannotBeDeletedError_WhenOnlyOneTranslation()
        {
            var experience = new Experience { Translations = [new ExperienceTranslation()] };
            _experienceRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<string>(), true, It.IsAny<CancellationToken>())).ReturnsAsync(experience);
            _experienceRepositoryMock.Setup(x => x.CountTranslationsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var result = await _experienceApplication.DeleteTranslationAsync(Guid.NewGuid(), "en", 1);

            Assert.IsTrue(result.IsT1);
            Assert.AreEqual(ExperienceErrorModelHelper.TranslationCannotBeDeletedError(), result.AsT1);
        }

        [TestMethod]
        public async Task DeleteTranslationAsync_ShouldReturnExperienceViewModel_WhenValidModel()
        {
            var experience = new Experience { Translations = [new ExperienceTranslation { LanguageCode = "en" }] };
            _experienceRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<string>(), true, It.IsAny<CancellationToken>())).ReturnsAsync(experience);
            _experienceRepositoryMock.Setup(x => x.CountTranslationsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(2);
            _experienceRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Experience>(), It.IsAny<CancellationToken>())).ReturnsAsync(experience);
            _mapperMock.Setup(x => x.Map<ExperienceViewModel>(It.IsAny<Experience>())).Returns(new ExperienceViewModel());

            var result = await _experienceApplication.DeleteTranslationAsync(Guid.NewGuid(), "en", 1);

            Assert.IsTrue(result.IsT0);
        }

        [TestMethod]
        public async Task DeleteAsync_ShouldReturnExperienceNotFoundError_WhenExperienceNotFound()
        {
            _experienceRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<string>(), true, It.IsAny<CancellationToken>())).ReturnsAsync((Experience)null);

            var result = await _experienceApplication.DeleteAsync(Guid.NewGuid(), 1);

            Assert.IsTrue(result.IsT1);
            Assert.AreEqual(ExperienceErrorModelHelper.ExperienceNotFoundError(), result.AsT1);
        }

        [TestMethod]
        public async Task DeleteAsync_ShouldReturnExperienceViewModel_WhenValidModel()
        {
            var experience = new Experience();
            _experienceRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<string>(), true, It.IsAny<CancellationToken>())).ReturnsAsync(experience);
            _experienceRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Experience>(), It.IsAny<CancellationToken>())).ReturnsAsync(experience);
            _mapperMock.Setup(x => x.Map<ExperienceViewModel>(It.IsAny<Experience>())).Returns(new ExperienceViewModel());

            var result = await _experienceApplication.DeleteAsync(Guid.NewGuid(), 1);

            Assert.IsTrue(result.IsT0);
        }

        [TestMethod]
        public async Task GetAsync_ShouldReturnExperienceNotFoundError_WhenExperienceNotFound()
        {
            _experienceRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<string>(), true, It.IsAny<CancellationToken>())).ReturnsAsync((Experience)null);

            var result = await _experienceApplication.GetAsync(Guid.NewGuid(), "en");

            Assert.IsTrue(result.IsT1);
            Assert.AreEqual(ExperienceErrorModelHelper.ExperienceNotFoundError(), result.AsT1);
        }

        [TestMethod]
        public async Task GetAsync_ShouldReturnExperienceViewModel_WhenValidModel()
        {
            var experience = new Experience();
            _experienceRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<string>(), true, It.IsAny<CancellationToken>())).ReturnsAsync(experience);
            _mapperMock.Setup(x => x.Map<ExperienceViewModel>(It.IsAny<Experience>())).Returns(new ExperienceViewModel());

            var result = await _experienceApplication.GetAsync(Guid.NewGuid(), "en");

            Assert.IsTrue(result.IsT0);
        }

        [TestMethod]
        public async Task GetAsync_MultipleLanguageCodes_ShouldReturnExperienceNotFoundError_WhenExperienceNotFound()
        {
            _experienceRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<string[]>(), true, It.IsAny<CancellationToken>())).ReturnsAsync((Experience)null);

            var result = await _experienceApplication.GetAsync(Guid.NewGuid(), new string[] { "en", "fr" });

            Assert.IsTrue(result.IsT1);
            Assert.AreEqual(ExperienceErrorModelHelper.ExperienceNotFoundError(), result.AsT1);
        }

        [TestMethod]
        public async Task GetAsync_MultipleLanguageCodes_ShouldReturnExperienceViewModel_WhenValidModel()
        {
            var experience = new Experience();
            _experienceRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<string[]>(), true, It.IsAny<CancellationToken>())).ReturnsAsync(experience);
            _mapperMock.Setup(x => x.Map<ExperienceViewModel>(It.IsAny<Experience>())).Returns(new ExperienceViewModel());

            var result = await _experienceApplication.GetAsync(Guid.NewGuid(), new string[] { "en", "fr" });

            Assert.IsTrue(result.IsT0);
        }

        [TestMethod]
        public async Task AddPhotoAsync_ShouldReturnExperienceNotFoundError_WhenExperienceNotFound()
        {
            _experienceRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<string>(), true, It.IsAny<CancellationToken>())).ReturnsAsync((Experience)null);

            var result = await _experienceApplication.AddPhotoAsync(Guid.NewGuid(), "fileId", "imageUrl", 1);

            Assert.IsTrue(result.IsT1);
            Assert.AreEqual(ExperienceErrorModelHelper.ExperienceNotFoundError(), result.AsT1);
        }

        [TestMethod]
        public async Task AddPhotoAsync_ShouldReturnExperienceViewModel_WhenValidModel()
        {
            var experience = new Experience();
            _experienceRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<string>(), true, It.IsAny<CancellationToken>())).ReturnsAsync(experience);
            _experienceRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Experience>(), It.IsAny<CancellationToken>())).ReturnsAsync(experience);
            _mapperMock.Setup(x => x.Map<ExperienceViewModel>(It.IsAny<Experience>())).Returns(new ExperienceViewModel());

            var result = await _experienceApplication.AddPhotoAsync(Guid.NewGuid(), "fileId", "imageUrl", 1);

            Assert.IsTrue(result.IsT0);
        }

        [TestMethod]
        public async Task RemovePhotoAsync_ShouldReturnExperienceNotFoundError_WhenExperienceNotFound()
        {
            _experienceRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<string>(), true, It.IsAny<CancellationToken>())).ReturnsAsync((Experience)null);

            var result = await _experienceApplication.RemovePhotoAsync(Guid.NewGuid(), "fileId", 1);

            Assert.IsTrue(result.IsT1);
            Assert.AreEqual(ExperienceErrorModelHelper.ExperienceNotFoundError(), result.AsT1);
        }
    }
}