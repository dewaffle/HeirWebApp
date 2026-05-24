using Microsoft.AspNetCore.Mvc;

namespace HeirWebApp.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}