using Microsoft.AspNet.Http.Authentication;
using Microsoft.AspNet.Mvc;
using System.Threading.Tasks;

namespace WebApiWithAuth.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
