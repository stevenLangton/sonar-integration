using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using JsPlc.Ssc.Link.Models;

namespace JsPlc.Ssc.Link.Portal.Helpers
{
    public class LinkServiceCaller
    {
        public static async Task<HttpResponseMessage> RunAsync(string meetingViewJson)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ServicesBaseUrl"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //// HTTP GET

                //HttpResponseMessage response = await client.GetAsync("api/Meetings");
                //if (response.IsSuccessStatusCode)
                //{
                //    MeetingView meetingView = await response.Content.ReadAsAsync<MeetingView>();
                //}

                // HTTP POST
                
                Trace.WriteLine("Sending meeting for create:" + meetingViewJson);
                var response = await client.PostAsJsonAsync("api/Meetings", meetingViewJson);
                if (!response.IsSuccessStatusCode)
                    return new HttpResponseMessage {StatusCode = HttpStatusCode.NotImplemented};
                
                //Uri meetingUrl = response.Headers.Location;

                //// HTTP PUT
                //meetingView.MeetingDate = DateTime.Now; // Update meetingDate
                //response = await client.PutAsJsonAsync(meetingUrl, meetingView);

                //// HTTP DELETE
                //response = await client.DeleteAsync(meetingUrl);
                return response;
            }
        }
    }
}