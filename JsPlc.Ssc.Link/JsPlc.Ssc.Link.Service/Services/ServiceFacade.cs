using System.Globalization;
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
    public class ServiceFacade : IDisposable
    {
        private Lazy<HttpClient> _client;

        public ServiceFacade()
        {
            _client = new Lazy<HttpClient>();
            //_client.Value.BaseAddress = new Uri("http://localhost/JsPlc.Ssc.Link.StubService/api/");
            _client.Value.BaseAddress = new Uri(ConfigurationManager.AppSettings["ServicesBaseUrl"] + ""); 
            
            _client.Value.DefaultRequestHeaders.Accept.Clear();
            _client.Value.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public ColleagueView GetColleague(string colleagueId)
        {
            HttpResponseMessage response = _client.Value.GetAsync("api/colleagues/?id=" + colleagueId.ToString(CultureInfo.InvariantCulture)).Result;

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsAsync<ColleagueView>().Result;
                return result;
            }
            return null;
        }

        public ColleagueView GetColleagueByEmail(string email)
        {
            HttpResponseMessage response = _client.Value.GetAsync("api/colleagues/?email=" + email.ToString(CultureInfo.InvariantCulture)).Result;

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsAsync<ColleagueView>().Result;
                return result;
            }
            return null;
        }

        public IEnumerable<ColleagueView> GetDirectReports(string managerId)
        {
            HttpResponseMessage response = _client.Value.GetAsync("api/directReports/" + managerId.ToString(CultureInfo.InvariantCulture)).Result;
            var meeting = response.Content.ReadAsAsync<IEnumerable<ColleagueView>>().Result;
            return response.IsSuccessStatusCode ? meeting : null;
        }

        public IEnumerable<ColleagueView> GetDirectReportsByManagerEmail(string managerEmail)
        {
            HttpResponseMessage response = _client.Value.GetAsync("api/directReports/?email=" + managerEmail.ToString(CultureInfo.InvariantCulture)).Result;
            var meeting = response.Content.ReadAsAsync<IEnumerable<ColleagueView>>().Result;
            return response.IsSuccessStatusCode ? meeting : null;
        }

        public void Dispose()
        {
            _client = null;
        }
    }
}