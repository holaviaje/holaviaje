using HolaViaje.Social.Features.Profiles;
using HolaViaje.Social.Shared;

namespace HolaViaje.Social.Tests.Features.Profiles
{
    [TestClass]
    public class UserProfileTests
    {
        [TestMethod]
        public void Constructor_ShouldInitializeControl()
        {
            var profile = new UserProfile();
            Assert.IsNotNull(profile.Control);
            Assert.AreEqual(DateTime.UtcNow.Date, profile.Control.CreatedAt.Date);
            Assert.AreEqual(DateTime.UtcNow.Date, profile.Control.LastModifiedAt.Date);
        }

        [TestMethod]
        public void Constructor_WithUserId_ShouldSetId()
        {
            long userId = 123;
            var profile = new UserProfile(userId);
            Assert.AreEqual(userId, profile.Id);
        }

        [TestMethod]
        public void IsVisibleFor_ShouldReturnTrueForOwner()
        {
            long userId = 123;
            var profile = new UserProfile(userId);
            Assert.IsTrue(profile.IsVisibleFor(userId));
        }

        [TestMethod]
        public void IsVisibleFor_ShouldReturnTrueForPublic()
        {
            var profile = new UserProfile { Visibility = Visibility.Public };
            Assert.IsTrue(profile.IsVisibleFor(123));
        }

        [TestMethod]
        public void IsOwner_ShouldReturnTrueForOwner()
        {
            long userId = 123;
            var profile = new UserProfile(userId);
            Assert.IsTrue(profile.IsOwner(userId));
        }

        [TestMethod]
        public void SetAvailability_ShouldUpdateAvailability()
        {
            var profile = new UserProfile();
            var newAvailability = new Availability(false, null);
            profile.SetAvailability(newAvailability);
            Assert.AreEqual(newAvailability, profile.Availability);
        }

        [TestMethod]
        public void UpdateLastModified_ShouldUpdateLastModifiedAt()
        {
            var profile = new UserProfile();
            var initialDate = profile.Control.LastModifiedAt;
            profile.UpdateLastModified();
            Assert.IsTrue(profile.Control.LastModifiedAt > initialDate);
        }

        [TestMethod]
        public void Delete_ShouldSetIsDeleted()
        {
            var profile = new UserProfile();
            profile.Delete();
            Assert.IsTrue(profile.Control.IsDeleted);
            Assert.IsNotNull(profile.Control.DeletedAt);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void EnsureNotDeleted_ShouldThrowIfDeleted()
        {
            var profile = new UserProfile();
            profile.Delete();
            profile.Delete();
        }

        [TestMethod]
        public void SetPlace_ShouldUpdatePlace()
        {
            var profile = new UserProfile();
            var newPlace = new PlaceInfo { Country = "USA" };
            profile.SetPlace(newPlace);
            Assert.AreEqual(newPlace, profile.Place);
        }

        [TestMethod]
        public void SetSpokenLanguages_ShouldUpdateLanguages()
        {
            var profile = new UserProfile();
            var newLanguages = new List<SpokenLanguage> { new SpokenLanguage("en", "english"), new SpokenLanguage("es", "spanish") };
            profile.SetSpokenLanguages(newLanguages);
            CollectionAssert.AreEquivalent(newLanguages, (System.Collections.ICollection)profile.SpokenLanguages);
        }

        [TestMethod]
        public void SetPicture_ShouldUpdatePicture()
        {
            var profile = new UserProfile();
            var newPicture = new ProfilePicture("filename.png", "url");
            profile.SetPicture(newPicture);
            Assert.AreEqual(newPicture, profile.Picture);
        }
    }
}
