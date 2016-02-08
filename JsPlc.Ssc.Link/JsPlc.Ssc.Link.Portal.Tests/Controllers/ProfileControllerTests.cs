using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JsPlc.Ssc.Link.Portal.Controllers;
using System.Web.Mvc;
using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Portal.Tests.Helpers;

namespace JsPlc.Ssc.Link.Portal.Tests.Controllers
{
    [TestClass]
    public class ProfileControllerTests
    {
        [TestMethod]
        public void Show()
        {
            //Arrange
            var controller = new ProfileController(TestMocks.AnyCurrentUser(), TestMocks.LinkServiceFacade());

            //Act
            ActionResult result = controller.Show("Anything");

            //Assert
            Assert.IsNotNull(result);
        }
    }
}
