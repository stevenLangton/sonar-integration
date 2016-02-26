using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.WebPages;
using Elmah;
using JsPlc.Ssc.Link.Portal.Models;
using JsPlc.Ssc.Link.Portal.Helpers;
using Microsoft.Owin;
using Microsoft.Owin.Infrastructure;
using Microsoft.Owin.Security.MicrosoftAccount;
using Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using System.Configuration;
using System.Globalization;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Threading.Tasks;
using System.Security.Claims;
using AuthenticationContext = Microsoft.IdentityModel.Clients.ActiveDirectory.AuthenticationContext;


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
            app.UseKentorOwinCookieSaver();

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
                            Elmah.ErrorSignal.FromCurrentContext().Raise(context.Exception);
                            //context.Response.Redirect("/Home/Error?message=" + context.Exception.Message);
                            context.Response.Redirect("/");
                            return Task.FromResult(0);
                        }

                    },
                    TokenValidationParameters = new System.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = false
                    }

                });
        }

        /// <summary>
        /// This class allows us to append the cookie domain to the usual auth cookies
        /// http://stackoverflow.com/questions/28252478/how-is-owin-able-to-set-the-asp-net-identity-authentication-cookies-after-the-ap
        /// </summary>
        public class ChunkingCookieManagerWithSubdomains : ICookieManager
        {
            private readonly ChunkingCookieManager _chunkingCookieManager;

            public ChunkingCookieManagerWithSubdomains()
            {
                _chunkingCookieManager = new ChunkingCookieManager();
            }

            public string GetRequestCookie(IOwinContext context, string key)
            {
                return _chunkingCookieManager.GetRequestCookie(context, key);
            }

            public void AppendResponseCookie(IOwinContext context, string key, string value, CookieOptions options)
            {
                // Simplification (use the context parameter to get the required request info)
                //options.Domain = ".localhost";
                options.HttpOnly = true;
                options.Expires = DateTime.Now.AddYears(1); // Make it work in IE, possibly misinterprets date of cookie expiry if not set.
                _chunkingCookieManager.AppendResponseCookie(context, key, value, options);
                var user = new ClaimsIdentity(context.Authentication.User.Claims);
                context.Authentication.SignIn(new AuthenticationProperties { RedirectUri = "/", IsPersistent = true }, user);
            }

            public void DeleteCookie(IOwinContext context, string key, CookieOptions options)
            {
                // Simplification (use the context parameter to get the required request info)
                //options.Domain = "localhost:59387"; //".domainBasedOnRequestInContext.com";
                _chunkingCookieManager.DeleteCookie(context, key, options);
            }
        }
    }
}