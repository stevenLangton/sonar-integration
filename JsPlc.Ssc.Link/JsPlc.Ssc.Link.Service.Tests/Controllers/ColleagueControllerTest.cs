using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Results;
using JsPlc.Ssc.Link.Interfaces.Services;
using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Service.Controllers;
using JsPlc.Ssc.Link.Service.Services;
using JsPlc.Ssc.Link.StubService.Areas.HelpPage.ModelDescriptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JsPlc.Ssc.Link.Models.Entities;
using Moq;

namespace JsPlc.Ssc.Link.Service.Tests.Controllers
{
    // The Controller doesnt interact with repository directly. Controller depends on Service methods. Thats what we mock here. 
    [TestClass]
    public class ColleagueControllerTest
    {
        ColleagueController _controller;
        private Mock<IColleagueService> _mockColleagueService;

        [TestInitialize]
        public void ColleagueControllerTestSetup()
        {
            // Test Initialize/Setup
            _mockColleagueService = new Mock<IColleagueService>();

            // Colleague controller only calls the ColleagueService.
            _controller = new ColleagueController(_mockColleagueService.Object);
        }

        /// <summary>
        /// Colleague Controller - test that it calls correct ColleagueService method
        /// </summary>
        [TestMethod]
        public void GetEmployeeDetailsByEmail()
        {
            var stubServiceMethodSetup = _mockColleagueService.Setup(service => service
                .GetColleagueByEmail("vasundhara.b@jsmocktest1.onmicrosoft.com"))
                .Returns(new ColleagueView(){ FirstName = "StubColleague"});
            stubServiceMethodSetup.Verifiable();

            // Act
            var result = _controller.GetMyDetailsByEmail("vasundhara.b@jsmocktest1.onmicrosoft.com") as OkNegotiatedContentResult<ColleagueView>;
            var result1 = _controller.GetMyDetailsByEmail("nosuchuser@jsmocktest1.onmicrosoft.com") as NotFoundResult;
            
            //Assert
            Assert.IsNotNull(result,"Invalid user no record found or email address is wrong");
            Assert.IsTrue(result.Content.FirstName.Equals("StubColleague"), "Controller didnt call correct service");
            _mockColleagueService.Verify(facade => facade.GetColleagueByEmail("vasundhara.b@jsmocktest1.onmicrosoft.com"), "Colleague controller failed to call Stub Colleague service");

            // Assert Not found path - NotFound result should be NonNull
            Assert.IsNotNull(result1, "Found a user which should not have been found");
            _mockColleagueService.Verify(facade => facade.GetColleagueByEmail("vasundhara.b@jsmocktest1.onmicrosoft.com"), "Colleague controller failed to call Stub Colleague service");
        }

        /// <summary>
        /// Colleague Controller - test that it calls correct ColleagueService method
        /// </summary>
        [TestMethod]
        public void GetEmployeeDetailsById()
        {
            var stubServiceMethodSetup = _mockColleagueService.Setup(service => service
                .GetColleague("E001"))
                .Returns(new ColleagueView() { FirstName = "StubColleague" });
            stubServiceMethodSetup.Verifiable();

            // Act
            var result = _controller.GetMyDetailsById("E001") as OkNegotiatedContentResult<ColleagueView>;
            var result1 = _controller.GetMyDetailsById("E002") as NotFoundResult;

            //Assert
            Assert.IsNotNull(result, "Invalid user no record found or email address is wrong");
            Assert.IsTrue(result.Content.FirstName.Equals("StubColleague"), "Controller didnt call correct service");
            _mockColleagueService.Verify(facade => facade.GetColleague("E001"), "Colleague controller failed to call Stub Colleague service");

            // Assert Not found path - NotFound result should be NonNull
            Assert.IsNotNull(result1, "Found a user which should not have been found");
            _mockColleagueService.Verify(facade => facade.GetColleague("E001"), "Colleague controller failed to call Stub Colleague service");
        }

        [TestMethod]
        public void IsManagerById()
        {
            var stubServiceMethodSetup = _mockColleagueService.Setup(service => service
                .IsManager("E001"))
                .Returns(true);
            stubServiceMethodSetup.Verifiable();

            // Act
            var result = _controller.IsManagerById("E001");
            var result1 = _controller.IsManagerById("E002");

            //Assert
            Assert.IsTrue(result, "Controller didnt call correct service");
            _mockColleagueService.Verify(facade => facade.IsManager("E001"), "Colleague controller failed to call Stub Colleague service");

            // Assert Not found path - NotFound result should be NonNull
            Assert.IsFalse(result1, "Found a user which should not have been found");
            _mockColleagueService.Verify(facade => facade.IsManager("E001"), "Colleague controller failed to call Stub Colleague service");
        }

        [TestMethod]
        public void IsManagerByEmail()
        {
            var stubServiceMethodSetup = _mockColleagueService.Setup(service => service
                .IsManagerByEmail("manager@domain.com"))
                .Returns(true);
            stubServiceMethodSetup.Verifiable();

            // Act
            var result = _controller.IsManagerByEmail("manager@domain.com");
            var result1 = _controller.IsManagerByEmail("nosuchmanager@domain.com");

            //Assert
            Assert.IsTrue(result, "Controller didnt call correct service");
            _mockColleagueService.Verify(facade => facade.IsManagerByEmail("manager@domain.com"), "Colleague controller failed to call Stub Colleague service");

            // Assert Not found path - NotFound result should be NonNull
            Assert.IsFalse(result1, "Found a user which should not have been found");
            _mockColleagueService.Verify(facade => facade.IsManagerByEmail("manager@domain.com"), "Colleague controller failed to call Stub Colleague service");
        }

        [TestMethod]
        public void GetDirectReports()
        {
            var stubServiceMethodSetup = _mockColleagueService.Setup(service => service
                .GetDirectReports("E001"))
                .Returns(new List<ColleagueView>()
                {
                    new ColleagueView {FirstName = "StubColleague1" },
                    new ColleagueView {FirstName = "StubColleague2" },
                });
            stubServiceMethodSetup.Verifiable();

            // Act
            var result = _controller.GetDirectReports("E001") as OkNegotiatedContentResult<List<ColleagueView>>;
            var noReportsResult = _controller.GetDirectReports("E002") as OkNegotiatedContentResult<List<ColleagueView>>;

            //Assert
            Assert.IsNotNull(result, "Invalid user no record found or email address is wrong");
            Assert.IsTrue(result.Content.Count == 2, "Controller didnt call correct service");
            _mockColleagueService.Verify(facade => facade.GetDirectReports("E001"), "Colleague controller failed to call Stub Colleague service");

            // Assert Not found path - NotFound result should be NonNull
            Assert.IsNotNull(noReportsResult, "Error - We expected a Non Null result - OK Result, got null");
            Assert.IsNull(noReportsResult.Content, "Error - Found a direct reports for manager which should not have been found");
            _mockColleagueService.Verify(facade => facade.GetDirectReports("E001"), "Colleague controller failed to call Stub Colleague service");
        }

        [TestMethod]
        public void GetDirectReportsByEmail()
        {
            var stubServiceMethodSetup = _mockColleagueService.Setup(service => service
                .GetDirectReportsByManagerEmail("vasu.b@domain.com"))
                .Returns(new List<ColleagueView>()
                {
                    new ColleagueView {FirstName = "StubColleague1" },
                    new ColleagueView {FirstName = "StubColleague2" },
                });
            stubServiceMethodSetup.Verifiable();

            // Act
            var result = _controller.GetDirectReportsByEmail("vasu.b@domain.com") as OkNegotiatedContentResult<List<ColleagueView>>;
            var noReportsResult = _controller.GetDirectReportsByEmail("notauser@domain.com") as OkNegotiatedContentResult<List<ColleagueView>>;

            //Assert
            Assert.IsNotNull(result, "Invalid user no record found or email address is wrong");
            Assert.IsTrue(result.Content.Count == 2, "Controller didnt call correct service");
            _mockColleagueService.Verify(facade => facade.GetDirectReportsByManagerEmail("vasu.b@domain.com"), "Colleague controller failed to call Stub Colleague service");

            // Assert Not found path - NotFound result should be NonNull
            Assert.IsNotNull(noReportsResult, "Error - We expected a Non Null result - OK Result, got null");
            Assert.IsNull(noReportsResult.Content, "Error - Found a direct reports for manager which should not have been found");
            _mockColleagueService.Verify(facade => facade.GetDirectReportsByManagerEmail("vasu.b@domain.com"), "Colleague controller failed to call Stub Colleague service");
        }

    }
}
