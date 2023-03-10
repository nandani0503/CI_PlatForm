using CI_Platform.Entities.Models;
using CI_Platform.Entities.ViewModel;
using CI_Platform.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CI_PlatForm.Controllers
{
    public class MissionController : Controller
    {
        private readonly IMissionRepository _MissionRepository;
        public MissionController(IMissionRepository MissionRepository)
        {
            _MissionRepository = MissionRepository;
        }
       
        public IActionResult PlatformLanding()
        {
           
            ViewBag.sessionv = HttpContext.Session.GetString("username");
            List<Country> countries = _MissionRepository.CountryList().ToList();
            ViewBag.countries=countries;
            List<City> cities = _MissionRepository.CityList().ToList();
            ViewBag.cities = cities;
            List<MissionTheme> missionthemes = _MissionRepository.MissionThemeList().ToList();
            ViewBag.missionthemes = missionthemes;
            List<Skill> skills = _MissionRepository.SkillList().ToList();
            ViewBag.skills = skills;

            List<Mission> missionDetail = _MissionRepository.GetMissionList();
            List<Card> missionAllDetails = new List<Card>();
            foreach (var allDetailsCard in missionDetail)
            {
                Card cardDetails = new Card();

                cardDetails.Theme = _MissionRepository.getThemeTitle(allDetailsCard.ThemeId);
                cardDetails.Title = allDetailsCard.Title;
                cardDetails.MissionId = allDetailsCard.MissionId;
                cardDetails.CityName = _MissionRepository.getCity(allDetailsCard.CityId);
                cardDetails.ShortDescription = allDetailsCard.ShortDescription;
                cardDetails.OrganizationName = allDetailsCard.OrganizationName;
                cardDetails.StartDate = allDetailsCard.StartDate;
                cardDetails.EndDate = allDetailsCard.EndDate;
                cardDetails.MediaName = _MissionRepository.getMediaName(allDetailsCard.MissionId);
               cardDetails.Rating = _MissionRepository.getMissionRating(allDetailsCard.MissionId);
                
                missionAllDetails.Add(cardDetails);
            }

            ViewBag.cardDetail = missionAllDetails;

            return View();
        }


    }
}
