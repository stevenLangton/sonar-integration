using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(JsPlc.Ssc.Link.Portal.Startup))]
namespace JsPlc.Ssc.Link.Portal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
