using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using JsPlc.Ssc.Link.Portal.Helpers.Api;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace JsPlc.Ssc.Link.Portal.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public bool IsLineManager()
        {
            //if (username.ToLower().Contains("sandip"))
            //{
            //    return true;
            //}
            //return false; // TODO custom code to check this application role against LinkRepository

            using (var facade = new LinkServiceFacade())
            {
                var username = UserName; // what's the logged in User's name or other props.
                return facade.IsManager(username);
            }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
           
            //var currentUser = manager.FindById(userIdentity.GetUserId());

            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer(new UserStoreDbInit()); // NEW Strategy
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
    
    // NEW Strategy to Drop and Create 
    internal class UserStoreDbInit : IDatabaseInitializer<ApplicationDbContext>
    {
        public void InitializeDatabase(ApplicationDbContext context)
        {
            var x = new UserStoreSeed();
            x.InitializeDatabase(context);
        }
    }

    // Always drop and create DB
    internal class UserStoreSeed : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        // any seed code here
        protected override void Seed(ApplicationDbContext context)
        {
            base.Seed(context);
            // possibly register users here.

            context.SaveChanges();
        }
    }
}