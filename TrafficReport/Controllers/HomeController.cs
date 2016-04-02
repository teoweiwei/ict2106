using System.Web.Mvc;

namespace TrafficReport.Controllers
{
    //This controller handles user request for the home page
    public class HomeController : Controller
    {
        //The home page
        public ActionResult Index()
        {
            return View();
        }
    }
}