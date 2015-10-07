using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
//using Moq;
using System.Web.Routing;

namespace JsPlc.Ssc.Link.Portal.Tests
{
    public static class TestHelpers
    {
        //public static Controller AddUser(Controller con)
        //{

        //    var identity = new Mock<IIdentity>();
        //    var userMock = new Mock<IPrincipal>();
        //    //userMock.Setup(p => p.IsInRole("admin")).Returns(true);
        //    userMock.Setup(p => p.Identity).Returns(identity.Object);

        //    var contextMock = new Mock<HttpContextBase>();
        //    contextMock.ExpectGet(ctx => ctx.User)
        //               .Returns(userMock.Object);

        //    var controllerContextMock = new Mock<ControllerContext>();
        //    controllerContextMock.SetupGet(x => x.HttpContext)
        //                         .Returns(contextMock.Object);

        //    con.ControllerContext = controllerContextMock.Object;

        //    return con;

        //}

        //public static Controller AddHttpRequest(Controller con)
        //{
        //    var request = new Mock<HttpRequestBase>();
        //    // Not working - IsAjaxRequest() is static extension method and cannot be mocked
        //    //request.Setup(x => x.Headers["X-Requested-With"]).Returns(true /* or false */);
        //    // use this
        //    request.SetupGet(x => x.Headers).Returns(
        //            new System.Net.WebHeaderCollection { { "X-Requested-With", "XMLHttpRequest" } }
        //        );

        //    request.SetupGet(x => x.ApplicationPath).Returns("/");

        //    var context = new Mock<HttpContextBase>();
        //    context.SetupGet(x => x.Request).Returns(request.Object);

        //    //var controller = new YourController();
        //    con.ControllerContext = new ControllerContext(context.Object, new RouteData(), con);

        //    return con;
        //}
    }
}
