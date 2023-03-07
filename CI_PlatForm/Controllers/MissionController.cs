using CI_Platform.Entities.Models;
using CI_Platform.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CI_PlatForm.Controllers
{
    public class MissionController : Controller
    {
        private readonly IUserRepository _UserRepository;
        public MissionController(IUserRepository UserRepository)
        {
            _UserRepository = UserRepository;
        }
       
        public IActionResult PlatformLanding()
        {
           
            ViewBag.sessionv = HttpContext.Session.GetString("username");
            List<Country> countries = _UserRepository.CountryList().ToList();
            ViewBag.countries=countries;
            List<City> cities = _UserRepository.CityList().ToList();
            ViewBag.cities = cities;
            List<MissionTheme> missionthemes = _UserRepository.MissionThemeList().ToList();
            ViewBag.missionthemes = missionthemes;
            List<Skill> skills = _UserRepository.SkillList().ToList();
            ViewBag.skills = skills;
            return View();
        }


    }
}
