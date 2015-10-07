using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace JsPlc.Ssc.Link.Portal.Helpers.Extensions
{
    public static class ModelStateHelper
    {
        public static IEnumerable<KeyValuePair<string, string[]>> Errors(this ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                return modelState
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray())
                    .Where(m => m.Value.Any());
            }

            return null;
        }
    }
}