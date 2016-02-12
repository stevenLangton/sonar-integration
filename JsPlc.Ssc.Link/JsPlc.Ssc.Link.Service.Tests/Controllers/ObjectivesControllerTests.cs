using System.Collections.Generic;
using System.Web.Http.Results;
using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Service.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using JsPlc.Ssc.Link.Models.Entities;
using Moq;
using Moq.Language.Flow;

using JsPlc.Ssc.Link.Interfaces.Services;

namespace JsPlc.Ssc.Link.Service.Tests.Controllers
{
    [TestClass]
    public class ObjectivesControllerTests
    {
        private static List<LinkObjective> MockObjectives = new List<LinkObjective>(new LinkObjective[] { 
                new LinkObjective(){Objective = "Freedom"}, 
                new LinkObjective() {Objective = "Peace"}, 
                new LinkObjective(){Objective = "Stability"} 
            });

        ObjectivesController _controller;

        private Mock<IObjectivesService> MockService()
        {
            var _mockService = new Mock<IObjectivesService>();
            _mockService.Setup(x => x.GetAllObjectives(It.IsAny<string>())).Returns(MockObjectives);

            _mockService.Setup(x => x.DeleteObjective(It.IsAny<int>())).Returns(true);

            _mockService.Setup(x => x.GetObjective(It.IsAny<int>())).Returns(new LinkObjective() { Objective = "May the force be with you" });

            return _mockService;
        }

        [TestInitialize]
        public void TestSetup()
        {
            _controller = new ObjectivesController(MockService().Object);
        }

        [TestMethod]
        public void TestGetAllObjectives()
        {
            var response = _controller.GetAllObjectives("Any string") as OkNegotiatedContentResult<List<LinkObjective>>;
            List<LinkObjective> ObjectivesList = response.Content as List<LinkObjective>;
            Assert.IsTrue(ObjectivesList.Count==MockObjectives.Count, @"Unexpected number of Objectives found");
        }

        [TestMethod]
        public void TestDeleteObjective()
        {
            bool response = _controller.DeleteObjective(123);
            Assert.IsTrue(response);
        }

        [TestMethod]
        public void TestGetObjective()
        {
            var response = _controller.GetObjective("Any string", 123) as OkNegotiatedContentResult<LinkObjective>;
            LinkObjective item = response.Content as LinkObjective;
            Assert.IsNotNull(response);
        }
    }
}
