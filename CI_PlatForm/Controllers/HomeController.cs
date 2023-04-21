using CI_PlatForm.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CI_PlatForm.Controllers
{
    public class HomeController : Controller
    {
      

       

        public IActionResult Privacy()
        {
            ViewBag.sessionValue = HttpContext.Session.GetString("username");
            return View();
        }
        
       
      
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}