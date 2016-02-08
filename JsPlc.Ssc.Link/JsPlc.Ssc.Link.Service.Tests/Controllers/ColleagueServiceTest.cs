using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Results;
using JsPlc.Ssc.Link.Interfaces.Services;
using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Service.Controllers;
using JsPlc.Ssc.Link.Service.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JsPlc.Ssc.Link.Models.Entities;
using Moq;

namespace JsPlc.Ssc.Link.Service.Tests.Controllers
{
    // Test that the service calls the Facade method..
    [TestClass]
    public class ColleagueServiceTest : RepositoryMock
    {
        IColleagueService _service;
        private Mock<IStubServiceFacade> _mockStubServiceFacade;
        private Mock<IConfigurationDataService> _mockConfigurationDataService;
        private Mock<IDomainTranslationService> _mockDomainTranslationService;

        [TestInitialize]
        public void ColleagueServiceTestSetup()
        {
            // Test Initialize/Setup
            _mockStubServiceFacade = new Mock<IStubServiceFacade>();

            _mockConfigurationDataService = new Mock<IConfigurationDataService>();

            _mockDomainTranslationService = new Mock<IDomainTranslationService>();

            _service = new ColleagueService(_mockStubServiceFacade.Object, _mockDomainTranslationService.Object);
        }

        /// <summary>
        /// Colleague Service - test that it calls facade ColleagueService and Domain Translation service
        /// </summary>
        [TestMethod]
        public void GetEmployeeDetailsByEmail()
        {
            // Arrange
            var mockDomainXlationMethod =
                _mockDomainTranslationService.Setup(
                    service => service.AdDomainToDbDomain("vasundhara.b@jsmocktest1.onmicrosoft.com"))
                    .Returns("vasundhara.b@domain.com");
            mockDomainXlationMethod.Verifiable();

            var stubServiceMethodSetup = _mockStubServiceFacade.Setup(facade => facade.GetColleagueByEmail("vasundhara.b@domain.com"))
                .Returns(new ColleagueView());
            stubServiceMethodSetup.Verifiable();

            // Act
            var result = _service.GetColleagueByEmail("vasundhara.b@jsmocktest1.onmicrosoft.com");
            var notFoundResult = _service.GetColleagueByEmail("nosuchUser@jsmocktest1.onmicrosoft.com");

            //Assert
            Assert.IsNotNull(result, "Invalid user not record found or email address is wrong");
            _mockDomainTranslationService.Verify(service => service.AdDomainToDbDomain("vasundhara.b@jsmocktest1.onmicrosoft.com"), "Colleague service failed to call domain translation method");
            _mockStubServiceFacade.Verify(facade => facade.GetColleagueByEmail("vasundhara.b@domain.com"), "Colleague service failed to call Facade method");

            Assert.IsNull(notFoundResult, "User not expected, but found");
        }

        [TestMethod]
        public void GetEmployeeDetailsById()
        {
            // Arrange
            var stubServiceMethodSetup = _mockStubServiceFacade.Setup(facade => facade.GetColleague("E001"))
                .Returns(new ColleagueView());
            stubServiceMethodSetup.Verifiable();

            // Act
            var result = _service.GetColleague("E001");
            var notFoundResult = _service.GetColleague("E002");

            //Assert
            Assert.IsNotNull(result, "Invalid user not record found or email address is wrong");
            _mockStubServiceFacade.Verify(facade => facade.GetColleague("E001"), "Colleague service failed to call Facade method");

            Assert.IsNull(notFoundResult, "User not expected, but found");
        }

        [TestMethod]
        public void GetDirectReports()
        {
            // Arrange
            var stubServiceMethodSetup = _mockStubServiceFacade
                .Setup(facade => facade.GetDirectReports("E001"))
                .Returns(new List<ColleagueView>
                {
                    new ColleagueView(),
                    new ColleagueView()
                });
            stubServiceMethodSetup.Verifiable();

            // Act
            var result = _service.GetDirectReports("E001");
            var notFoundResult = _service.GetDirectReports("E002");

            //Assert
            Assert.IsNotNull(result, "Expected a list, got null");
            Assert.IsTrue(result.Count == 2, "Expected 2 direct reports, found " + result.Count);
            _mockStubServiceFacade.Verify(facade => facade.GetDirectReports("E001"), "Colleague service failed to call Facade method");

            Assert.IsNotNull(notFoundResult, "Expected empty list of reports, got null");
            Assert.IsTrue(notFoundResult.Count == 0, "Expected no reports, found some in error");
        }

        [TestMethod]
        public void GetDirectReportsByEmail()
        {
            // Arrange
            var mockDomainXlationMethod = _mockDomainTranslationService
                .Setup(service => service.AdDomainToDbDomain("vasundhara.b@jsmocktest1.onmicrosoft.com"))
                .Returns("vasundhara.b@domain.com");

            mockDomainXlationMethod.Verifiable();

            var stubServiceMethodSetup = _mockStubServiceFacade
                .Setup(facade => facade.GetDirectReportsByManagerEmail("vasundhara.b@domain.com"))
                .Returns(new List<ColleagueView>
                {
                    new ColleagueView(),
                    new ColleagueView()
                });
            stubServiceMethodSetup.Verifiable();

            // Act
            var result = _service.GetDirectReportsByManagerEmail("vasundhara.b@jsmocktest1.onmicrosoft.com");
            var notFoundResult = _service.GetDirectReportsByManagerEmail("noSuchUser@domain.com");

            //Assert
            Assert.IsNotNull(result, "Expected a list, got null");
            Assert.IsTrue(result.Count == 2, "Expected 2 direct reports, found " + result.Count);
            _mockStubServiceFacade.Verify(facade => facade.GetDirectReportsByManagerEmail("vasundhara.b@domain.com"), "Colleague service failed to call Facade method");

            Assert.IsNotNull(notFoundResult, "Expected empty list of reports, got null");
            Assert.IsTrue(notFoundResult.Count == 0, "Expected no reports, found some in error");
        }

        [TestMethod]
        public void IsManager()
        {
            // Arrange
            var stubServiceMethodSetup = _mockStubServiceFacade
                .Setup(facade => facade.GetDirectReports("E001"))
                .Returns(new List<ColleagueView>
                {
                    new ColleagueView(),
                    new ColleagueView()
                });
            stubServiceMethodSetup.Verifiable();

            // Act
            var result = _service.IsManager("E001");
            var notFoundResult = _service.IsManager("E002");

            //Assert
            Assert.IsTrue(result, "Expected to be manager since 2 direct reports, not found direct reports..");
            _mockStubServiceFacade.Verify(facade => facade.GetDirectReports("E001"), "Colleague service failed to call Facade method");

            Assert.IsFalse(notFoundResult, "Expected not manager, found direct reports hence ismanager..");
        }

        [TestMethod]
        public void IsManagerByEmail()
        {
            // Arrange
            var mockDomainXlationMethod = _mockDomainTranslationService
                .Setup(service => service.AdDomainToDbDomain("vasundhara.b@jsmocktest1.onmicrosoft.com"))
                .Returns("vasundhara.b@domain.com");

            mockDomainXlationMethod.Verifiable();

            var stubServiceMethodSetup = _mockStubServiceFacade
                .Setup(facade => facade.GetDirectReportsByManagerEmail("vasundhara.b@domain.com"))
                .Returns(new List<ColleagueView>
                {
                    new ColleagueView(),
                    new ColleagueView()
                });
            stubServiceMethodSetup.Verifiable();

            // Act
            var result = _service.IsManagerByEmail("vasundhara.b@jsmocktest1.onmicrosoft.com");
            var notFoundResult = _service.IsManagerByEmail("noSuchUser@jsmocktest1.onmicrosoft.com");

            //Assert
            Assert.IsTrue(result, "Expected to be manager since 2 direct reports, not found direct reports..");
            _mockStubServiceFacade.Verify(facade => facade.GetDirectReportsByManagerEmail("vasundhara.b@domain.com"), "Colleague service failed to call Facade method");

            Assert.IsFalse(notFoundResult, "Expected not manager, found direct reports hence ismanager..");
        }

    }
}
