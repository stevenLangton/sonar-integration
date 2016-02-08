using System.Collections.Generic;
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
        /// Colleague Controller - test that it calls stub ColleagueService and Domain Translation service
        /// </summary>
        [TestMethod]
        public void GetEmployeeDetails()
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
            
            //Assert
            Assert.IsNotNull(result,"Invalid user not record found or email address is wrong");
            _mockDomainTranslationService.Verify(service => service.AdDomainToDbDomain("vasundhara.b@jsmocktest1.onmicrosoft.com"), "Colleague service failed to call domain translation method");
            _mockStubServiceFacade.Verify(facade => facade.GetColleagueByEmail("vasundhara.b@domain.com"), "Colleague service failed to call Facade method");
        }

        [TestMethod]
        public void GetMyMeetings()
        {
            //var controller = new ColleagueController();
            //var result = controller.GetMyMeetings("E001") as OkNegotiatedContentResult<ColleagueTeamView>;
            //Assert.IsNotNull(result,"failed as  no meetigns found");
        }

        [TestMethod]
        public void IsManager()
        {
            //var controller = new ColleagueController(Repository);
            var result = _service.IsManagerByEmail("vasundhara.b@sainsburys.co.uk");
            Assert.IsFalse(result,"failed as he / she is Manager");

            result = _service.IsManagerByEmail("sandip.v@sainsburys.co.uk");
            Assert.IsTrue(result,"failed as email is wrong or not a manager");
        }

        [TestMethod]
        public void GetMyTeam()
        {
            //var controller = new ColleagueController(Repository);
            //var result = controller.GetMyTeam("E0010") as OkNegotiatedContentResult<IList<ColleagueTeamView>>;
            //Assert.IsNotNull(result, "No team members found");
        }
    }
}
