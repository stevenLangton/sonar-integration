using Owin;
using System.Web.Http;
using Microsoft.Owin;

//[assembly: OwinStartupAttribute(typeof(JsPlc.Ssc.Link.Portal.Startup))]
namespace JsPlc.Ssc.Link.Portal
{
    // We might have 2 startup classes (One for Dev one for Production and so on...)
    // We can control the app startup pipeline hence..
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }

    public class DevStartup
    {
        public void Configuration(IAppBuilder app)
        {
            // Enable tracing, debugging and what not..
            new Startup().Configuration(app);
        }
    }
    public class ProductionStartup
    {
        public void Configuration(IAppBuilder app)
        {
            // Checks for production if any
            new Startup().Configuration(app);
        }
    }
}
