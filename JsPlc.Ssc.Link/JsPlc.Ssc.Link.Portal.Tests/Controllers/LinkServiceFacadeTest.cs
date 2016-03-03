using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Helpers;
using JsPlc.Ssc.Link.Interfaces.Services;
using JsPlc.Ssc.Link.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JsPlc.Ssc.Link.Models.Entities;
using Moq;

namespace JsPlc.Ssc.Link.Portal.Tests.Controllers
{
    // Test that the Service Facade calls the HttpClient method..
    [TestClass]
    public class LinkServiceFacadeTest
    {
        private LinkServiceFacade _serviceFacade;
        private Mock<IConfigurationDataService> _mockConfigurationDataService;

        [TestInitialize]
        public void LinkServiceFacadeTestSetup()
        {
            // Test Initialize/Setup
            _mockConfigurationDataService = new Mock<IConfigurationDataService>();

            _mockConfigurationDataService.Setup(cds => cds.GetConfigSettingValue("ServicesBaseUrl"))
                .Returns("http://LinkServiceUrl");
        }

        [TestMethod]
        public void LinkServiceFacadeCtorTest()
        {
            // Arrange
            var cdsMethod = _mockConfigurationDataService.Setup(cds => cds.GetConfigSettingValue("ServicesBaseUrl"))
                .Returns("http://LinkServiceUrl");
            cdsMethod.Verifiable();

            var httpClient = new Lazy<HttpClient>(() => new HttpClient(new UnitTestRoutedHttpMsgHandler("")));

            // Act
            _serviceFacade = new LinkServiceFacade(httpClient, _mockConfigurationDataService.Object);

            // Assert
            Assert.IsNotNull(_serviceFacade, "Unable to construct Link Service facade");
            _mockConfigurationDataService.Verify(cds => cds.GetConfigSettingValue("ServicesBaseUrl"), "Link service facade failed to call config service to get Svc Url");
        }

        /// <summary>
        /// GetColleague - test that it calls the correct route and gets the right value
        /// </summary>
        [TestMethod]
        public void LinkServiceFacadeGetColleagueTest()
        {
            // Arrange
            var httpClient = new Lazy<HttpClient>(() => new HttpClient(new UnitTestRoutedHttpMsgHandler("api/Colleague")));
            _serviceFacade = new LinkServiceFacade(httpClient, _mockConfigurationDataService.Object);

            // Act
            var result = _serviceFacade.GetColleague("E001");

            //Assert
            Assert.IsNotNull(result, "Colleague record not found");
            Assert.AreEqual(result.FirstName, "Stub Colleague");
        }

        /// <summary>
        /// GetColleagueByUsername - test that it calls the correct route and gets the right value
        /// </summary>
        [TestMethod]
        public void LinkServiceFacadeGetColleagueByUsernameTest()
        {
            // Arrange
            var httpClient = new Lazy<HttpClient>(() => new HttpClient(new UnitTestRoutedHttpMsgHandler("api/ColleagueByEmail")));
            _serviceFacade = new LinkServiceFacade(httpClient, _mockConfigurationDataService.Object);

            // Act
            var result = _serviceFacade.GetColleagueByUsername("email@sainsburys.co.uk");

            //Assert
            Assert.IsNotNull(result, "Colleague record not found");
            Assert.AreEqual(result.EmailAddress, "email@sainsburys.co.uk");
        }

        /// <summary>
        /// IsManagerByEmail - test that it calls the correct route and gets the right value
        /// </summary>
        [TestMethod]
        public void LinkServiceFacadeIsManagerByEmailTest()
        {
            // Arrange
            var httpClient = new Lazy<HttpClient>(() => new HttpClient(new UnitTestRoutedHttpMsgHandler("api/IsManagerByEmail")));
            _serviceFacade = new LinkServiceFacade(httpClient, _mockConfigurationDataService.Object);

            // Act
            var result = _serviceFacade.IsManagerByEmail("email@sainsburys.co.uk");

            //Assert
            Assert.IsNotNull(result, "IsManagerByEmail returned false, expected true");
            Assert.IsTrue(result);
        }


        /// <summary>
        /// HasMeetingAccess - test that it calls the correct route and gets the right value
        /// </summary>
        [TestMethod]
        public void LinkServiceFacadeHasMeetingAccessTest()
        {
            // Arrange
            var httpClient = new Lazy<HttpClient>(() => new HttpClient(new UnitTestRoutedHttpMsgHandler("api/HasMeetingAccess")));
            _serviceFacade = new LinkServiceFacade(httpClient, _mockConfigurationDataService.Object);

            // Act
            var result = _serviceFacade.HasMeetingAccess(1,"e001");

            //Assert
            Assert.IsTrue(result, "HasMeetingAccess returned false, expected true");
        }

        /// <summary>
        /// HasColleagueAccess - test that it calls the correct route and gets the right value
        /// </summary>
        [TestMethod]
        public void LinkServiceFacadeHasColleagueAccessTest()
        {
            // Arrange
            var httpClient = new Lazy<HttpClient>(() => new HttpClient(new UnitTestRoutedHttpMsgHandler("api/HasColleagueAccess")));
            _serviceFacade = new LinkServiceFacade(httpClient, _mockConfigurationDataService.Object);

            // Act
            var result = _serviceFacade.HasColleagueAccess("e001", "e002");

            //Assert
            Assert.IsTrue(result, "HasColleagueAccess returned false, expected true");
        }

        /// <summary>
        /// GetUserDetails - test that it calls the correct route and gets the right value
        /// </summary>
        [TestMethod]
        public void LinkServiceFacadeGetUserDetailsTest()
        {
            // Arrange
            var httpClient = new Lazy<HttpClient>(() => new HttpClient(new UnitTestRoutedHttpMsgHandler("api/employees/?emailaddress=email@sainsburys.co.uk")));
            _serviceFacade = new LinkServiceFacade(httpClient, _mockConfigurationDataService.Object);

            // Act
            var result = _serviceFacade.GetUserDetails("email@sainsburys.co.uk");

            //Assert
            Assert.IsNotNull(result, "GetUserDetails failed, Colleague record not found");
            Assert.AreEqual(result.FirstName, "Stub Colleague");
        }

        /// <summary>
        /// GetMeeting - test that it calls the correct route and gets the right value
        /// </summary>
        [TestMethod]
        public void LinkServiceFacadeGetMeetingTest()
        {
            // Arrange
            var httpClient = new Lazy<HttpClient>(() => new HttpClient(new UnitTestRoutedHttpMsgHandler("api/meetings/1")));
            _serviceFacade = new LinkServiceFacade(httpClient, _mockConfigurationDataService.Object);

            // Act
            var result = _serviceFacade.GetMeeting(1);

            //Assert
            Assert.IsNotNull(result, "GetMeeting failed, Meeting record not found");
            Assert.AreEqual(result.MeetingId, 1);
        }

        /// <summary>
        /// UnshareMeeting - test that it calls the correct route and gets the right value
        /// </summary>
        [TestMethod]
        public void LinkServiceFacadeUnshareMeetingTest()
        {
            // Arrange
            var httpClient = new Lazy<HttpClient>(() => new HttpClient(new UnitTestRoutedHttpMsgHandler("api/unshareMeeting/1")));
            _serviceFacade = new LinkServiceFacade(httpClient, _mockConfigurationDataService.Object);

            // Act
            var result = _serviceFacade.UnshareMeeting(1).Result;

            //Assert
            Assert.IsNotNull(result, "UnshareMeeting failed, invalid MeetingView returned");
            Assert.IsInstanceOfType(result, typeof(MeetingView));
        }

        /// <summary>
        /// ApproveMeeting - test that it calls the correct route and gets the right value
        /// </summary>
        [TestMethod]
        public void LinkServiceFacadeApproveMeetingTest()
        {
            // Arrange
            var httpClient = new Lazy<HttpClient>(() => new HttpClient(new UnitTestRoutedHttpMsgHandler("api/ApproveMeeting/1")));
            _serviceFacade = new LinkServiceFacade(httpClient, _mockConfigurationDataService.Object);

            // Act
            var result = _serviceFacade.ApproveMeeting(1).Result;

            //Assert
            Assert.IsNotNull(result, "ApproveMeeting failed, invalid MeetingView returned");
            Assert.IsInstanceOfType(result, typeof(MeetingView));
        }

        /// <summary>
        /// GetNewMeetingView - test that it calls the correct route and gets the right value
        /// </summary>
        [TestMethod]
        public void LinkServiceFacadeGetNewMeetingViewTest()
        {
            // Arrange
            var httpClient = new Lazy<HttpClient>(() => new HttpClient(new UnitTestRoutedHttpMsgHandler("newmeeting/e001")));
            _serviceFacade = new LinkServiceFacade(httpClient, _mockConfigurationDataService.Object);

            // Act
            var result = _serviceFacade.GetNewMeetingView("e001");

            //Assert
            Assert.IsNotNull(result, "GetNewMeetingView failed, invalid MeetingView returned");
            Assert.IsInstanceOfType(result, typeof(MeetingView));
            Assert.AreEqual(result.ColleagueId, "e001");
        }

        /// <summary>
        /// GetNextMeeting - test that it calls the correct route and gets the right value
        /// </summary>
        [TestMethod]
        public void LinkServiceFacadeGetNextMeetingTest()
        {
            // Arrange
            var httpClient = new Lazy<HttpClient>(() => new HttpClient(new UnitTestRoutedHttpMsgHandler("mymeetings/e001/NextInFuture")));
            _serviceFacade = new LinkServiceFacade(httpClient, _mockConfigurationDataService.Object);

            // Act
            var result = _serviceFacade.GetNextMeeting("e001");

            //Assert
            Assert.IsNotNull(result, "GetNextMeeting failed, invalid LinkMeeting returned");
            Assert.IsInstanceOfType(result, typeof(LinkMeeting));
        }

        /// <summary>
        /// GetTeamView - test that it calls the correct route and gets the right value
        /// </summary>
        [TestMethod]
        public void LinkServiceFacadeGetTeamViewTest()
        {
            // Arrange
            var httpClient = new Lazy<HttpClient>(() => new HttpClient(new UnitTestRoutedHttpMsgHandler("myteam/e0011")));
            _serviceFacade = new LinkServiceFacade(httpClient, _mockConfigurationDataService.Object);

            // Act
            var result = _serviceFacade.GetTeamView("e0011");

            //Assert
            Assert.IsNotNull(result, "GetTeamView failed, invalid IEnumerable<ColleagueTeamView> returned");
            Assert.IsInstanceOfType(result, typeof(IEnumerable<ColleagueTeamView>));
        }

        /// <summary>
        /// GetMyMeetingsView - test that it calls the correct route and gets the right value
        /// </summary>
        [TestMethod]
        public void LinkServiceFacadeGetMyMeetingsViewTest()
        {
            // Arrange
            var httpClient = new Lazy<HttpClient>(() => new HttpClient(new UnitTestRoutedHttpMsgHandler("mymeetings/e001")));
            _serviceFacade = new LinkServiceFacade(httpClient, _mockConfigurationDataService.Object);

            // Act
            var result = _serviceFacade.GetMyMeetingsView("e001");

            //Assert
            Assert.IsNotNull(result, "GetMyMeetingsView failed, invalid ColleagueTeamView returned");
            Assert.IsInstanceOfType(result, typeof(ColleagueTeamView));
        }


        /// <summary>
        /// CreateObjective - test that it calls the correct route and gets the right value
        /// </summary>
        [TestMethod]
        public void LinkServiceFacadeCreateObjectiveTest()
        {
            // Arrange
            var httpClient = new Lazy<HttpClient>(() => new HttpClient(new UnitTestRoutedHttpMsgHandler("colleagues/e001/objectives")));
            _serviceFacade = new LinkServiceFacade(httpClient, _mockConfigurationDataService.Object);

            // Act
            var result = _serviceFacade.CreateObjective(new LinkObjective{ColleagueId = "e001"}).Result;

            //Assert
            Assert.IsNotNull(result, "CreateObjective failed, invalid int returned");
            Assert.AreEqual(result, 1);
        }

        /// <summary>
        /// UpdateObjective - test that it calls the correct route and gets the right value
        /// </summary>
        [TestMethod]
        public void LinkServiceFacadeUpdateObjectiveTest()
        {
            // Arrange
            var httpClient = new Lazy<HttpClient>(() => new HttpClient(new UnitTestRoutedHttpMsgHandler("colleagues/e001/objectives/1")));
            _serviceFacade = new LinkServiceFacade(httpClient, _mockConfigurationDataService.Object);

            // Act
            var result = _serviceFacade.UpdateObjective(new LinkObjective {ColleagueId = "e001", Id = 1}).Result;

            //Assert
            Assert.IsTrue(result, "UpdateObjective failed, expected true, actual false");
        }

        /// <summary>
        /// GetObjective - test that it calls the correct route and gets the right value
        /// </summary>
        [TestMethod]
        public void LinkServiceFacadeGetObjectiveTest()
        {
            // Arrange
            var httpClient = new Lazy<HttpClient>(() => new HttpClient(new UnitTestRoutedHttpMsgHandler("colleagues/e001/objectives/1")));
            _serviceFacade = new LinkServiceFacade(httpClient, _mockConfigurationDataService.Object);

            // Act
            var result = _serviceFacade.GetObjective("e001", 1);

            //Assert
            Assert.IsNotNull(result, "GetObjective failed, invalid Objective returned");
            Assert.IsInstanceOfType(result, typeof(LinkObjective));
        }

        /// <summary>
        /// GetObjectivesList - test that it calls the correct route and gets the right value
        /// </summary>
        [TestMethod]
        public void LinkServiceFacadeGetObjectivesListTest()
        {
            // Arrange
            var httpClient = new Lazy<HttpClient>(() => new HttpClient(new UnitTestRoutedHttpMsgHandler("colleagues/e001/objectives")));
            _serviceFacade = new LinkServiceFacade(httpClient, _mockConfigurationDataService.Object);

            // Act
            var result = _serviceFacade.GetObjectivesList("e001");

            //Assert
            Assert.IsNotNull(result, "GetObjectivesList failed, invalid List<LinkObjective> returned");
            Assert.IsInstanceOfType(result, typeof(List<LinkObjective>));
        }

        /// <summary>
        /// GetPdp - test that it calls the correct route and gets the right value
        /// </summary>
        [TestMethod]
        public void LinkServiceFacadeGetPdpTest()
        {
            // Arrange
            var httpClient = new Lazy<HttpClient>(() => new HttpClient(new UnitTestRoutedHttpMsgHandler("colleagues/e001/pdp/")));
            _serviceFacade = new LinkServiceFacade(httpClient, _mockConfigurationDataService.Object);

            // Act
            var result = _serviceFacade.GetPdp("e001");

            //Assert
            Assert.IsNotNull(result, "GetPdp failed, invalid LinkPdp returned");
            Assert.IsInstanceOfType(result, typeof(LinkPdp));
        }

        /// <summary>
        /// UpdatePdp - test that it calls the correct route and gets the right value
        /// </summary>
        [TestMethod]
        public void LinkServiceFacadeUpdatePdpTest()
        {
            // Arrange
            var httpClient = new Lazy<HttpClient>(() => new HttpClient(new UnitTestRoutedHttpMsgHandler("colleagues/e001/pdp/1")));
            _serviceFacade = new LinkServiceFacade(httpClient, _mockConfigurationDataService.Object);

            // Act
            var result = _serviceFacade.UpdatePdp(new LinkPdp{ColleagueId = "e001", Id=1}).Result;

            //Assert
            Assert.IsNotNull(result, "UpdatePdp failed, Invalid linkPdp returned");
            Assert.IsInstanceOfType(result, typeof(LinkPdp));
        }

        /// <summary>
        /// Dispose - test that it calls the correct route and gets the right value
        /// </summary>
        [TestMethod]
        public void LinkServiceFacadeDisposeTest()
        {
            // Arrange
            var httpClient = new Lazy<HttpClient>(() => new HttpClient(new UnitTestRoutedHttpMsgHandler("colleagues/e001/pdp/1")));
            _serviceFacade = new LinkServiceFacade(httpClient, _mockConfigurationDataService.Object);

            // Act
            _serviceFacade.Dispose();

            //Assert
            Assert.IsTrue(true);
        }

        /// <summary>
        /// GetApiServiceKeys - test that it calls the correct route and gets the right value
        /// </summary>
        [TestMethod]
        public void LinkServiceFacadeGetApiServiceKeysTest()
        {
            // Arrange
            var httpClient = new Lazy<HttpClient>(() => new HttpClient(new UnitTestRoutedHttpMsgHandler("api/ShowKeys")));
            _serviceFacade = new LinkServiceFacade(httpClient, _mockConfigurationDataService.Object);

            // Act
            var result = _serviceFacade.GetApiServiceKeys();

            //Assert
            Assert.IsNotNull(result, "GetApiServiceKeys failed, invalid List<string> returned");
            Assert.IsInstanceOfType(result, typeof(List<string>));
        }
        /// <summary>
        /// All methods - test that it when not success, we get null return value
        /// </summary>
        [TestMethod]
        public void TestApiCallNotSuccessReturnsNull()
        {
            // Arrange
            var httpClient = new Lazy<HttpClient>(() => new HttpClient(new UnitTestRoutedHttpMsgHandler("/api/noSuchApiOrReturnsNotSuccess")));
            _serviceFacade = new LinkServiceFacade(httpClient, _mockConfigurationDataService.Object);

            // Act
            var result1 = _serviceFacade.GetColleague("");
            var result2 = _serviceFacade.GetColleagueByUsername("");

            var result3 = _serviceFacade.HasMeetingAccess(0, "");
            var result4 = _serviceFacade.IsManagerByEmail("");
            var result5 = _serviceFacade.HasColleagueAccess("", "");
            var result6 = _serviceFacade.GetUserDetails("");
            var result7 = _serviceFacade.GetMeeting(0);
            var result8 = _serviceFacade.UnshareMeeting(0).Result;
            var result9 = _serviceFacade.ApproveMeeting(0).Result;
            var result10 = _serviceFacade.GetNewMeetingView("");
            var result11 = _serviceFacade.GetNextMeeting("");
            var result12 = _serviceFacade.GetTeamView("");
            var result13 = _serviceFacade.GetMyMeetingsView("");
            var result14 = _serviceFacade.GetObjective("", 0);
            var result15 = _serviceFacade.GetObjectivesList("");
            var result16 = _serviceFacade.GetPdp("");
            var result17 = _serviceFacade.CreateObjective(new LinkObjective{ColleagueId = "e001"}).Result;
            var result18 = _serviceFacade.UpdateObjective(new LinkObjective { ColleagueId = "e001", Id = 1 }).Result;
            var result19 = _serviceFacade.UpdatePdp(new LinkPdp { ColleagueId = "e001", Id = 1 }).Result;

            //Assert
            Assert.IsNull(result1, "GetColleague should return null when Api NotSuccess");
            Assert.IsNull(result2, "GetColleagueByUsername should return null when Api NotSuccess");
            Assert.IsFalse(result3, "HasMeetingAccess should return false when Api NotSuccess");
            Assert.IsFalse(result4, "IsManagerByEmail should return false when Api NotSuccess");
            Assert.IsFalse(result5, "HasColleagueAccess should return false when Api NotSuccess");
            Assert.IsNull(result6, "GetUserDetails should return null when Api NotSuccess");
            Assert.IsNull(result7, "GetMeeting should return null when Api NotSuccess");
            Assert.IsNull(result8, "UnshareMeeting should return null when Api NotSuccess");
            Assert.IsNull(result9, "ApproveMeeting should return null when Api NotSuccess");
            Assert.IsNull(result10, "GetNewMeetingView should return null when Api NotSuccess");
            Assert.IsNull(result11, "GetNextMeeting should return null when Api NotSuccess");
            Assert.IsNull(result12, "GetTeamView should return null when Api NotSuccess");
            Assert.IsNull(result13, "GetMyMeetingsView should return null when Api NotSuccess");
            Assert.IsNull(result14, "GetObjective should return null when Api NotSuccess");
            Assert.IsNull(result15, "GetObjectivesList should return null when Api NotSuccess");
            Assert.IsNull(result16, "GetPdp should return null when Api NotSuccess");
            Assert.IsTrue(result17==0, "CreateObjective should return 0 when Api NotSuccess");
            Assert.IsFalse(result18, "UpdateObjective should return false when Api NotSuccess");
            Assert.IsNull(result19, "UpdatePdp should return null when Api NotSuccess");
        }

    }
    [ExcludeFromCodeCoverage]
    internal class UnitTestRoutedHttpMsgHandler : HttpMessageHandler
    {
        private string _apiRoute = "";

        public UnitTestRoutedHttpMsgHandler(string apiRoute)
        {
            _apiRoute = apiRoute;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (_apiRoute.Contains("noSuchApiOrReturnsNotSuccess"))
            {
                string c = Json.Encode(GetStubColleague());
                return Task.FromResult(GetNotSucessResponseMessage(c));
            }
            var methodOtherThanGet = "";
            if (request.Method != HttpMethod.Get)
            {
                methodOtherThanGet = request.Method.Method.ToUpper();
            }

            // Cases for all LinkServiceFacade routes (NOTE: Lowercase strings)
            switch (methodOtherThanGet + request.RequestUri.PathAndQuery.ToLower())
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
                case "/api/ismanagerbyemail/email@sainsburys.co.uk":
                    {
                        string cbool = Json.Encode(true);
                        return Task.FromResult(GetJsonResponseMessage(cbool));
                    }
                case "/api/hasmeetingaccess/1/e001":
                    {
                        string cbool = Json.Encode(true);
                        return Task.FromResult(GetJsonResponseMessage(cbool));
                    }
                case "/api/hascolleagueaccess/e001/e002":
                    {
                        string cbool = Json.Encode(true);
                        return Task.FromResult(GetJsonResponseMessage(cbool));
                    }
                case "/api/employees/?emailaddress=email@sainsburys.co.uk":
                    {
                        string c = Json.Encode(GetStubColleague());
                        return Task.FromResult(GetJsonResponseMessage(c));
                    }
                case "/api/meetings/1":
                    {
                        string c = Json.Encode(GetStubMeeting());
                        return Task.FromResult(GetJsonResponseMessage(c));
                    }
                case "/api/unsharemeeting/1":
                    {
                        string c = Json.Encode(GetStubMeeting());
                        return Task.FromResult(GetJsonResponseMessage(c));
                    }
                case "/api/approvemeeting/1":
                    {
                        string c = Json.Encode(GetStubMeeting());
                        return Task.FromResult(GetJsonResponseMessage(c));
                    }
                case "/newmeeting/e001":
                    {
                        string c = Json.Encode(GetStubMeeting());
                        return Task.FromResult(GetJsonResponseMessage(c));
                    }
                case "/mymeetings/e001/nextinfuture":
                    {
                        string c = Json.Encode(GetStubLinkMeeting());
                        return Task.FromResult(GetJsonResponseMessage(c));
                    }
                case "/myteam/e0011":
                    {
                        string c = Json.Encode(GetStubTeamView());
                        return Task.FromResult(GetJsonResponseMessage(c));
                    }
                case "/mymeetings/e001":
                    {
                        string c = Json.Encode(GetStubTeamView().First());
                        return Task.FromResult(GetJsonResponseMessage(c));
                    }
                case "/colleagues/e001/objectives/1":
                    {
                        string c = Json.Encode(GetStubObjective());
                        return Task.FromResult(GetJsonResponseMessage(c));
                    }
                case "/colleagues/e001/objectives":
                    {
                        string c = Json.Encode(GetStubListObjectives());
                        return Task.FromResult(GetJsonResponseMessage(c));
                    }
                case "/colleagues/e001/pdp/":
                    {
                        string c = Json.Encode(GetStubPdp());
                        return Task.FromResult(GetJsonResponseMessage(c));
                    }
                case "/api/showkeys":
                    {
                        string c = Json.Encode(new[] { "string1", "string2" });
                        return Task.FromResult(GetJsonResponseMessage(c));
                    }
                case "POST/colleagues/e001/objectives/":
                    {
                        string c = Json.Encode("{}");
                        var response = GetJsonResponseMessageWithLocationHeader(c,
                            new Uri("http://somehost/colleagues/e001/objectives/1"));
                        return Task.FromResult(response);
                    }
                case "PUT/colleagues/e001/objectives/1":
                    {
                        string c = Json.Encode("{true}");
                        return Task.FromResult(GetJsonResponseMessage(c));
                    }
                case "PUT/colleagues/e001/pdp/":
                    {
                        string c = Json.Encode(GetStubPdp());
                        return Task.FromResult(GetJsonResponseMessage(c));
                    }
            }
            return null;
        }

        private MeetingView GetStubMeeting()
        {
            return new MeetingView{ MeetingId = 1, ColleagueId = "e001"};
        }

        private LinkMeeting GetStubLinkMeeting()
        {
            return new LinkMeeting { ColleagueId = "e001"};
        }
        private ColleagueView GetStubColleague()
        {
            return new ColleagueView { FirstName = "Stub Colleague", EmailAddress = "email@sainsburys.co.uk" };
        }

        private IEnumerable<ColleagueTeamView> GetStubTeamView()
        {
            return new List<ColleagueTeamView>
            {
                new ColleagueTeamView {
                    Colleague = new ColleagueView 
                    { FirstName = "Stub Colleague1", EmailAddress = "email1@sainsburys.co.uk" }
                },
                new ColleagueTeamView {
                    Colleague = new ColleagueView 
                    { FirstName = "Stub Colleague2", EmailAddress = "email2@sainsburys.co.uk" }
                }
            };
        }

        private LinkObjective GetStubObjective()
        {
            return new LinkObjective { Id = 1, ColleagueId = "e001" };
        }

        private List<LinkObjective> GetStubListObjectives()
        {
            return new List<LinkObjective> { GetStubObjective(), GetStubObjective() };
        }

        private LinkPdp GetStubPdp()
        {
            return new LinkPdp {Id = 1, ColleagueId = "e001"};
        }


        private HttpResponseMessage GetJsonResponseMessageWithLocationHeader(string jsonContent, Uri locationUri)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json"),
            };
            response.Headers.Location = locationUri;
            return response;
        }

        private HttpResponseMessage GetJsonResponseMessage(string jsonContent)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json"),
            };
            return response;
        }

        private HttpResponseMessage GetNotSucessResponseMessage(string jsonContent)
        {
            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json"),
            };
            return response;
        }
    }
}
