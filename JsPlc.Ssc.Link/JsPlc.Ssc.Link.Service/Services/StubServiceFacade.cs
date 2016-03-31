using System.Globalization;
using System.Threading.Tasks;
using JsPlc.Ssc.Link.Interfaces.Services;
using JsPlc.Ssc.Link.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace JsPlc.Ssc.Link.Service.Services
{
    public class StubServiceFacade : IStubServiceFacade
    {
        private Lazy<HttpClient> _client;

		public StubServiceFacade(Lazy<HttpClient> client=null, IConfigurationDataService configurationDataService=null)
        {
            _client = client ?? new Lazy<HttpClient>();
            //_client.Value.BaseAddress = new Uri("http://localhost/JsPlc.Ssc.Link.StubService/api/");
            var url = (configurationDataService == null)
                ? ConfigurationManager.AppSettings["ServicesBaseUrl"] + ""
                : configurationDataService.GetConfigSettingValue("ServicesBaseUrl");
            _client.Value.BaseAddress = new Uri(url); 
            
            _client.Value.DefaultRequestHeaders.Accept.Clear();
            _client.Value.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public ColleagueView GetColleague(string colleagueId)
        {
            HttpResponseMessage response = _client.Value.GetAsync("api/colleague/" + colleagueId.ToString(CultureInfo.InvariantCulture)).Result;

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsAsync<ColleagueView>().Result;
                return result;
            }
            return null;
        }

        public ColleagueView GetColleagueByEmail(string email)
        {
            HttpResponseMessage response = _client.Value.GetAsync("api/colleagueByEmail/" + email.ToString(CultureInfo.InvariantCulture)).Result;

			ColleagueView result = null;

			if (response.IsSuccessStatusCode)
            {
                result = response.Content.ReadAsAsync<ColleagueView>().Result;
            }

			return result;
        }

        public IEnumerable<ColleagueView> GetDirectReports(string managerId)
        {
            HttpResponseMessage response = _client.Value.GetAsync("api/directReports/" + managerId.ToString(CultureInfo.InvariantCulture)).Result;
            
            if (!response.IsSuccessStatusCode) return null;
            
            var directReports = response.Content.ReadAsAsync<IEnumerable<ColleagueView>>().Result;
            return directReports;
        }

        public IEnumerable<ColleagueView> GetDirectReportsByManagerEmail(string managerEmail)
        {
            HttpResponseMessage response = _client.Value.GetAsync("api/directReportsByEmail/" + managerEmail.ToString(CultureInfo.InvariantCulture)).Result;
            
            if (!response.IsSuccessStatusCode) return null;

            var directReports = response.Content.ReadAsAsync<IEnumerable<ColleagueView>>().Result;
            return directReports;
        }

        public void Dispose()
        {
            _client = null;
        }
    }

}