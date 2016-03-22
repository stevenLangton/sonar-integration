using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using Microsoft.Ajax.Utilities;
using System.Threading;
using JsPlc.Ssc.Link.Models;


namespace JsPlc.Ssc.Link.Portal.Security
{
    public class AuthorizationService
    {
        public static string GetEmailAddress() {
            //var authenticatedEmailAddr = User.Identity.Name;
            var emailClaim =
                ClaimsPrincipal.Current.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");
            var authenticatedEmailAddr = "";
            if (emailClaim != null)
            {
                authenticatedEmailAddr = emailClaim.Value;
            }

            if (authenticatedEmailAddr.IsNullOrWhiteSpace())
            {
//                authenticatedEmailAddr = User.Identity.Name;
                authenticatedEmailAddr = Thread.CurrentPrincipal.Identity.Name;
            }
            // Last ditch check
            if (authenticatedEmailAddr.IsNullOrWhiteSpace())
            {
                var exceptionMsg = "";
                if (emailClaim == null || emailClaim.Value.IsNullOrWhiteSpace())
                {
                    exceptionMsg += "Error: Missing claim = http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress";
                }
                var test1 = Thread.CurrentPrincipal.Identity;
                //if (User.Identity.Name.IsNullOrWhiteSpace())
                if (Thread.CurrentPrincipal.Identity.Name.IsNullOrWhiteSpace())
                {
                    exceptionMsg += "\n --- Error: Username/Email expected on authentication, Found blank email address claim value";
                }
                var ex = new ApplicationException(exceptionMsg);
                throw ex; 
            }

            return authenticatedEmailAddr;
        }
        //GetEmailAddress

        public static ColleagueView GetColleague(string EmailAddress)
        {
            using (var facade = new LinkServiceFacade())
            {
                return facade.GetColleagueByUsername(EmailAddress);
            }
        }
                //        CurrentUser.IsLineManager = ServiceFacade.IsManagerByEmail(authenticatedEmailAddr);
                //CurrentUser.Colleague = ServiceFacade.GetColleagueByUsername(authenticatedEmailAddr);

        /// <summary>
        /// Team access means you can access your own data or that of your direct reports.
        /// </summary>
        /// <param name="Accessor">Colleague id of accessing user</param>
        /// <param name="Owner">Colleague id of colleague who owns the data</param>
        /// <returns></returns>
        public static bool HasTeamAccess(string Accessor, string Owner)
        {
            //if (string.IsNullOrEmpty(Accessor) || string.IsNullOrEmpty(Owner))
            //{
            //    return false;
            //}

            using (var facade = new LinkServiceFacade())
            {
                return facade.HasColleagueAccess(Accessor, Owner);
            }
        }
    }
}