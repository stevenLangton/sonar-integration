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

        public static EmployeeView ToEmployeeView(this Employee employee)
        {
            return new EmployeeView
            {
                Id = employee.Id, FirstName = employee.FirstName, LastName = employee.LastName, 
                ColleagueId = employee.ColleagueId, ManagerId = employee.ManagerId, ManagerName = "", EmailAddress = employee.EmailAddress
            };
        }
    }
}