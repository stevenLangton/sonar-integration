using JsPlc.Ssc.Link.Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace JsPlc.Ssc.Link.Portal
{
    public class LinkServiceFacade : IDisposable
    {
        private Lazy<HttpClient> _client;

        public LinkServiceFacade()
        {
            _client = new Lazy<HttpClient>();
            _client.Value.BaseAddress = new Uri("http://localhost/JsPlc.Ssc.Link.Service/api/");
            _client.Value.DefaultRequestHeaders.Accept.Clear();
            _client.Value.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public void GetAMeeting(Int64 MeetingId)
        {
            HttpResponseMessage response = _client.Value.GetAsync("questions/?periodid=" + MeetingId.ToString()).Result;

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsAsync<IEnumerable<Questions>>().Result;
            }
            else
            {
                //ViewBag.result = "Error, Unable to connect to service.";
            }
        }

        public void Dispose()
        {
            _client = null;
        }
    }
}