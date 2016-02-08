using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using JsPlc.Ssc.Link.Interfaces.Services;
using JsPlc.Ssc.Link.Models;
using System.Web.Configuration;
using Microsoft.Ajax.Utilities;

namespace JsPlc.Ssc.Link.Service.Services
{
    public class ConfigurationDataService : IConfigurationDataService
    {
        string IConfigurationDataService.GetConfigSettingValue(string configKey)
        {
            string settingValue = WebConfigurationManager.AppSettings[configKey];
            return settingValue;
        }
    }
}
