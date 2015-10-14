using System.Globalization;
using JsPlc.Ssc.Link.Models;
//using JsPlc.Ssc.Link.Portal.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace JsPlc.Ssc.Link.Portal.Helpers.Api
{
    public class LinkServiceFacade : IDisposable
    {
        private Lazy<HttpClient> _client;

        public LinkServiceFacade()
        {
            _client = new Lazy<HttpClient>();
            //_client.Value.BaseAddress = new Uri("http://localhost/JsPlc.Ssc.Link.Service/api/");
            _client.Value.BaseAddress = new Uri(ConfigurationManager.AppSettings["ServicesBaseUrl"] + "/api/");
            
            _client.Value.DefaultRequestHeaders.Accept.Clear();
            _client.Value.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public void GetQuestion(int id)
        {
            HttpResponseMessage response = _client.Value.GetAsync("questions/?periodid=" + id).Result;

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsAsync<IEnumerable<Question>>().Result;
            }
        }

        public MeetingView GetMeeting(int id)
        {
            HttpResponseMessage response = _client.Value.GetAsync("meetings/" + id.ToString(CultureInfo.InvariantCulture)).Result;

            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<MeetingView>().Result;
            }
            return null;
        }

        public MeetingView GetNewMeetingView(string colleagueId)
        {
            HttpResponseMessage response = _client.Value.GetAsync("meetings/?colleagueId=" + colleagueId).Result;

            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<MeetingView>().Result;
            }
            return null;
         }

        public bool IsManager(string username)
        {
            // api/Employees/?UserName=Mike.G@sainsburys.co.uk
            HttpResponseMessage response = _client.Value.GetAsync("Employees/?UserName=" + username).Result;

            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<bool>().Result;
            }
            return false;
        }

        public void Dispose()
        {
            _client = null;
        }
    }
}