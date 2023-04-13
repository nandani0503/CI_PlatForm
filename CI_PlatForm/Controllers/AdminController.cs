using Microsoft.AspNetCore.Mvc;

namespace CI_PlatForm.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult UserCRUD()
        {
            return View();
        }
        public IActionResult CMSPage()
        {
            return PartialView("_CMSPage");
        }
    }
}
