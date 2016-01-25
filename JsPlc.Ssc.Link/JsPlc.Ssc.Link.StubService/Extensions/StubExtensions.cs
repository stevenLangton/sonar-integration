using System;
using System.Collections.Generic;
using System.Linq;
using JsPlc.Ssc.Link.StubService.StubModels;

namespace JsPlc.Ssc.Link.StubService.Extensions
{
    public static class StubExtensions
    {
        public static ColleagueDto ToColleagueDto(this StubColleague stubColleague, StubColleague manager=null)
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
                HasManager = !(String.IsNullOrEmpty(stubColleague.ManagerId)),
                Manager = (manager==null) ? null : manager.ToColleagueDto()
            };
            return retval;
        }

        public static List<ColleagueDto> ToColleagueDtoList(this List<StubColleague> stubColleagues, StubColleague manager=null)
        {
            return stubColleagues.Select(c => c.ToColleagueDto(manager)).ToList();
        }
    }
}