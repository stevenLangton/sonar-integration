using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using JsPlc.Ssc.Link.Models;

namespace JsPlc.Ssc.Link.Portal.Helpers
{
    // TODO use facade method and retire this code.. Currently used for Create Meeting - POST to Api.
    public class LinkServiceCaller
    {
        public static async Task<HttpResponseMessage> RunAsync(string meetingViewJson)
        {
            using (var client = new HttpClient())
            {
                //client.BaseAddress = new Uri();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //// HTTP GET

                //HttpResponseMessage response = await client.GetAsync("api/Meetings");
                //if (response.IsSuccessStatusCode)
                //{
                //    MeetingView meetingView = await response.Content.ReadAsAsync<MeetingView>();
                //}

                // HTTP POST
                var serviceUrl = String.Format("{0}api/Meetings", ConfigurationManager.AppSettings["ServicesBaseUrl"]);

                // serviceUrl = serviceUrl.Replace("//localhost", "//" + Environment.MachineName);

                Trace.WriteLine("serviceUrl:" + serviceUrl);
                Trace.WriteLine("Sending meeting for create:" + meetingViewJson);

                ////var response = client.PostAsJsonAsync(serviceUrl, meetingViewJson).Result;
                //var response = client.PostAsync(new Uri(serviceUrl), content).Result;

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, serviceUrl);
               
                request.Content = new StringContent(meetingViewJson,
                                                    Encoding.UTF8,
                                                    "application/json");

                var response = await client.SendAsync(request)
                    .ContinueWith(responseTask =>
                    {
                        Console.WriteLine("Response: {0}", responseTask.Result);
                        return responseTask.Result;
                    });

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