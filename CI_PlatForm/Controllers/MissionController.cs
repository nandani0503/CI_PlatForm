using CI_Platform.Entities.Models;
using CI_Platform.Entities.ViewModel;
using CI_Platform.Repository.Interface;
using CI_PlatForm.Entities.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CI_PlatForm.Controllers
{
    public class MissionController : Controller
    {

        public readonly IMissionRepository _MissionRepository;
        public readonly IUserRepository _UserRepository;
        public MissionController(IMissionRepository MissionRepository, IUserRepository userRepository)
        {
            _MissionRepository = MissionRepository;
            _UserRepository = userRepository;
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
    /*----------------------------------Volunteer mission-----------------------------------------------------------------------------*/
       public IActionResult MissionVolunteering(long id)
        {
            ViewBag.sessionValue = HttpContext.Session.GetString("username");
            
            List<Card> VolunteerCard = _MissionRepository.GetMissionCard();
            
            var missions = VolunteerCard.FirstOrDefault(i => i.MissionId == id);
            ViewBag.cardData = missions;
            
            ViewBag.commentViewBag = _MissionRepository.getComment(missions.MissionId);
            
            ViewBag.getSkill = _MissionRepository.GetSkillName(missions.MissionId);

            

            var user = Convert.ToInt64(HttpContext.Session.GetString("userId"));
            ViewBag.userId = user;
            
            var userDetails = _UserRepository.UserList().Where(i => i.UserId != user);
            ViewBag.userDetails = userDetails;

            ViewBag.getVolunteer = _MissionRepository.GetRecentUser(missions.MissionId);
            ViewBag.totalVol = _MissionRepository.GetRecentUser(missions.MissionId).Count();
            ViewBag.totalApplicant = _MissionRepository.getMissionApplicant().Count();

            var RelatedMission = VolunteerCard.Where(a => a.MissionId != missions.MissionId && (a.CityId == missions.CityId || a.ThemeId == missions.ThemeId || a.MissionType == missions.MissionType)).Take(3).ToList();   
            ViewBag.relatedMission = RelatedMission;
            
            return View();
        }
        public void PostCommentInMission(string comment, long missionId)
        {
            long userId = Convert.ToInt64(HttpContext.Session.GetString("userId"));

            _MissionRepository.PostComment(comment, userId, missionId);
        }

        public void AddToRecentVolunteer(long missionId)
        {
            long userId = Convert.ToInt64(HttpContext.Session.GetString("userId"));
            _MissionRepository.AddToRecent(missionId, userId);

        }
        public bool AddMissionToFav(int missionId)
        {
            long userId = Convert.ToInt64(HttpContext.Session.GetString("userId"));
            var fav = _MissionRepository.addToFavourite(missionId, userId);
            return fav;
        }
       public void VolunteerList(List<long> Volunteers, long MissionId)
        {
            long userId = Convert.ToInt64(HttpContext.Session.GetString("userId"));
            var voluntees = _MissionRepository.Recommend(userId, MissionId, Volunteers);

        }
        /* ------------------------Volunteer story---------------------------------------------------*/
        public IActionResult VolunteeringStory()
        {
            ViewBag.sessionValue = HttpContext.Session.GetString("username");

           

            var MissionTheme = _MissionRepository.GetMissionTheme();
            ViewBag.missionTheme = MissionTheme;

            var SkillList = _MissionRepository.GetSkillsList();
            ViewBag.skillList = SkillList;

            var storyInfo = _MissionRepository.GetStoryDetails();
            ViewBag.storyInfo = storyInfo;
            
            return View();
        }
      
       [HttpPost]
       public ActionResult SearchStory(string? searchStory)
        {
            searchStory = string.IsNullOrEmpty(searchStory) ? "" : searchStory.ToLower();
            List<StoryViewModel> storyInfo = _MissionRepository.GetStoryData(searchStory);
            ViewBag.storyInfo = storyInfo;
            return View("_storyCard");
            if(storyInfo == null)
            {
                return View("_MissionNotFound");
            }
        }


        


        //---------------------Mission not found --------------------------------------------------------------------------------

       
        public IActionResult ShareStory()
        {
            ViewBag.sessionValue = HttpContext.Session.GetString("username");
            

            var missionStory = _MissionRepository.GetStoryList();
            ViewBag.missionStory = missionStory;
            return View();
        }
    }
}        
   
        
