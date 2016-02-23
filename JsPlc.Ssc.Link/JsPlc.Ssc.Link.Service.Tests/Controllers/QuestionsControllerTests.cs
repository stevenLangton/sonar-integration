using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Results;
using System.Web.Razor.Generator;
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
    public class QuestionsControllerTests:RepositoryMock
    {
        [TestMethod]
        public void GetQuestions()
        {
            IMeetingService mockMeetingService = new MeetingService(RepositoryMock.Context);
            var controller = new QuestionsController(mockMeetingService, null);
            //var mockColleagueService = new Mock<IColleagueService>();
            //var colleagueService = mockColleagueService.Object;
            
            var response = controller.GetQuestions() as OkNegotiatedContentResult<IEnumerable<Question>>;
            Assert.IsNotNull(response);
            var questions = response.Content;
            var numques = questions as Question[] ?? questions.ToArray();
            Assert.IsTrue(numques.Count() == 5, "Wrong number of questions found:" + numques.Count()); // MockContext has 5 questions setup
        }
    }
}
