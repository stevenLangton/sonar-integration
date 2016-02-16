using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Http.Dispatcher;
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
    public class StubServiceFacadeTest
    {
        private StubServiceFacade _serviceFacade;
        private Mock<IConfigurationDataService> _mockConfigurationDataService;

        [TestInitialize]
        public void StubServiceFacadeTestSetup()
        {
            // Test Initialize/Setup
            _mockConfigurationDataService = new Mock<IConfigurationDataService>();

            _mockConfigurationDataService.Setup(cds => cds.GetConfigSettingValue("ServicesBaseUrl"))
                .Returns("http://stubserviceUrl");
        }

        [TestMethod]
        public void StubServiceFacadeCtorTest()
        {
            // Arrange
            var cdsMethod = _mockConfigurationDataService.Setup(cds => cds.GetConfigSettingValue("ServicesBaseUrl"))
                .Returns("http://stubserviceUrl");
            cdsMethod.Verifiable();

            var httpClient = new Lazy<HttpClient>(() => new HttpClient(new UnitTestRoutedHttpMsgHandler("")));

            // Act
            _serviceFacade = new StubServiceFacade(httpClient, _mockConfigurationDataService.Object);

            // Assert
            Assert.IsNotNull(_serviceFacade, "Unable to construct Stub facade");
            _mockConfigurationDataService.Verify(cds => cds.GetConfigSettingValue("ServicesBaseUrl"), "Stub service failed to call config service to get Svc Url");
        }

        /// <summary>
        /// GetColleague - test that it calls the correct route and gets the right value
        /// </summary>
        [TestMethod]
        public void StubServiceFacadeGetColleagueTest()
        {
            // Arrange
            var httpClient = new Lazy<HttpClient>(() => new HttpClient(new UnitTestRoutedHttpMsgHandler("api/Colleague")));
            _serviceFacade = new StubServiceFacade(httpClient, _mockConfigurationDataService.Object);

            // Act
            var result = _serviceFacade.GetColleague("E001");

            //Assert
            Assert.IsNotNull(result, "Colleague record not found");
            Assert.AreEqual(result.FirstName, "Stub Colleague");
        }

        /// <summary>
        /// GetColleagueByEmail - test that it calls the correct route and gets the right value
        /// </summary>
        [TestMethod]
        public void StubServiceFacadeGetColleagueByEmailTest()
        {
            // Arrange
            var httpClient = new Lazy<HttpClient>(() => new HttpClient(new UnitTestRoutedHttpMsgHandler("api/Colleague")));
            _serviceFacade = new StubServiceFacade(httpClient, _mockConfigurationDataService.Object);

            // Act
            var result = _serviceFacade.GetColleagueByEmail("email@sainsburys.co.uk");

            //Assert
            Assert.IsNotNull(result, "Colleague record not found");
            Assert.AreEqual(result.EmailAddress, "email@sainsburys.co.uk");
        }

        /// <summary>
        /// GetDirectReports - test that it calls the correct route and gets the right value
        /// </summary>
        [TestMethod]
        public void StubServiceFacadeGetDirectReportsTest()
        {
            // Arrange
            var httpClient = new Lazy<HttpClient>(() => new HttpClient(new UnitTestRoutedHttpMsgHandler("api/directreports")));
            _serviceFacade = new StubServiceFacade(httpClient, _mockConfigurationDataService.Object);

            // Act
            var result = _serviceFacade.GetDirectReports("e0010");

            //Assert
            Assert.IsNotNull(result, "Direct reports not found, expected 2");
            Assert.AreEqual(result.Count(), 2);
        }

        /// <summary>
        /// GetDirectReportsByManagerEmail - test that it calls the correct route and gets the right value
        /// </summary>
        [TestMethod]
        public void StubServiceFacadeGetDirectReportsByEmailTest()
        {
            // Arrange
            var httpClient = new Lazy<HttpClient>(() => new HttpClient(new UnitTestRoutedHttpMsgHandler("api/directreportsbyemail")));
            _serviceFacade = new StubServiceFacade(httpClient, _mockConfigurationDataService.Object);

            // Act
            var result = _serviceFacade.GetDirectReportsByManagerEmail("manager@sainsburys.co.uk");

            //Assert
            Assert.IsNotNull(result, "Direct reports not found, expected 2");
            Assert.AreEqual(result.Count(), 2);
        }
    }
    internal class UnitTestRoutedHttpMsgHandler : HttpMessageHandler
    {
        private string _apiRoute = "";

        public UnitTestRoutedHttpMsgHandler(string apiRoute)
        {
            _apiRoute = apiRoute;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Cases for all StubServiceFacade routes (NOTE: Lowercase strings)
            switch (request.RequestUri.PathAndQuery.ToLower())
            {
                case "/api/colleague/e001":
                    {
                        string c = Json.Encode(GetStubColleague());
                        return Task.FromResult(GetJsonResponseMessage(c));
                    }
                case "/api/colleaguebyemail/email@sainsburys.co.uk":
                    {
                        string c = Json.Encode(GetStubColleague());
                        return Task.FromResult(GetJsonResponseMessage(c));
                    }
                case "/api/directreports/e0010":
                    {
                        string cMany = Json.Encode(GetStubDirectReports());
                        return Task.FromResult(GetJsonResponseMessage(cMany));
                    }
                case "/api/directreportsbyemail/manager@sainsburys.co.uk":
                    {
                        string cMany = Json.Encode(GetStubDirectReports());
                        return Task.FromResult(GetJsonResponseMessage(cMany));
                    }
            }
            return null;
        }

        private ColleagueView GetStubColleague()
        {
            return new ColleagueView { FirstName = "Stub Colleague", EmailAddress = "email@sainsburys.co.uk" };
        }

        private List<ColleagueView> GetStubDirectReports()
        {
            return new List<ColleagueView>
            {
                new ColleagueView {FirstName = "Stub Colleague1", EmailAddress = "email1@sainsburys.co.uk"},
                new ColleagueView {FirstName = "Stub Colleague2", EmailAddress = "email2@sainsburys.co.uk"}
            };
        }

        private HttpResponseMessage GetJsonResponseMessage(string jsonContent)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json"),
            };
            return response;
        }
    }
}
