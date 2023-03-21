using CI_Platform.Entities.Models;
using CI_Platform.Entities.ViewModel;
using CI_Platform.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CI_PlatForm.Controllers
{
    public class MissionController : Controller
    {

        public readonly IMissionRepository _MissionRepository;
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
            ViewBag.totalMission = missionCardDetails.Count();

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
        [HttpPost]
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

       public IActionResult MissionVolunteering(long id)
        {
            ViewBag.sessionValue = HttpContext.Session.GetString("username");
            List<Card> VolunteerCard = _MissionRepository.GetMissionCard();
            var missions = VolunteerCard.FirstOrDefault(i => i.MissionId == id);
            ViewBag.cardData = missions;
            ViewBag.commentViewBag = _MissionRepository.getComment(missions.MissionId);
            ViewBag.getSkill = _MissionRepository.GetSkillName(missions.MissionId);
            var user = HttpContext.Session.GetInt32("userId");
            ViewBag.userId = user;
            var RelatedMission = VolunteerCard.Where(a => a.MissionId != missions.MissionId && (a.CityId == missions.CityId || a.ThemeId == missions.ThemeId || a.MissionType == missions.MissionType)).Take(3).ToList();   
            ViewBag.relatedMission = RelatedMission;
            return View();
        }
        public void PostCommentInMission(string comment, long missionId)
        {
            long userId = Convert.ToInt64(HttpContext.Session.GetString("userId"));

            _MissionRepository.PostComment(comment, userId, missionId);
        }

        public void AddToRecentVolunteer(long missionId, long userId)
        {
            _MissionRepository.AddToRecent(missionId, userId);

        }
        public bool AddMissionToFav(int missionId)
        {
            long userId = Convert.ToInt64(HttpContext.Session.GetString("userId"));
            var fav = _MissionRepository.addToFavourite(missionId, userId);
            return fav;
        }
       
        //---------------------Mission not found --------------------------------------------------------------------------------

        /*public IActionResult MissionNotFound()
        {
            return View();
        }*/
    }
}        
   
        
