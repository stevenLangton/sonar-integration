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
    [TestClass]
    public class ColleagueControllerTest:RepositoryMock
    {
        ColleagueController _controller;
        private Mock<IStubServiceFacade> _mockStubServiceFacade;
        private Mock<IConfigurationDataService> _mockConfigurationDataService;
        private Mock<IDomainTranslationService> _mockDomainTranslationService;
        private IDomainTranslationService _domainTranslationService;

        [TestInitialize]
        public void ColleagueControllerTestSetup()
        {
            // Test Initialize/Setup
            _mockStubServiceFacade = new Mock<IStubServiceFacade>();
            
            _mockConfigurationDataService = new Mock<IConfigurationDataService>();

            //IConfigurationDataService configurationDataService = new ConfigurationDataService();
            //_domainTranslationService = new DomainTranslationService(_mockConfigurationDataService.Object);
            _mockDomainTranslationService = new Mock<IDomainTranslationService>();

            IColleagueService mockColleagueService = new ColleagueService(_mockStubServiceFacade.Object, _mockDomainTranslationService.Object);

            _controller = new ColleagueController(mockColleagueService);
            // Colleague controller only calls the ColleagueService.

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
            var result = _controller.GetMyDetailsByEmail("vasundhara.b@jsmocktest1.onmicrosoft.com") as OkNegotiatedContentResult<ColleagueView>;
            
            //Assert
            Assert.IsNotNull(result,"Invalid user no record found or email address is wrong");
            _mockStubServiceFacade.Verify(facade => facade.GetColleagueByEmail("vasundhara.b@domain.com"), "Colleague controller failed to call Stub Colleague service");
            _mockDomainTranslationService.Verify(service => service.AdDomainToDbDomain("vasundhara.b@jsmocktest1.onmicrosoft.com"), "Colleague controller fFailed to call domain translation method");
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
            var result = _controller.IsManagerByEmail("vasundhara.b@sainsburys.co.uk");
            Assert.IsFalse(result,"failed as he / she is Manager");

            result = _controller.IsManagerByEmail("sandip.v@sainsburys.co.uk");
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
