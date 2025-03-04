using AutoMapper;
using HolaViaje.Catalog.Features.Companies;
using HolaViaje.Catalog.Features.Companies.Models;
using HolaViaje.Catalog.Features.Companies.Repository;
using HolaViaje.Catalog.Shared.Models;
using Moq;

namespace HolaViaje.Catalog.Tests.Features.Companies
{
    [TestClass]
    public class CompanyApplicationTests
    {
        private Mock<ICompanyRepository> _companyRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private ICompanyApplication _companyApplication;

        [TestInitialize]
        public void Setup()
        {
            _companyRepositoryMock = new Mock<ICompanyRepository>();
            _mapperMock = new Mock<IMapper>();
            _companyApplication = new CompanyApplication(_companyRepositoryMock.Object, _mapperMock.Object);
        }

        [TestMethod]
        public async Task CreateAsync_ShouldReturnCompanyViewModel()
        {
            // Arrange
            var companyModel = new CompanyModel { Name = "Test Company" };
            var company = new Company(1);
            var companyViewModel = new CompanyViewModel { Name = "Test Company" };

            _companyRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<Company>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(company);
            _mapperMock.Setup(mapper => mapper.Map<CompanyViewModel>(It.IsAny<Company>()))
                .Returns(companyViewModel);

            // Act
            var result = await _companyApplication.CreateAsync(companyModel, 1, CancellationToken.None);

            // Assert
            Assert.IsTrue(result.IsT0);
            Assert.AreEqual(companyViewModel, result.AsT0);
        }

        [TestMethod]
        public async Task UpdateAsync_ShouldReturnCompanyViewModel()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var companyModel = new CompanyModel { Name = "Updated Company" };
            var company = new Company(1);
            var companyViewModel = new CompanyViewModel { Name = "Updated Company" };

            _companyRepositoryMock.Setup(repo => repo.GetAsync(companyId, true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(company);
            _companyRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Company>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(company);
            _mapperMock.Setup(mapper => mapper.Map<CompanyViewModel>(It.IsAny<Company>()))
                .Returns(companyViewModel);

            // Act
            var result = await _companyApplication.UpdateAsync(companyId, companyModel, 1, CancellationToken.None);

            // Assert
            Assert.IsTrue(result.IsT0);
            Assert.AreEqual(companyViewModel, result.AsT0);
        }

        [TestMethod]
        public async Task DeleteAsync_ShouldReturnCompanyViewModel()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var company = new Company(1);
            var companyViewModel = new CompanyViewModel { Name = "Deleted Company" };

            _companyRepositoryMock.Setup(repo => repo.GetAsync(companyId, true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(company);
            _companyRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Company>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(company);
            _mapperMock.Setup(mapper => mapper.Map<CompanyViewModel>(It.IsAny<Company>()))
                .Returns(companyViewModel);

            // Act
            var result = await _companyApplication.DeleteAsync(companyId, 1, CancellationToken.None);

            // Assert
            Assert.IsTrue(result.IsT0);
            Assert.AreEqual(companyViewModel, result.AsT0);
        }

        [TestMethod]
        public async Task UpdateBookInfoAsync_ShouldReturnCompanyViewModel()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var bookInfoModel = new BookInfoModel("911", "911", null);
            var company = new Company(1);
            var companyViewModel = new CompanyViewModel { Name = "Company with Book Info" };

            _companyRepositoryMock.Setup(repo => repo.GetAsync(companyId, true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(company);
            _companyRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Company>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(company);
            _mapperMock.Setup(mapper => mapper.Map<CompanyViewModel>(It.IsAny<Company>()))
                .Returns(companyViewModel);

            // Act
            var result = await _companyApplication.UpdateBookInfoAsync(companyId, bookInfoModel, 1, CancellationToken.None);

            // Assert
            Assert.IsTrue(result.IsT0);
            Assert.AreEqual(companyViewModel, result.AsT0);
        }

        [TestMethod]
        public async Task UpdateManagersAsync_ShouldReturnCompanyViewModel()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var managerModels = new List<ManagerModel> { new ManagerModel(1) };
            var company = new Company(1);
            var companyViewModel = new CompanyViewModel { Name = "Company with Managers" };

            _companyRepositoryMock.Setup(repo => repo.GetAsync(companyId, true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(company);
            _companyRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Company>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(company);
            _mapperMock.Setup(mapper => mapper.Map<CompanyViewModel>(It.IsAny<Company>()))
                .Returns(companyViewModel);

            // Act
            var result = await _companyApplication.UpdateManagersAsync(companyId, managerModels, 1, CancellationToken.None);

            // Assert
            Assert.IsTrue(result.IsT0);
            Assert.AreEqual(companyViewModel, result.AsT0);
        }

        [TestMethod]
        public async Task GetAsync_ShouldReturnCompanyViewModel()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var company = new Company(1);
            var companyViewModel = new CompanyViewModel { Name = "Test Company" };

            _companyRepositoryMock.Setup(repo => repo.GetAsync(companyId, true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(company);
            _mapperMock.Setup(mapper => mapper.Map<CompanyViewModel>(It.IsAny<Company>()))
                .Returns(companyViewModel);

            // Act
            var result = await _companyApplication.GetAsync(companyId, CancellationToken.None);

            // Assert
            Assert.IsTrue(result.IsT0);
            Assert.AreEqual(companyViewModel, result.AsT0);
        }
    }
}
