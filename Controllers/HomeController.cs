using System.Diagnostics;
using HeirWebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace HeirWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // create a session id
            SetSession("id", Guid.NewGuid().ToString());

            // Check if the user is authenticated (null-checked to clear the warnings)
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                // User is logged in, get their username
                var userName = User.Identity.Name ?? "guest";
                SetCookies("userName", userName);
                // create a session with username
                SetSession("username", userName);
            }
            else
            {
                // User is not logged in, set a cookie with "guest"
                SetCookies("userName", "guest");
                // create a session with username
                SetSession("username", "guest");
            }

            // Get the browser type
            SetCookies("broswerName", Request.Headers["User-Agent"].ToString());

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult SetCookies(string cookieName, string cookieValue)
        {
            CookieOptions options = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(15),  // Cookie expires in 15 days
                HttpOnly = true,                     // Prevent JavaScript access to the cookies
                Secure = true,                       // Use Secure flag (HTTPS only)
                SameSite = SameSiteMode.Strict       // Prevent CSRF attacks
            };
            Response.Cookies.Append(cookieName, cookieValue, options);
            return Ok("Cookies has been set.");
        }

        public IActionResult SetSession(string key, string value)
        {
            // Set session value
            HttpContext.Session.SetString(key, value);
            return RedirectToAction("Index");
        }

        public IActionResult About()
        {
            return View();
        }
    }
}
