using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using JsPlc.Ssc.Link.Portal.Models;
using JsPlc.Ssc.Link.Portal.Helpers;

using Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using System.Configuration;
using System.Globalization;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Threading.Tasks;
using System.Security.Claims;


namespace JsPlc.Ssc.Link.Portal
{
    public partial class Startup
    {
        //
        // The Client ID is used by the application to uniquely identify itself to Azure AD.
        // The App Key is a credential used to authenticate the application to Azure AD.  Azure AD supports password and certificate credentials.
        // The Metadata Address is used by the application to retrieve the signing keys used by Azure AD.
        // The AAD Instance is the instance of Azure, for example public Azure or Azure China.
        // The Authority is the sign-in URL of the tenant.
        // The Post Logout Redirect Uri is the URL where the user will be redirected after they sign out.
        //
        private static string clientId = ConfigurationManager.AppSettings["ida:ClientId"];
        private static string appKey = ConfigurationManager.AppSettings["ida:AppKey"];
        private static string aadInstance = ConfigurationManager.AppSettings["ida:AADInstance"];
        private static string tenant = ConfigurationManager.AppSettings["ida:Tenant"];
        private static string postLogoutRedirectUri = ConfigurationManager.AppSettings["ida:PostLogoutRedirectUri"];

        public static readonly string Authority = String.Format(CultureInfo.InvariantCulture, aadInstance, tenant);

        // This is the resource ID of the AAD Graph API.  We'll need this to request a token to call the Graph API.
        string graphResourceId = ConfigurationManager.AppSettings["ida:GraphResourceId"];

        public void ConfigureAuth(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            app.UseCookieAuthentication(new CookieAuthenticationOptions());

            app.UseOpenIdConnectAuthentication(
                new OpenIdConnectAuthenticationOptions
                {
                    ClientId = clientId,
                    Authority = Authority,
                    PostLogoutRedirectUri = postLogoutRedirectUri,

                    Notifications = new OpenIdConnectAuthenticationNotifications()
                    {
                        //
                        // If there is a code in the OpenID Connect response, redeem it for an access token and refresh token, and store those away.
                        //
                        AuthorizationCodeReceived = (context) =>
                        {
                            var code = context.Code;

                            ClientCredential credential = new ClientCredential(clientId, appKey);
                            string userObjectID = context.AuthenticationTicket.Identity.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
                            AuthenticationContext authContext = new AuthenticationContext(Authority, new NaiveSessionCache(userObjectID));
                            AuthenticationResult result = authContext.AcquireTokenByAuthorizationCode(code, new Uri(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Path)), credential, graphResourceId);

                            return Task.FromResult(0);
                        },

                        AuthenticationFailed = context =>
                        {
                            context.HandleResponse();
                            context.Response.Redirect("/Home/Error?message=" + context.Exception.Message);
                            return Task.FromResult(0);
                        }

                    }

                });
        }



        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        //public void ConfigureAuth(IAppBuilder app)
        //{
        //    // Configure the db context, user manager and signin manager to use a single instance per request
        //    app.CreatePerOwinContext(ApplicationDbContext.Create);
        //    app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
        //    app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

        //    // Enable the application to use a cookie to store information for the signed in user
        //    // and to use a cookie to temporarily store information about a user logging in with a third party login provider
        //    // Configure the sign in cookie
        //    app.UseCookieAuthentication(new CookieAuthenticationOptions
        //    {
        //        AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
        //        LoginPath = new PathString("/Account/Login"),
        //        Provider = new CookieAuthenticationProvider
        //        {
        //            // Enables the application to validate the security stamp when the user logs in.
        //            // This is a security feature which is used when you change a password or add an external login to your account.  
        //            OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
        //                validateInterval: TimeSpan.FromMinutes(30),
        //                regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
        //        }
        //    });            
        //    app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

        //    // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
        //    app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

        //    // Enables the application to remember the second login verification factor such as phone or email.
        //    // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
        //    // This is similar to the RememberMe option when you log in.
        //    app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

        //    // Uncomment the following lines to enable logging in with third party login providers
        //    //app.UseMicrosoftAccountAuthentication(
        //    //    clientId: "",
        //    //    clientSecret: "");

        //    //app.UseTwitterAuthentication(
        //    //   consumerKey: "",
        //    //   consumerSecret: "");

        //    //app.UseFacebookAuthentication(
        //    //   appId: "",
        //    //   appSecret: "");

        //    //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
        //    //{
        //    //    ClientId = "",
        //    //    ClientSecret = ""
        //    //});
        //}
    }
}