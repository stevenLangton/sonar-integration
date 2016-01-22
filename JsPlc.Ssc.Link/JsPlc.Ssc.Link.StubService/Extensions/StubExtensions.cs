using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JsPlc.Ssc.Link.StubService.StubModels;
using Microsoft.Owin.Security.Provider;

namespace JsPlc.Ssc.Link.StubService.Extensions
{
    public static class StubExtensions
    {
        public static ColleagueDto ToColleagueDto(this StubColleague stubColleague)
        {
            var retval = new ColleagueDto
            {
                ColleagueId = stubColleague.ColleagueId,
                Department = stubColleague.Department,
                Division = stubColleague.Division,
                EmailAddress = stubColleague.EmailAddress,
                FirstName = stubColleague.FirstName,
                LastName = stubColleague.LastName,
                Grade = stubColleague.Grade,
                KnownAsName = stubColleague.KnownAsName,
                ManagerId = stubColleague.ManagerId,
                HasManager = !(String.IsNullOrEmpty(stubColleague.ManagerId))
            };
            return retval;
        }

        public static List<ColleagueDto> ToColleagueDtoList(this List<StubColleague> stubColleagues)
        {
            return stubColleagues.Select(c => c.ToColleagueDto()).ToList();
        }
    }
}