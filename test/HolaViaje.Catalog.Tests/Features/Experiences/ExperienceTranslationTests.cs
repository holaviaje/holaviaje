using HolaViaje.Catalog.Features.Experiences;
using HolaViaje.Catalog.Shared;
using HolaViaje.Global.Shared;

namespace HolaViaje.Catalog.Tests.Features.Experiences
{
    [TestClass]
    public class ExperienceTranslationTests
    {
        [TestMethod]
        public void SetServices_ShouldAddAndRemoveServices()
        {
            // Arrange
            var experience = new Experience();
            var translation = new ExperienceTranslation(experience, "EN");
            var initialServices = new List<ExperienceService>
            {
                new ExperienceService { RecordId = Guid.NewGuid(), Title = "Service1" },
                new ExperienceService { RecordId = Guid.NewGuid(), Title = "Service2" }
            };
            translation.SetServices(initialServices);

            var newServices = new List<ExperienceService>
            {
                new ExperienceService { RecordId = initialServices[0].RecordId, Title = "Service1" },
                new ExperienceService { RecordId = Guid.NewGuid(), Title = "Service3" }
            };

            // Act
            translation.SetServices(newServices);

            // Assert
            Assert.AreEqual(2, translation.Services.Count);
            Assert.IsTrue(translation.Services.Any(s => s.Title == "Service1"));
            Assert.IsTrue(translation.Services.Any(s => s.Title == "Service3"));
            Assert.IsFalse(translation.Services.Any(s => s.Title == "Service2"));
        }

        [TestMethod]
        public void SetPickupPoints_ShouldAddAndRemovePickupPoints()
        {
            // Arrange
            var experience = new Experience();
            var translation = new ExperienceTranslation(experience, "EN");
            var initialPoints = new List<ExperienceMapPoint>
            {
                new ExperienceMapPoint { RecordId = Guid.NewGuid(), Name = "Point1" },
                new ExperienceMapPoint { RecordId = Guid.NewGuid(), Name = "Point2" }
            };
            translation.SetPickupPoints(initialPoints);

            var newPoints = new List<ExperienceMapPoint>
            {
                new ExperienceMapPoint { RecordId = initialPoints[0].RecordId, Name = "Point1" },
                new ExperienceMapPoint { RecordId = Guid.NewGuid(), Name = "Point3" }
            };

            // Act
            translation.SetPickupPoints(newPoints);

            // Assert
            Assert.AreEqual(2, translation.PickupPoints.Count);
            Assert.IsTrue(translation.PickupPoints.Any(p => p.Name == "Point1"));
            Assert.IsTrue(translation.PickupPoints.Any(p => p.Name == "Point3"));
            Assert.IsFalse(translation.PickupPoints.Any(p => p.Name == "Point2"));
        }

        [TestMethod]
        public void SetMeetingPoints_ShouldAddAndRemoveMeetingPoints()
        {
            // Arrange
            var experience = new Experience();
            var translation = new ExperienceTranslation(experience, "EN");
            var initialPoints = new List<ExperienceMapPoint>
            {
                new ExperienceMapPoint { RecordId = Guid.NewGuid(), Name = "Point1" },
                new ExperienceMapPoint { RecordId = Guid.NewGuid(), Name = "Point2" }
            };
            translation.SetMeetingPoints(initialPoints);

            var newPoints = new List<ExperienceMapPoint>
            {
                new ExperienceMapPoint { RecordId = initialPoints[0].RecordId, Name = "Point1" },
                new ExperienceMapPoint { RecordId = Guid.NewGuid(), Name = "Point3" }
            };

            // Act
            translation.SetMeetingPoints(newPoints);

            // Assert
            Assert.AreEqual(2, translation.MeetingPoints.Count);
            Assert.IsTrue(translation.MeetingPoints.Any(p => p.Name == "Point1"));
            Assert.IsTrue(translation.MeetingPoints.Any(p => p.Name == "Point3"));
            Assert.IsFalse(translation.MeetingPoints.Any(p => p.Name == "Point2"));
        }

        [TestMethod]
        public void SetTicketRedemptionPoint_ShouldUpdateTicketRedemptionPoint()
        {
            // Arrange
            var experience = new Experience();
            var translation = new ExperienceTranslation(experience, "EN");
            var point = new ExperienceMapPoint { RecordId = Guid.NewGuid(), Name = "Point1" };

            // Act
            translation.SetTicketRedemptionPoint(point);

            // Assert
            Assert.AreEqual(point, translation.TicketRedemptionPoint);
        }

        [TestMethod]
        public void SetEndPoint_ShouldUpdateEndPoint()
        {
            // Arrange
            var experience = new Experience();
            var translation = new ExperienceTranslation(experience, "EN");
            var point = new ExperienceMapPoint { RecordId = Guid.NewGuid(), Name = "Point1" };

            // Act
            translation.SetEndPoint(point);

            // Assert
            Assert.AreEqual(point, translation.EndPoint);
        }

        [TestMethod]
        public void SetStops_ShouldAddAndRemoveStops()
        {
            // Arrange
            var experience = new Experience();
            var translation = new ExperienceTranslation();
            var initialStops = new List<ExperienceStop>
            {
                new ExperienceStop { RecordId = Guid.NewGuid(), Title = "Stop1" },
                new ExperienceStop { RecordId = Guid.NewGuid(), Title = "Stop2" }
            };
            translation.SetStops(initialStops);

            var newStops = new List<ExperienceStop>
            {
                new ExperienceStop { RecordId = initialStops[0].RecordId, Title = "Stop1" },
                new ExperienceStop { RecordId = Guid.NewGuid(), Title = "Stop3" }
            };

            // Act
            translation.SetStops(newStops);

            // Assert
            Assert.AreEqual(2, translation.Stops.Count);
            Assert.IsTrue(translation.Stops.Any(s => s.Title == "Stop1"));
            Assert.IsTrue(translation.Stops.Any(s => s.Title == "Stop3"));
            Assert.IsFalse(translation.Stops.Any(s => s.Title == "Stop2"));
        }

        [TestMethod]
        public void SetAdditionalInfos_ShouldAddAndRemoveAdditionalInfos()
        {
            // Arrange
            var experience = new Experience();
            var translation = new ExperienceTranslation(experience, "EN");
            var initialInfos = new List<AdditionalInfo>
            {
                new AdditionalInfo { RecordId = Guid.NewGuid(), Description = "Info1" },
                new AdditionalInfo { RecordId = Guid.NewGuid(), Description = "Info2" }
            };
            translation.SetAdditionalInfos(initialInfos);

            var newInfos = new List<AdditionalInfo>
            {
                new AdditionalInfo { RecordId = initialInfos[0].RecordId, Description = "Info1" },
                new AdditionalInfo { RecordId = Guid.NewGuid(), Description = "Info3" }
            };

            // Act
            translation.SetAdditionalInfos(newInfos);

            // Assert
            Assert.AreEqual(2, translation.AdditionalInfos.Count);
            Assert.IsTrue(translation.AdditionalInfos.Any(i => i.Description == "Info1"));
            Assert.IsTrue(translation.AdditionalInfos.Any(i => i.Description == "Info3"));
            Assert.IsFalse(translation.AdditionalInfos.Any(i => i.Description == "Info2"));
        }

        [TestMethod]
        public void SetPlace_ShouldUpdatePlace()
        {
            // Arrange
            var experience = new Experience();
            var translation = new ExperienceTranslation(experience, "EN");
            var place = new MapPoint { Name = "Place1" };

            // Act
            translation.SetPlace(place);

            // Assert
            Assert.AreEqual(place, translation.Place);
        }
    }
}
