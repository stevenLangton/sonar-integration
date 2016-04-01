using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using JsPlc.Ssc.Link.Interfaces.Services;
using JsPlc.Ssc.Link.Models;
using System.Web.Configuration;
using Microsoft.Ajax.Utilities;
using System;
using log4net;

namespace JsPlc.Ssc.Link.Service.Services
{
    public class DomainTranslationService : IDomainTranslationService
    {
        private readonly IConfigurationDataService _configurationDataService;
		
        public DomainTranslationService() { }

        public DomainTranslationService(IConfigurationDataService configurationDataService) { _configurationDataService = configurationDataService; }

        /// <summary>
        /// Used to replace domain coming from Azure AD to that stored in DB for lookups to work in dev, test and live
        /// </summary>
        /// <param name="colleagueEmail"></param>
        /// <param name="configurationDataService"></param>
        /// <returns></returns>
        private static string _AdDomainToDbDomain(string colleagueEmail, IConfigurationDataService configurationDataService)
        {
			//domain to use
            string azureAdEmailDomain = configurationDataService.GetConfigSettingValue("AzureLinkDomain"); //WebConfigurationManager.AppSettings["AzureLinkDomain"];
            string dbLinkDomain = configurationDataService.GetConfigSettingValue("DbLinkDomain"); //WebConfigurationManager.AppSettings["DbLinkDomain"];
            if (dbLinkDomain.IsNullOrWhiteSpace()) dbLinkDomain = "@domain.com"; // use stubbed values

            var inputString = colleagueEmail;
            string[] parts = inputString.Split('@');
            if (parts.Length < 2) throw new ApplicationException("Invalid email: expected at least one @ symbol in it : " + colleagueEmail);

            // No need to touch anything if both domains match
            if (azureAdEmailDomain.Equals(dbLinkDomain)) return colleagueEmail;

            // Replacing Authenticated email domain with the DB Email domain...
            string name = "";
            string domain = "";
            name = parts[0];
            domain = parts[1];

            if (!domain.Equals(dbLinkDomain))
            {
                colleagueEmail = string.Format("{0}{1}", name, dbLinkDomain);
            }

			return colleagueEmail;
        }

        string IDomainTranslationService.AdDomainToDbDomain(string colleagueEmail)
        {
            return _AdDomainToDbDomain(colleagueEmail, _configurationDataService);
        }
    }
}
