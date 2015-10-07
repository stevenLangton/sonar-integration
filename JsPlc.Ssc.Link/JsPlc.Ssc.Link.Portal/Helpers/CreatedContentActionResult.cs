﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace JsPlc.Ssc.Link.Portal.Helpers
{
    public class CreatedContentActionResult : IHttpActionResult
    {
        private readonly HttpRequestMessage _request;
        private readonly string _location;

        public CreatedContentActionResult(HttpRequestMessage request, string location)
        {
            _request = request;
            _location = location;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = _request.CreateResponse(HttpStatusCode.Created);
            response.Headers.Location = new Uri(_location);
            return Task.FromResult(response);
        }
    }
}