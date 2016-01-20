using System.Linq;
using System.Web.Mvc;
using JsPlc.Ssc.Link.StubService.StubRepository;

namespace JsPlc.Ssc.Link.StubService.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Display first employee for sanity check of StubService
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            using (var repo = new StubRepositoryContext())
            {
                var x = repo.Colleagues.FirstOrDefault();
            }

            return View();
        }
    }
}
