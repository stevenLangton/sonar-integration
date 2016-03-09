using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Moq;
using Moq.Language.Flow;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JsPlc.Ssc.Link.Portal.Tests.Helpers
{
    public class TestMocks
    {
        private static List<LinkObjective> MockObjectives = new List<LinkObjective>(new LinkObjective[] { 
                        new LinkObjective(){Objective = "Freedom"}, 
                        new LinkObjective() {Objective = "Peace"}, 
                        new LinkObjective(){Objective = "Stability"} 
                    });

        public static LinkUserView AnyCurrentUser()
        {
            var user = new LinkUserView();
            user.Colleague = new ColleagueView();
            user.Colleague.ColleagueId = "Any thing";
            return user;
        }

        public static ILinkServiceFacade LinkServiceFacade()
        {
            var LinkService = new Mock<ILinkServiceFacade>();

            var TestList = TestMocks.GetMockObjectivesList();
            var anObjective = Mock.Of<LinkObjective>();

            LinkService.Setup(x => x.GetObjectivesList(It.IsAny<string>()))
                .Returns(TestList);

            LinkService.Setup(x => x.GetObjective(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Task.FromResult(anObjective));

            Random rnd = new Random();
            LinkService.Setup(x => x.CreateObjective(It.IsAny<LinkObjective>()))
                .Returns(() => Task<int>.Factory.StartNew(() => rnd.Next()));

            LinkService.Setup(x => x.UpdateObjective(It.IsAny<LinkObjective>()))
                .Returns(() => Task<bool>.Factory.StartNew(() => true));

            LinkService.Setup(x => x.GetColleague(It.IsAny<String>()))
                .Returns(Mock.Of<ColleagueView>());

            LinkService.Setup(x => x.GetNextMeeting(It.IsAny<String>()))
                .Returns(Mock.Of<LinkMeeting>());

            LinkService.Setup(x => x.GetMyMeetingsView(It.IsAny<String>()))
                .Returns(Mock.Of<ColleagueTeamView>());

            return LinkService.Object as ILinkServiceFacade;
        }

        public static List<LinkObjective> GetMockObjectivesList()
        {
            return MockObjectives;
        }

        public static void IsSameAsMockObjectivesList(List<LinkObjective> ReturnedList)
        {
            //Verify list contains similar items
            Assert.IsTrue(Enumerable.SequenceEqual(ReturnedList.OrderBy(x => x.Objective), MockObjectives.OrderBy(x => x.Objective)));

            //Verify list has 3 items
            Assert.IsTrue(ReturnedList.Count == MockObjectives.Count);

            //Search for all 3 objectives
            List<string> searchTerms = new List<string>(new string[] { "Freedom", "Peace", "Stability" });
            Assert.IsTrue(ReturnedList.FindAll(x => searchTerms.Contains(x.Objective)).Count == 3);
        }

    }
}
