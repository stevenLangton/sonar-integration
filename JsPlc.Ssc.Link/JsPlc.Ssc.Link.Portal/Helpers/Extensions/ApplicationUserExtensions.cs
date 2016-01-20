using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Portal.Models;

namespace JsPlc.Ssc.Link.Portal.Helpers.Extensions
{
    public static class ApplicationUserExtensions
    {
        //public static UserView ToUserView(this ApplicationUser user)
        //{
        //    return new UserView
        //    {
        //        Id = user.Id,
        //        //ColleagueId = "",
        //        //FirstName = user.FirstName,
        //        //LastName = user.LastName, 
        //        IsLineManager = user.IsLineManager(),
        //        //EmailAddress = user.UserName,
            
        //    };
        //}

        public static ColleagueView ToColleagueView(this ColleagueView colleague)
        {
            throw new NotImplementedException();
            //return new ColleagueView
            //{
            //    LinkUserId = colleague.Id, FirstName = colleague.FirstName, LastName = colleague.LastName, 
            //    ColleagueId = colleague.ColleagueId, ManagerId = colleague.ManagerId, ManagerName = "", EmailAddress = colleague.EmailAddress
            //};
        }
    }
}