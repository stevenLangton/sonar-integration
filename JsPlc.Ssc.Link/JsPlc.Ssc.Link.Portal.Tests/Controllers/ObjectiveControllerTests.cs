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
using JsPlc.Ssc.Link.Portal.Tests.Helpers;

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
            var controller = new ObjectiveController(TestMocks.AnyCurrentUser(), TestMocks.LinkServiceFacade());

            //Act
            ActionResult result = controller.Index();

            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetAllColleagueObjectives()
        {
            //Arrange
            var controller = new ObjectiveController(new LinkUserView(), TestMocks.LinkServiceFacade());

            //Act 
            JsonResult response = controller.GetAllColleagueObjectives("Any string") as JsonResult;
            List<LinkObjective> ObjectivesList = response.Data as List<LinkObjective>;

            //Assert
            TestMocks.IsSameAsMockObjectivesList(ObjectivesList);
        }

        [TestMethod]
        public void GetObjectives()
        {
            //Arrange
            var controller = new ObjectiveController(new LinkUserView(), TestMocks.LinkServiceFacade());

            //Act 
            JsonResult response = controller.GetObjectives("Any string") as JsonResult;
            List<LinkObjective> ObjectivesList = response.Data as List<LinkObjective>;

            //Assert
            TestMocks.IsSameAsMockObjectivesList(ObjectivesList);
        }

        /// <summary>
        /// Test the New method returning Specific "Show" View
        /// </summary>
        [TestMethod]
        public void New()
        {
            //Arrange
            var controller = new ObjectiveController(TestMocks.AnyCurrentUser(), TestMocks.LinkServiceFacade());

            //Act
            var actResult = controller.New() as ViewResult;

            //Assert
            Assert.AreEqual(actResult.ViewName, "Show");
        }

        [TestMethod]
        public async void Show()
        {
            //Arrange
            var controller = new ObjectiveController(TestMocks.AnyCurrentUser(), TestMocks.LinkServiceFacade());

            //Act
            var actResult = await controller.Show(101) as ViewResult;

            //Assert
            Assert.IsNotNull(actResult);
        }

        [TestMethod]
        public async void Create()
        {
            //Arrange
            var controller = new ObjectiveController(TestMocks.AnyCurrentUser(), TestMocks.LinkServiceFacade());

            //Act
            JsonResult actual = await controller.Create(Mock.Of<LinkObjective>()) as JsonResult;

            //Assert
            IDictionary<string, object> data = new RouteValueDictionary(actual.Data);
            Assert.AreEqual(true, data["success"]);
        }

        [TestMethod]
        public void Update()
        {
            //Arrange
            var controller = new ObjectiveController(TestMocks.AnyCurrentUser(), TestMocks.LinkServiceFacade());

            //Act
            JsonResult actual = controller.Update(Mock.Of<LinkObjective>()) as JsonResult;

            //Assert
            IDictionary<string, object> data = new RouteValueDictionary(actual.Data);
            Assert.AreEqual(true, data["success"]);
        }
    }
}
