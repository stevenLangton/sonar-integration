using System.Collections.Generic;
using JsPlc.Ssc.Link.Models;

namespace JsPlc.Ssc.Link.Interfaces.Services
{
    public interface IDomainTranslationService
    {
        string AdDomainToDbDomain(string colleagueEmail);
    }
}
