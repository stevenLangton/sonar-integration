using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using JsPlc.Ssc.Link.Models;

namespace JsPlc.Ssc.Link.Portal.ModelBinding
{
    // http://stackoverflow.com/questions/21923656/net-mvc-4-http-post-data-as-json-object
    public class MeetingViewModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            MeetingView model= null;

            if (controllerContext.RequestContext.HttpContext.Request.AcceptTypes != null && 
                (controllerContext.RequestContext.HttpContext.Request != null && 
                (controllerContext.RequestContext.HttpContext != null && 
                (controllerContext.RequestContext != null && 
                controllerContext.RequestContext.HttpContext.Request.AcceptTypes.Contains("application/json")))))
            {
                var serializer = new JavaScriptSerializer();
                var form = controllerContext.RequestContext.HttpContext.Request.Form.ToString();
                var urlDecodedForm = HttpUtility.UrlDecode(form);
                if (urlDecodedForm != null)
                {
                    model = serializer.Deserialize<MeetingView>(urlDecodedForm);
                } 
            }
            else
            {
                model = (MeetingView)ModelBinders.Binders.DefaultBinder.BindModel(controllerContext, bindingContext);
            }

            return model;
        }

    }
}