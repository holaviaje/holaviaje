using HolaViaje.Catalog.Features.Experiences;
using HolaViaje.Catalog.Shared;
using HolaViaje.Global.Helpers;

namespace HolaViaje.Catalog.Tests.Features.Experiences
{
    [TestClass]
    public class ExperienceTests
    {
        [TestMethod]
        public void SetCancellationPolicy_ShouldUpdateCancellationPolicy()
        {
            // Arrange
            var experience = new Experience();
            var newPolicy = new CancellationPolicy();

            // Act
            experience.SetCancellationPolicy(newPolicy);

            // Assert
            Assert.AreEqual(newPolicy, experience.CancellationPolicy);
        }

        [TestMethod]
        public void SetDuration_ShouldUpdateDuration()
        {
            // Arrange
            var experience = new Experience();
            var newDuration = new Duration { Days = 1, Hours = 2, Minutes = 30 };

            // Act
            experience.SetDuration(newDuration);

            // Assert
            Assert.AreEqual(newDuration, experience.Duration);
        }

        [TestMethod]
        public void AddTranslation_ShouldAddTranslation()
        {
            // Arrange
            var experience = new Experience();
            var translation = new ExperienceTranslation(experience, LanguageCodeHelper.DefaultLanguageCode);

            // Act
            experience.AddTranslation(translation);

            // Assert
            Assert.IsTrue(experience.Translations.Contains(translation));
        }

        [TestMethod]
        public void UpdateLastModified_ShouldUpdateLastModifiedAt()
        {
            // Arrange
            var experience = new Experience();
            var initialTime = experience.Control.LastModifiedAt;

            // Act
            experience.UpdateLastModified();

            // Assert
            Assert.IsTrue(experience.Control.LastModifiedAt > initialTime);
        }

        [TestMethod]
        public void Delete_ShouldMarkAsDeleted()
        {
            // Arrange
            var experience = new Experience();

            // Act
            experience.Delete();

            // Assert
            Assert.IsTrue(experience.IsDeleted());
        }

        [TestMethod]
        public void IsDeleted_ShouldReturnTrueIfDeleted()
        {
            // Arrange
            var experience = new Experience();
            experience.Delete();

            // Act
            var result = experience.IsDeleted();

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void RemoveTranslation_ShouldRemoveTranslation()
        {
            // Arrange
            var experience = new Experience();
            var translation = new ExperienceTranslation();
            experience.AddTranslation(translation);

            // Act
            experience.RemoveTranslation(translation);

            // Assert
            Assert.IsFalse(experience.Translations.Contains(translation));
        }

        [TestMethod]
        public void AddPhoto_ShouldAddPhoto()
        {
            // Arrange
            var experience = new Experience();
            var photo = new Photo();

            // Act
            experience.AddPhoto(photo);

            // Assert
            Assert.IsTrue(experience.Photos.Contains(photo));
        }

        [TestMethod]
        public void RemovePhoto_ShouldRemovePhoto()
        {
            // Arrange
            var experience = new Experience();
            var photo = new Photo { FileId = "testFileId" };
            experience.AddPhoto(photo);

            // Act
            experience.RemovePhoto("testFileId");

            // Assert
            Assert.IsFalse(experience.Photos.Contains(photo));
        }
    }
}
