using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
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
            var username = UserName; // what's the logged in User's name or other props.
            if (username.ToLower().Contains("sandip"))
            {
                return true;
            }
            return false; // custom code to check this application role against LinkRepository
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
            const String fixedPasswordHash = "AK0SK99vnma/V/KcDnsuNkkI99KlUdwct5lMTGWfEd75eRCxY8gwHo0xskATZ6rZCA==";
            
            //context.Users.Add(new ApplicationUser { 
            //    Id = "1",
            //    UserName = "Parveen.Kumar@sainsburys.co.uk", 
            //    Email = "Parveen.Kumar@sainsburys.co.uk",
            //    PasswordHash = fixedPasswordHash, 
            //    SecurityStamp = "1fd86dec-b433-42ce-ab76-fe7202995890", LockoutEnabled = true
            //});
            //context.Users.Add(new ApplicationUser
            //{
            //    Id="2",
            //    UserName = "Steven.Farkas@sainsburys.co.uk",
            //    Email = "Steven.Farkas@sainsburys.co.uk",
            //    PasswordHash = fixedPasswordHash
            //});
            //context.Users.Add(new ApplicationUser
            //{
            //    Id="3",
            //    UserName = "Vasundhara.B@sainsburys.co.uk",
            //    Email = "Vasundhara.B@sainsburys.co.uk",
            //    PasswordHash = fixedPasswordHash
            //});
            //context.Users.Add(new ApplicationUser
            //{
            //    Id = "4",
            //    UserName = "Luan.Au@sainsburys.co.uk",
            //    Email = "Luan.Au@sainsburys.co.uk",
            //    PasswordHash = fixedPasswordHash
            //});
            //context.Users.Add(new ApplicationUser
            //{
            //    Id = "5",
            //    UserName = "Sandip.Vaidya@sainsburys.co.uk",
            //    Email = "Sandip.Vaidya@sainsburys.co.uk",
            //    PasswordHash = fixedPasswordHash
            //});
            //context.Users.Add(new ApplicationUser
            //{
            //    Id = "6",
            //    UserName = "Sunna.Syed@sainsburys.co.uk",
            //    Email = "Sunna.Syed@sainsburys.co.uk",
            //    PasswordHash = fixedPasswordHash
            //});
            context.SaveChanges();
        }
    }
}