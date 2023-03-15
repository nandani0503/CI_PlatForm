using CI_Platform.Entities.Models;
using CI_Platform.Entities.ViewModel;
using CI_Platform.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CI_PlatForm.Controllers
{
    public class MissionController : Controller
    {

        private readonly IMissionRepository _MissionRepository;
        public MissionController(IMissionRepository MissionRepository)
        {
            _MissionRepository = MissionRepository;
        }

        //------------------Mission drop down---------------------------
        public IActionResult PlatformLanding()
        {

            ViewBag.sessionValue = HttpContext.Session.GetString("username");

            var CountryList = _MissionRepository.GetCountryData();
            ViewBag.countryList = CountryList;

            var MissionTheme = _MissionRepository.GetMissionTheme();
            ViewBag.missionTheme = MissionTheme;

            var SkillList = _MissionRepository.GetSkillsList();
            ViewBag.skillList = SkillList;

            var missions = _MissionRepository.GetMissionList();
            ViewBag.missions = missions;

            var totalMission = _MissionRepository.getTotalMission();
            ViewBag.totalMission = totalMission;



            //-------------------Mission card----------------------------------------------------------


            List<Card> missionCardDetails = _MissionRepository.GetMissionCard();

            ViewBag.CardDetail = missionCardDetails;
            return View();

        }

        [HttpPost]
        public JsonResult GetCity(List<string> countryId)
        {
            List<City> city = _MissionRepository.GetCityFromCountry(countryId);
            var json = JsonConvert.SerializeObject(city);

            return Json(json);
        }
        //--------------------------------------search bar-------------------------------------------
        [HttpPost]
        public ActionResult Search(string? search, string[] countries, string[] cities, string[] themes, string[] skills, int sort)
        {
            search = string.IsNullOrEmpty(search) ? "" : search.ToLower();
            List<Card> missions = _MissionRepository.GetMissionList(search, countries, cities, themes, skills, sort);
            ViewBag.cardDetail = missions;
           
            if (missions == null)
            {
                return PartialView("_MissionNotFound");
            }
           
            return PartialView("_gridView");
        }
        public ActionResult SearchList(string? search, string[] countries, string[] cities, string[] themes, string[] skills, int sort)
        {
            search = string.IsNullOrEmpty(search) ? "" : search.ToLower();
            List<Card> missions = _MissionRepository.GetMissionList(search, countries, cities, themes, skills, sort);
            ViewBag.cardDetail = missions;

            if (missions == null)
            {
                return PartialView("_MissionNotFound");
            }

            return PartialView("_listView");
        }

        public IActionResult MissionVolunteering()
        {
            return View();
        }
        //---------------------Mission not found --------------------------------------------------------------------------------

        /*public IActionResult MissionNotFound()
        {
            return View();
        }*/
    }
}        
   
        
           /*List<Country> countries = _MissionRepository.CountryList().ToList();
            ViewBag.countries=countries;
            List<City> cities = _MissionRepository.CityList().ToList();
            ViewBag.cities = cities;
           /*List<City> cities = _MissionRepository.GetCityFromCountry().ToList();
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
            public JsonResult GetCity(long countryId)
            {
                List<City> city = _MissionRepository.GetCityFromCountry(countryId);
                var json = JsonConvert.SerializeObject(city);

                return Json(json);
            }
            //------------------------------searchbar-----------------------------------------------------------------


            public ActionResult Search(string? search, string[] countries, string[] cities, string[] themes, string[] skills)
            {
                search = string.IsNullOrEmpty(search) ? "" : search.ToLower();
                List<Mission> missions = _MissionRepository.GetMissionList(search, countries, cities, themes, skills);
                List<Card> missionListingList = _MissionRepository.GetMissionCard(missions);

                ViewBag.cardDetail = missionListingList;

                return View();
            }

        }


    }*/

