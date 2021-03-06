﻿using System;
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
    public class LinkServiceCaller
    {
        public static async Task<HttpResponseMessage> RunAsync(string meetingViewJson, HttpMethod method)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var serviceUrl = String.Format("{0}api/Meetings", ConfigurationManager.AppSettings["ServicesBaseUrl"]);

                HttpRequestMessage request = new HttpRequestMessage(method, serviceUrl);

                request.Content = new StringContent( meetingViewJson,
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
                
                return response;
            }
        }
    }
}