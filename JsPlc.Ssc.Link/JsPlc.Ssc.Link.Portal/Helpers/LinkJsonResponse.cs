using System;
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
    public class LinkJsonData
    {
        public JsonStatusCode JsonStatusCode { get; set; }
        public object JsonObject { get; set; }
        public object ModelErrors { get; set; }
    }

    public class JsonStatusCode
    {
        public HttpStatusCode ApiStatusCode { get; set; }
        public string CustomStatusCode { get; set; } // extent to be enums
    }
}