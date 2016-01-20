using System;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(JsPlc.Ssc.Link.StubService.Startup))]

namespace JsPlc.Ssc.Link.StubService
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
