using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Mission08_Team0111.Models;

namespace Mission08_Team0111.Controllers
{
    public class HomeController : Controller
    {
        // Redirect root / to Tasks Index (Quadrants view)
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Tasks");
        }

        // Keep privacy page intact
        public IActionResult Privacy()
        {
            return View();
        }

        // Default error handler (unchanged)
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}