using System.Globalization;
using JsPlc.Ssc.Link.Interfaces.Services;
using JsPlc.Ssc.Link.Models;
//using JsPlc.Ssc.Link.Portal.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using JsPlc.Ssc.Link.Portal;
using JsPlc.Ssc.Link.Portal.Helpers.Extensions;
using JsPlc.Ssc.Link.Portal.Models;
using JsPlc.Ssc.Link.Models.Entities;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace JsPlc.Ssc.Link.Portal
{
    public class LinkServiceFacade : IDisposable, ILinkServiceFacade
    {
        private Lazy<HttpClient> _client;

		private static readonly ILog _log = LogManager.GetLogger("GlobalActionExecutedEx");

        public LinkServiceFacade(Lazy<HttpClient> client = null, IConfigurationDataService configurationDataService = null)
        {
            _client = client ?? new Lazy<HttpClient>();
            //_client.Value.BaseAddress = new Uri("http://localhost/JsPlc.Ssc.Link.Service/api/");
            //_client.Value.BaseAddress = new Uri(ConfigurationManager.AppSettings["ServicesBaseUrl"] + ""); // "not needing api/"
            var url = (configurationDataService == null)
                ? ConfigurationManager.AppSettings["ServicesBaseUrl"] + ""
                : configurationDataService.GetConfigSettingValue("ServicesBaseUrl");
            _client.Value.BaseAddress = new Uri(url); 
            
            _client.Value.DefaultRequestHeaders.Accept.Clear();
            _client.Value.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        #region "Security api - Check various access types based on use cases"
        public bool HasMeetingAccess(int meetingId, string colleagueId)
        {
            var filter = String.Format("{0}/{1}", meetingId, colleagueId);

            HttpResponseMessage response = _client.Value.GetAsync("api/HasMeetingAccess/" + filter).Result;

            return response.IsSuccessStatusCode && response.Content.ReadAsAsync<bool>().Result;
        }
        public bool HasColleagueAccess(string colleagueId, string otherColleagueId)
        {
            var filter = String.Format("{0}/{1}", colleagueId, otherColleagueId);

            HttpResponseMessage response = _client.Value.GetAsync("api/HasColleagueAccess/" + filter).Result;

            return response.IsSuccessStatusCode && response.Content.ReadAsAsync<bool>().Result;
        }


        #endregion "security api"

        #region User api - api to provide info related to the logged in user

        public ColleagueView GetUserDetails(string Email)
        {
            // api/employees/?emailaddress=vasundhara.b@sainsburys.co.uk
            HttpResponseMessage response = _client.Value.GetAsync("api/employees/?emailaddress=" + Email).Result;
            var item = response.Content.ReadAsAsync<ColleagueView>().Result;
            return response.IsSuccessStatusCode ? item : null;
        }

        //public bool IsManager(string username)
        //    {
        //    HttpResponseMessage response = _client.Value.GetAsync("api/Employees/?UserName=" + username).Result;

        //    return response.IsSuccessStatusCode && response.Content.ReadAsAsync<bool>().Result;
        //    }

        #endregion

        #region Link Meeting api - api to manage Link meetings

        public MeetingView GetMeeting(int Id)
        {
            HttpResponseMessage response = _client.Value.GetAsync("api/meetings/" + Id.ToString(CultureInfo.InvariantCulture)).Result;
            var meeting = response.Content.ReadAsAsync<MeetingView>().Result;
            return response.IsSuccessStatusCode ? meeting : null;
        }

        public async Task<MeetingView> UnshareMeeting(int meetingId)
        {
            HttpResponseMessage response = await _client.Value.GetAsync("api/unshareMeeting/" + meetingId.ToString(CultureInfo.InvariantCulture));
            MeetingView meeting = await response.Content.ReadAsAsync<MeetingView>();
            return response.IsSuccessStatusCode ? meeting : null;
        }

        public async Task<MeetingView> ApproveMeeting(int meetingId)
        {
            HttpResponseMessage response = await _client.Value.GetAsync("api/ApproveMeeting/" + meetingId.ToString(CultureInfo.InvariantCulture));
            MeetingView meeting = await response.Content.ReadAsAsync<MeetingView>();
            return response.IsSuccessStatusCode ? meeting : null;
        }
        public MeetingView GetNewMeetingView(string colleagueId)
        {
            HttpResponseMessage response = _client.Value.GetAsync("newmeeting/" + colleagueId).Result;

            return response.IsSuccessStatusCode ? response.Content.ReadAsAsync<MeetingView>().Result : null;
        }

        /// <summary>
        /// Get the next meeting in the future for a colleague
        /// </summary>
        /// <param name="colleagueId">Sainsburys colleague id</param>
        /// <returns></returns>
        public LinkMeeting GetNextMeeting(string colleagueId)
        {
            HttpResponseMessage response = _client.Value.GetAsync("mymeetings/" + colleagueId + "/NextInFuture").Result;

            return response.IsSuccessStatusCode ? response.Content.ReadAsAsync<LinkMeeting>().Result : null;
        }

        #endregion

        #region Colleague information api

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
        /// Get colleague details given the colleague id
        /// </summary>
        /// <param name="ColleagueId">Sainsburys colleague id</param>
        /// <returns></returns>
        public ColleagueView GetColleague(string ColleagueId)
        {
            HttpResponseMessage response = _client.Value.GetAsync("api/Colleague/" + ColleagueId).Result;
            var colleague = response.Content.ReadAsAsync<ColleagueView>().Result;

            return response.IsSuccessStatusCode ? colleague : null;
        }

        /// <summary>
        /// Uses COLLEAGUE Profile Services
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public ColleagueView GetColleagueByUsername(string email)
        {
            HttpResponseMessage response = _client.Value.GetAsync("api/ColleagueByEmail/" + email).Result;

			_log.WarnFormat("LinkServiceFacade Content. Email: {0}. Response contet: {1}",
						email,
						response.Content.ReadAsStringAsync().Result);

			_log.WarnFormat("LinkServiceFacade Everything Else. Email: {0}. Response status code: {1}. Reason phrase: {2}. All {3}",
					email,
					response.StatusCode,
					response.ReasonPhrase,
					response.ToString());

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

            HttpResponseMessage response = await _client.Value.PostAsync(pathSuffix, httpContent);

            //Return new object id if fine else 0
            if (response.IsSuccessStatusCode)
                return int.Parse(response.Headers.Location.LocalPath.Split('/').Last());
            else
                return 0; //Failed to create object.
        }

        public async Task<LinkObjective> GetObjective(string colleagueId, int objectiveId)
        {
            HttpResponseMessage response = await _client.Value.GetAsync("colleagues/" + colleagueId + "/objectives/" + objectiveId.ToString(CultureInfo.InvariantCulture));
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

        /// <summary>
        /// Get a list of all objectives for a colleague
        /// </summary>
        /// <param name="colleagueId">Sainsburys colleague id</param>
        /// <returns></returns>
        public async Task<List<LinkObjective>> GetObjectivesList(string colleagueId)
        {
            HttpResponseMessage response = await _client.Value.GetAsync("colleagues/" + colleagueId + "/objectives");

            List<LinkObjective> objectivesList=null;
            if (response.IsSuccessStatusCode)
            {
                objectivesList = response.Content.ReadAsAsync<List<LinkObjective>>().Result;
            }
           
            return objectivesList;
        }

        /// <summary>
        /// Get objectives which a colleague has shared
        /// </summary>
        /// <param name="colleagueId"></param>
        /// <returns>A list of shared objectives</returns>
        public async Task<List<LinkObjective>> GetSharedObjectives(string colleagueId)
        {
            HttpResponseMessage response = await _client.Value.GetAsync("colleagues/" + colleagueId + "/objectives/shared");

            List<LinkObjective> objectivesList = null;
            if (response.IsSuccessStatusCode)
            {
                objectivesList = response.Content.ReadAsAsync<List<LinkObjective>>().Result;
            }

            return objectivesList;
        }

        public List<string> GetApiServiceKeys()
        {
            HttpResponseMessage response = _client.Value.GetAsync("api/ShowKeys").Result;

            var keyValues = response.Content.ReadAsAsync<List<string>>().Result;

            return keyValues;
        }

        
        #endregion

        #region PDP

        public async Task<LinkPdp> UpdatePdp(LinkPdp modified)
        {
            var jsonString = JsonConvert.SerializeObject(modified);

            var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            string pathSuffix = "colleagues/" + modified.ColleagueId + "/pdp/";

            HttpResponseMessage response = _client.Value.PutAsync(pathSuffix, httpContent).Result;
            LinkPdp pdp = null;
            if (response.IsSuccessStatusCode)
            {
                pdp = response.Content.ReadAsAsync<LinkPdp>().Result;
            }
            return pdp;
        }

        public LinkPdp GetPdp(string ColleagueId)
        {
            HttpResponseMessage response = _client.Value.GetAsync("colleagues/" + ColleagueId + "/pdp/").Result;
            var pdp = response.Content.ReadAsAsync<LinkPdp>().Result;
            return response.IsSuccessStatusCode ? pdp : null;
        }

        #endregion

        public void Dispose()
        {
            _client = null;
        }
    }
}