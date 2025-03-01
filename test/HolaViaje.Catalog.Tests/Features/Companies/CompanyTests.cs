using HolaViaje.Catalog.Features.Companies;
using HolaViaje.Catalog.Shared;

namespace HolaViaje.Catalog.Tests.Features.Companies
{
    [TestClass]
    public class CompanyTests
    {
        [TestMethod]
        public void IsOwner_ShouldReturnTrue_WhenUserIdMatches()
        {
            // Arrange
            var company = new Company(1);

            // Act
            var result = company.IsOwner(1);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsOwner_ShouldReturnFalse_WhenUserIdDoesNotMatch()
        {
            // Arrange
            var company = new Company(1);

            // Act
            var result = company.IsOwner(2);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsAdmin_ShouldReturnTrue_WhenUserIsOwner()
        {
            // Arrange
            var company = new Company(1);

            // Act
            var result = company.IsAdmin(1);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsAdmin_ShouldReturnTrue_WhenUserIsManagerWithManageAll()
        {
            // Arrange
            var company = new Company(1);
            company.Managers.Add(new Manager(2) { ManageAll = true });

            // Act
            var result = company.IsAdmin(2);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsAdmin_ShouldReturnFalse_WhenUserIsNotOwnerOrManager()
        {
            // Arrange
            var company = new Company(1);

            // Act
            var result = company.IsAdmin(2);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void UpdateLastModified_ShouldUpdateLastModifiedAt()
        {
            // Arrange
            var company = new Company(1);
            var initialDate = company.Control.LastModifiedAt;

            // Act
            company.UpdateLastModified();

            // Assert
            Assert.IsTrue(company.Control.LastModifiedAt > initialDate);
        }

        [TestMethod]
        public void Delete_ShouldSetIsDeletedToTrue()
        {
            // Arrange
            var company = new Company(1);

            // Act
            company.Delete();

            // Assert
            Assert.IsTrue(company.IsDeleted());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void EnsureNotDeleted_ShouldThrowException_WhenCompanyIsDeleted()
        {
            // Arrange
            var company = new Company(1);
            company.Delete();

            // Act
            company.UpdateLastModified();
        }

        [TestMethod]
        public void SetBookInfo_ShouldUpdateBookInfoAndLastModified()
        {
            // Arrange
            var company = new Company(1);
            var bookInfo = new BookInfo("911", "911", null);
            var initialDate = company.Control.LastModifiedAt;

            // Act
            company.SetBookInfo(bookInfo);

            // Assert
            Assert.AreEqual(bookInfo, company.BookInfo);
            Assert.IsTrue(company.Control.LastModifiedAt > initialDate);
        }

        [TestMethod]
        public void SetManagers_ShouldUpdateManagersAndLastModified()
        {
            // Arrange
            var company = new Company(1);
            var managers = new List<Manager> { new Manager(2) { ManageAll = true } };
            var initialDate = company.Control.LastModifiedAt;

            // Act
            company.SetManagers(managers);

            // Assert
            Assert.AreEqual(1, company.Managers.Count);
            Assert.AreEqual(2, company.Managers.First().UserId);
            Assert.IsTrue(company.Control.LastModifiedAt > initialDate);
        }
    }
}
