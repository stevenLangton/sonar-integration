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
using JsPlc.Ssc.Link.Portal.Models;
using JsPlc.Ssc.Link.Models.Entities;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;

namespace JsPlc.Ssc.Link.Portal
{
    public class LinkServiceFacade : IDisposable
    {
        private Lazy<HttpClient> _client;

        public LinkServiceFacade()
        {
            _client = new Lazy<HttpClient>();
            //_client.Value.BaseAddress = new Uri("http://localhost/JsPlc.Ssc.Link.Service/api/");
            _client.Value.BaseAddress = new Uri(ConfigurationManager.AppSettings["ServicesBaseUrl"] + ""); // "not needing api/"
            
            _client.Value.DefaultRequestHeaders.Accept.Clear();
            _client.Value.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        #region User api - api to provide info related to the logged in user

        public ColleagueView GetUserDetails(string Email)
        {
            // api/employees/?emailaddress=vasundhara.b@sainsburys.co.uk
            HttpResponseMessage response = _client.Value.GetAsync("api/employees/?emailaddress=" + Email).Result;
            var item = response.Content.ReadAsAsync<ColleagueView>().Result;
            return response.IsSuccessStatusCode ? item : null;
        }

        public bool IsManager(string username)
            {
            HttpResponseMessage response = _client.Value.GetAsync("api/Employees/?UserName=" + username).Result;

            return response.IsSuccessStatusCode && response.Content.ReadAsAsync<bool>().Result;
            }

        #endregion

        #region Link Meeting api - api to manage Link meetings

        public MeetingView GetMeeting(int Id)
        {
            HttpResponseMessage response = _client.Value.GetAsync("api/meetings/" + Id.ToString(CultureInfo.InvariantCulture)).Result;
            var meeting = response.Content.ReadAsAsync<MeetingView>().Result;
            return response.IsSuccessStatusCode ? meeting : null;
        }

        public MeetingView GetNewMeetingView(string colleagueId)
        {
            HttpResponseMessage response = _client.Value.GetAsync("newmeeting/" + colleagueId).Result;

            return response.IsSuccessStatusCode ? response.Content.ReadAsAsync<MeetingView>().Result : null;
        }

        /// <summary>
        /// Uses COLLEAGUE Profile Services
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool IsManagerByEmail(string email)
        {
            HttpResponseMessage response = _client.Value.GetAsync("api/IsManagerByEmail/" + email).Result;

            return response.IsSuccessStatusCode && response.Content.ReadAsAsync<bool>().Result;
        }

        /// <summary>
        /// Uses COLLEAGUE Profile Services
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public ColleagueView GetColleagueByUsername(string email)
        {
            HttpResponseMessage response = _client.Value.GetAsync("api/ColleagueByEmail/" + email).Result;
            var colleague = response.Content.ReadAsAsync<ColleagueView>().Result;

            return response.IsSuccessStatusCode ? colleague : null;
        }

        public IEnumerable<ColleagueTeamView> GetTeamView(string managerId)
        {
            var apiPath = String.Format("myteam/{0}", managerId);
            HttpResponseMessage response = _client.Value.GetAsync(apiPath).Result;

            return response.IsSuccessStatusCode ? response.Content.ReadAsAsync<IEnumerable<ColleagueTeamView>>().Result : null;
        }

        public ColleagueTeamView GetMyMeetingsView(string colleagueId)
        {
            var apiPath = String.Format("mymeetings/{0}", colleagueId);
            HttpResponseMessage response = _client.Value.GetAsync(apiPath).Result;

            return response.IsSuccessStatusCode ? response.Content.ReadAsAsync<ColleagueTeamView>().Result : null;
        }
        #endregion

        #region Objectives api

        public async Task<int> CreateObjective(LinkObjective modified)
        {
            var jsonString = JsonConvert.SerializeObject(modified);

            var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            string pathSuffix = "colleagues/" + modified.ColleagueId + "/objectives/";

            HttpResponseMessage response = _client.Value.PostAsync(pathSuffix, httpContent).Result;

            //Return new object id if fine else 0
            if (response.IsSuccessStatusCode)
                return int.Parse(response.Headers.Location.LocalPath.Split('/').Last());
            else
                return 0; //Failed to create object.
        }

        public LinkObjective GetObjective(string ColleagueId, int ObjectiveId)
        {
            HttpResponseMessage response = _client.Value.GetAsync("colleagues/" + ColleagueId + "/objectives/" + ObjectiveId.ToString(CultureInfo.InvariantCulture)).Result;
            var objective = response.Content.ReadAsAsync<LinkObjective>().Result;
            return response.IsSuccessStatusCode ? objective : null;
        }

        public async Task<bool> UpdateObjective(LinkObjective modified)
        {
            var jsonString = JsonConvert.SerializeObject(modified);

            var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            string pathSuffix  = "colleagues/" + modified.ColleagueId + "/objectives/" + modified.Id.ToString(CultureInfo.InvariantCulture);

            HttpResponseMessage response = _client.Value.PutAsync(pathSuffix, httpContent).Result;
            return response.IsSuccessStatusCode ? true : false;
        }
        #endregion

        public void Dispose()
        {
            _client = null;
        }
    }
}