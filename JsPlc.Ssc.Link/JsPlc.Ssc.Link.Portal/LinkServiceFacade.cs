﻿using System.Globalization;
using JsPlc.Ssc.Link.Models;
//using JsPlc.Ssc.Link.Portal.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using JsPlc.Ssc.Link.Portal.Helpers.Extensions;

namespace JsPlc.Ssc.Link.Portal
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

        // NOT USED..
        public void GetQuestion(int Id)
        {
            HttpResponseMessage response = _client.Value.GetAsync("questions/?periodid=" + Id.ToString(CultureInfo.InvariantCulture)).Result;

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsAsync<IEnumerable<Question>>().Result;
            }
        }

        public MeetingView GetMeeting(int Id)
        {
            HttpResponseMessage response = _client.Value.GetAsync("meetings/" + Id.ToString(CultureInfo.InvariantCulture)).Result;

            return response.IsSuccessStatusCode ? response.Content.ReadAsAsync<MeetingView>().Result : null;
        }

        public MeetingView GetNewMeetingView(string colleagueId)
        {
            HttpResponseMessage response = _client.Value.GetAsync("meetings/?colleagueId=" + colleagueId).Result;

            return response.IsSuccessStatusCode ? response.Content.ReadAsAsync<MeetingView>().Result : null;
        }

        public bool IsManager(string username)
        {
            HttpResponseMessage response = _client.Value.GetAsync("Employees/?UserName=" + username).Result;

            return response.IsSuccessStatusCode && response.Content.ReadAsAsync<bool>().Result;
        }

        public EmployeeView GetColleagueByUsername(string username)
        {
            HttpResponseMessage response = _client.Value.GetAsync("Employees/?EmailAddress=" + username).Result;
            var employee = response.Content.ReadAsAsync<Employee>().Result;

            return response.IsSuccessStatusCode ? employee.ToEmployeeView() : null;
        }

        public IEnumerable<TeamView> GetTeamView(string managerId)
        {
            HttpResponseMessage response = _client.Value.GetAsync(String.Format("Employees/?managerId={0}", managerId)).Result;

            return response.IsSuccessStatusCode ? response.Content.ReadAsAsync<IEnumerable<TeamView>>().Result : null;
        }

        public void Dispose()
        {
            _client = null;
        }
    }
}