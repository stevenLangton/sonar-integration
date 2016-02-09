using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JsPlc.Ssc.Link.Portal.Controllers;
using System.Web.Mvc;
using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Portal.Tests.Helpers;

using Moq;
using Moq.Language.Flow;
using JsPlc.Ssc.Link.Models.Entities;

namespace JsPlc.Ssc.Link.Portal.Tests.Controllers
{
    [TestClass]
    public class PdpControllerTests
    {
        [TestMethod]
        public void IndexWithParam()
        {
            //Arrange
            var controller = new PdpController(TestMocks.AnyCurrentUser(), TestMocks.LinkServiceFacade());

            //Act
            ActionResult result = controller.Index(Mock.Of<LinkPdp>());

            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Index()
        {
            //Arrange
            var controller = new PdpController(TestMocks.AnyCurrentUser(), TestMocks.LinkServiceFacade());

            //Act
            ActionResult result = controller.Index();

            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetPdp()
        {
            //Arrange
            var controller = new PdpController(TestMocks.AnyCurrentUser(), TestMocks.LinkServiceFacade());

            //Act
            ActionResult result = controller.GetPdp("AnyColleagueId");

            //Assert
            Assert.IsNotNull(result);
        }
    }
}
