using System;
using System.Collections.Generic;
using System.Web.Http.Results;
using JsPlc.Ssc.Link.Interfaces.Services;
using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Service.Controllers;
using JsPlc.Ssc.Link.Service.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JsPlc.Ssc.Link.Models.Entities;
using Moq;
using Moq.Language.Flow;

namespace JsPlc.Ssc.Link.Service.Tests.Controllers
{
    [TestClass]
    public class MeetingsControllerTests : RepositoryMock
    {
        [TestMethod]
        public void GetMeeting()
        {
            var mockStubServiceFacade = new Mock<IStubServiceFacade>();
            IStubServiceFacade stubServiceFacade = mockStubServiceFacade.Object;
            var mockGetColleagueMethodSetup = mockStubServiceFacade.Setup(facade => facade.GetColleague("E001"))
                .Returns(new ColleagueView{ FirstName = "mock colleague", ManagerId = "E003", HasManager = true });
            mockGetColleagueMethodSetup.Verifiable();

            var mockGetColleaguesManagerSetup = mockStubServiceFacade.Setup(facade => facade.GetColleague("E003"))
                .Returns(new ColleagueView
                {
                    FirstName = "mock manager"
                });
            mockGetColleaguesManagerSetup.Verifiable();

            // FORCE Failure is off.
            // Never called, only here to force test to fail 
            var mockGetColleagueMethodSetup1 = mockStubServiceFacade.Setup(facade => facade.GetColleague("E005"))
                            .Returns(new ColleagueView
                            {
                                FirstName = "manager", LastName = "lastname"
                            });
            mockGetColleagueMethodSetup1.Verifiable();

            IColleagueService mockColleagueService = new ColleagueService(stubServiceFacade);

            IMeetingService mockMeetingService = new MeetingService(RepositoryMock.Context, mockColleagueService);

            var controller = new MeetingsController(_repository, mockMeetingService, mockColleagueService);
            var result=controller.GetMeeting(1) as OkNegotiatedContentResult<MeetingView>;
            Assert.IsNotNull(result);
            // Meeting 1 has Colleague E001 and Manager E003, so expectations are below..
            mockStubServiceFacade.Verify(facade => facade.GetColleague("E001"));
            mockStubServiceFacade.Verify(facade => facade.GetColleague("E003"));
            // FORCE Failure - off.
            //mockStubServiceFacade.Verify(facade => facade.GetColleague("E005")); // Never called, so uncomment will fail test.
        }

        [TestMethod]
        public void CreateMeeting()
        {
            var controller = new MeetingsController();
            var result = controller.CreateMeeting("E001") as OkNegotiatedContentResult<MeetingView>;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void SaveMeeting()
        {
            var meeting = new MeetingView()
            {
                   MeetingId=0,
                   ColleagueId="E001",
                   ColleagueName="vasu",
                   ManagerId ="E0010",
                   ManagerName ="Sandip.V", 
                   ColleagueSignOff= MeetingStatus.Completed,  
                   ManagerSignOff = MeetingStatus.Completed,
                   MeetingDate =DateTime.Now,
                   Questions = new List<QuestionView>()
                   {
                       new QuestionView(){ QuestionId  = 1, QuestionType = "LOOKING BACK", Question = "Test1", AnswerId = 1, ColleagueComment = "testing1", ManagerComment = "testing1"},
                       new QuestionView(){ QuestionId  = 2, QuestionType = "LOOKING BACK", Question = "Test2", AnswerId = 2, ColleagueComment = "testing2", ManagerComment = "testing2"},
                       new QuestionView(){ QuestionId  = 3, QuestionType = "LOOKING FORWARD", Question = "Test3", AnswerId = 3, ColleagueComment = "testing3", ManagerComment = "testing3"},
                       new QuestionView(){ QuestionId  = 4, QuestionType = "DRIVING MY DEVELOPMENT", Question = "Test4", AnswerId = 4, ColleagueComment = "testing4", ManagerComment = ""},
                       new QuestionView(){ QuestionId  = 5, QuestionType = "IN A NUTSHELL", Question = "Test5", AnswerId = 5, ColleagueComment = "", ManagerComment = ""}
                   }
            };

            var controller = new MeetingsController();
            var result = controller.SaveMeeting(meeting);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void UpdateMeeting()
        {
            var meeting = new MeetingView()
            {
                
                MeetingId = 1,
                ColleagueId = "E001",
                ColleagueName = "vasu",
                ManagerId = "E0010",
                ManagerName = "Sandip.V",
                ColleagueSignOff = MeetingStatus.Completed,
                ManagerSignOff = MeetingStatus.Completed,
                MeetingDate = DateTime.Now,
                Questions = new List<QuestionView>()
                   {
                       new QuestionView(){ QuestionId  = 1, QuestionType = "b", Question = "Test1", AnswerId = 1, ColleagueComment = "testing1", ManagerComment = "testing1"},
                       new QuestionView(){ QuestionId  = 2, QuestionType = "b", Question = "Test2", AnswerId = 2, ColleagueComment = "testing2", ManagerComment = "testing2"},
                       new QuestionView(){ QuestionId  = 3, QuestionType = "f", Question = "Test3", AnswerId = 3, ColleagueComment = "testing3", ManagerComment = "testing3"},
                       new QuestionView(){ QuestionId  = 4, QuestionType = "f", Question = "Test4", AnswerId = 4, ColleagueComment = "testing4", ManagerComment = ""},
                       new QuestionView(){ QuestionId  = 5, QuestionType = "f", Question = "Test5", AnswerId = 5, ColleagueComment = "", ManagerComment = ""}
                   }
            };

            var controller = new MeetingsController();
            var result = controller.UpdateMeeting(meeting);
            Assert.IsNotNull(result);
        }
    }
}
