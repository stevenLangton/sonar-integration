using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JsPlc.Ssc.Link.Portal.Controllers;
using System.Web.Mvc;
using JsPlc.Ssc.Link.Models;
using Moq;
using Moq.Language.Flow;
using System.Net.Http;
using JsPlc.Ssc.Link.Models.Entities;
using System.Threading.Tasks;
using System.Web.Routing;

namespace JsPlc.Ssc.Link.Portal.Tests.Controllers
{
    /// <summary>
    /// Summary description for ObjectiveControllerTests
    /// </summary>
    [TestClass]
    public class ObjectiveControllerTests
    {
        public ObjectiveControllerTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void Index()
        {
            //Arrange
            var user = new LinkUserView();
            user.Colleague = new ColleagueView();
            user.Colleague.ColleagueId = "Any thing";
            var controller = new ObjectiveController(user, TestHelpers.MockLinkServiceFacade().Object);

            //Act
            ActionResult result = controller.Index();

            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetAllColleagueObjectives()
        {
            //Arrange
            var controller = new ObjectiveController(new LinkUserView(), TestHelpers.MockLinkServiceFacade().Object);

            //Act 
            JsonResult response = controller.GetAllColleagueObjectives("Any string") as JsonResult;
            List<LinkObjective> ObjectivesList = response.Data as List<LinkObjective>;

            //Assert
            TestHelpers.IsSameAsMockObjectivesList(ObjectivesList);
        }

        [TestMethod]
        public void GetObjectives()
        {
            //Arrange
            var controller = new ObjectiveController(new LinkUserView(), TestHelpers.MockLinkServiceFacade().Object);

            //Act 
            JsonResult response = controller.GetObjectives("Any string") as JsonResult;
            List<LinkObjective> ObjectivesList = response.Data as List<LinkObjective>;

            //Assert
            TestHelpers.IsSameAsMockObjectivesList(ObjectivesList);
        }

        /// <summary>
        /// Test the New method returning Specific "Show" View
        /// </summary>
        [TestMethod]
        public void New()
        {
            //Arrange
            var colleague = new ColleagueView() { ColleagueId = "Anything" };
            var user = new Mock<ILinkUserView>();
            user.Setup(x => x.Colleague).Returns(colleague);
            var controller = new ObjectiveController(user.Object, TestHelpers.MockLinkServiceFacade().Object);

            //Act
            var actResult = controller.New() as ViewResult;

            //Assert
            Assert.AreEqual(actResult.ViewName, "Show");
        }

        [TestMethod]
        public void Show()
        {
            //Arrange
            var colleague = new ColleagueView() { ColleagueId = "Anything" };
            var user = new Mock<ILinkUserView>();
            user.Setup(x => x.Colleague).Returns(colleague);

            var controller = new ObjectiveController(user.Object, TestHelpers.MockLinkServiceFacade().Object);

            //Act
            var actResult = controller.Show(101) as ViewResult;

            //Assert
            Assert.IsNotNull(actResult);
        }

        [TestMethod]
        public void Create()
        {
            //Arrange
            var colleague = new ColleagueView() { ColleagueId = "Anything" };
            var user = new Mock<ILinkUserView>();
            user.Setup(x => x.Colleague).Returns(colleague);

            var controller = new ObjectiveController(user.Object, TestHelpers.MockLinkServiceFacade().Object);

            //Act
            JsonResult actual = controller.Create(Mock.Of<LinkObjective>()) as JsonResult;

            //Assert
            IDictionary<string, object> data = new RouteValueDictionary(actual.Data);
            Assert.AreEqual(true, data["success"]);
        }

        [TestMethod]
        public void Update()
        {
            //Arrange
            var colleague = new ColleagueView() { ColleagueId = "Anything" };
            var user = new Mock<ILinkUserView>();
            user.Setup(x => x.Colleague).Returns(colleague);

            var LinkServiceFacade = TestHelpers.MockLinkServiceFacade();
            LinkServiceFacade.Setup(x => x.UpdateObjective(It.IsAny<LinkObjective>()))
                .Returns(() => Task<bool>.Factory.StartNew(() => true));

            var controller = new ObjectiveController(user.Object, LinkServiceFacade.Object);

            //Act
            JsonResult actual = controller.Update(Mock.Of<LinkObjective>()) as JsonResult;

            //Assert
            IDictionary<string, object> data = new RouteValueDictionary(actual.Data);
            Assert.AreEqual(true, data["success"]);
        }
        
    }

    public class TestHelpers
    {
        private static List<LinkObjective> MockObjectives = new List<LinkObjective>(new LinkObjective[] { 
                        new LinkObjective(){Objective = "Freedom"}, 
                        new LinkObjective() {Objective = "Peace"}, 
                        new LinkObjective(){Objective = "Stability"} 
                    });

        public static Mock<ILinkServiceFacade> MockLinkServiceFacade()
        {
            var LinkService = new Mock<ILinkServiceFacade>();

            var TestList = TestHelpers.GetMockObjectivesList();
            var anObjective = Mock.Of<LinkObjective>();

            LinkService.Setup(x => x.GetObjectivesList(It.IsAny<string>()))
                .Returns(TestList);

            LinkService.Setup(x => x.GetObjective(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(anObjective);

            Random rnd = new Random();
            LinkService.Setup(x => x.CreateObjective(It.IsAny<LinkObjective>()))
                .Returns(() => Task<int>.Factory.StartNew(() => rnd.Next()));

            LinkService.Setup(x => x.UpdateObjective(It.IsAny<LinkObjective>()))
                .Returns(() => Task<bool>.Factory.StartNew(() => true));

            return LinkService;
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
