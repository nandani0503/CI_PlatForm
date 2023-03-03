using CI_PlatForm.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CI_PlatForm.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

       

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult MissionVolunteering()
        {
            return View();
        }
        public IActionResult VolunteeringStory()
        {
            return View();
        }
        public IActionResult PlatformLanding()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}