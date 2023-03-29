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
        public IActionResult PlatformLanding(long id)
        {

            ViewBag.sessionValue = HttpContext.Session.GetString("username");
            long user = (long)Convert.ToInt64(HttpContext.Session.GetString("userId"));


            var CountryList = _MissionRepository.GetCountryData();
            ViewBag.countryList = CountryList;

            var MissionTheme = _MissionRepository.GetMissionTheme();
            ViewBag.missionTheme = MissionTheme;

            var SkillList = _MissionRepository.GetSkillsList();
            ViewBag.skillList = SkillList;

            /*var missions = _MissionRepository.GetMissionList();
            ViewBag.missions = missions;*/

            



            //-------------------Mission card----------------------------------------------------------


            List<Card> missionCardDetails = _MissionRepository.GetMissionCard(user);
            var DetailsOfWorker = _UserRepository.UserList().Where(i => i.UserId != user);
            ViewBag.userDetails = DetailsOfWorker;
            var coWorkerList = _MissionRepository.Recommend;

            ViewBag.totalMission = missionCardDetails.Count();

            ViewBag.CardDetail = missionCardDetails.Skip((1-1)*6).Take(6).ToList();

            //--------------------------Pagnation----------------------------------------

            ViewBag.pg_no = 1;
            ViewBag.Totalpages = Math.Ceiling(missionCardDetails.Count() / 6.0);
            ViewBag.missionCardDetails = missionCardDetails.Skip((1-1)*6).Take(6).ToList();
            ViewBag.pg_no = 1;
            return View();

        }

        /*[HttpPost]*/
        public JsonResult GetCity(List<string> countryId)
        {
            List<City> city = _MissionRepository.GetCityFromCountry(countryId);
            var json = JsonConvert.SerializeObject(city);

            return Json(json);
        }
        //--------------------------------------search bar-------------------------------------------
        [HttpPost]
        public ActionResult Search(string? search, string[] countries, string[] cities, string[] themes, string[] skills, int sort, int paging)
        {
            long user = (long)Convert.ToInt64(HttpContext.Session.GetString("userId"));
            var DetailsOfWorker = _UserRepository.UserList().Where(i => i.UserId == user);
            ViewBag.userDetails = DetailsOfWorker;
            search = string.IsNullOrEmpty(search) ? "" : search.ToLower();
            List<Card> missionCardDetails = _MissionRepository.GetMissionCard(user);
            List<Card> missions = _MissionRepository.GetMissionList(search, countries, cities, themes, skills, sort,paging, user);
            ViewBag.cardDetail = missions;
            ViewBag.pg_no = paging;
            ViewBag.TotalPages = Math.Ceiling(missionCardDetails.Count()/6.0);
           
            if (missions == null)
            {
                return PartialView("_MissionNotFound");
            }
           
            return PartialView("_gridView");
        }
        [HttpPost]
        public ActionResult SearchList(string? search, string[] countries, string[] cities, string[] themes, string[] skills, int sort, int paging)
        {
            long user = (long)Convert.ToInt64(HttpContext.Session.GetString("userId"));
            var DetailsOfWorker = _UserRepository.UserList().Where(i => i.UserId == user);
            ViewBag.userDetails = DetailsOfWorker;
            search = string.IsNullOrEmpty(search) ? "" : search.ToLower();
            List<Card> missionCardDetails = _MissionRepository.GetMissionCard(user);
            List<Card> missions = _MissionRepository.GetMissionList(search, countries, cities, themes, skills, sort, paging, user);
            ViewBag.cardDetail = missions;
            ViewBag.pg_no = paging;
            ViewBag.TotalPages = Math.Ceiling(missionCardDetails.Count()/6.0);

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
            long user = (long)Convert.ToInt64(HttpContext.Session.GetString("userId"));
            List<Card> VolunteerCard = _MissionRepository.GetMissionCard(user);
            
            var missions = VolunteerCard.FirstOrDefault(i => i.MissionId == id);
            ViewBag.cardData = missions;
            
            ViewBag.commentViewBag = _MissionRepository.getComment(missions.MissionId);
            
            ViewBag.getSkill = _MissionRepository.GetSkillName(missions.MissionId);



            /*var user = Convert.ToInt64(HttpContext.Session.GetString("userId"));
            ViewBag.userId = user;*/
            ViewBag.userId = user;
            ViewBag.pg_no = 0;
            
            var userDetails = _UserRepository.UserList().Where(i => i.UserId != user);
            ViewBag.userDetails = userDetails;

            ViewBag.getVolunteer = _MissionRepository.GetRecentUser(missions.MissionId).Take(3).ToList();
            ViewBag.Totalpages = Math.Ceiling(_MissionRepository.getMissionApplicant().Count() / 3.0);
            ViewBag.totalVol = _MissionRepository.GetRecentUser(missions.MissionId).Count();
            ViewBag.totalApplicant = _MissionRepository.getMissionApplicant().Count();

            ViewBag.checkFav = _MissionRepository.checkFavourite(id, user);
            ViewBag.checkApplied = _MissionRepository.checkApplied(id, user);
            var coWokerList = _MissionRepository.Recommend;

            var RelatedMission = VolunteerCard.Where(a => a.MissionId != missions.MissionId && (a.CityId == missions.CityId || a.ThemeId == missions.ThemeId || a.MissionType == missions.MissionType)).Take(3).ToList();   
            ViewBag.relatedMission = RelatedMission;
            
            return View();
        }
        public JsonResult VolunteerList(List<long> Volunteers, long MissionId)
        {
            long userId = Convert.ToInt64(HttpContext.Session.GetString("userId"));
            var voluntees = _MissionRepository.Recommend(userId, MissionId, Volunteers);
            return Json(voluntees);
        }
        public void PostCommentInMission(string comment, long missionId)
        {
            long userId = Convert.ToInt64(HttpContext.Session.GetString("userId"));

            _MissionRepository.PostComment(comment, userId, missionId);
        }
        public bool AddMissionToFav(int missionId)
        {
            long userId = Convert.ToInt64(HttpContext.Session.GetString("userId"));
            var fav = _MissionRepository.addToFavourite(missionId, userId);
            return fav;
        }
        public void AddToRecentVolunteer(long missionId, long userId)
        {
            bool check = _MissionRepository.checkApplied(missionId, userId);

            if (check != true)
            {
                _MissionRepository.AddToRecent(missionId, userId);
            }
        }
        public ActionResult VolunteerPaging(int paging, long mission_id)
        {
            if(paging > 0)
            {
                ViewBag.getVolunteer = _MissionRepository.GetRecentUser(mission_id).Skip(9 * paging).Take(9).ToList();
                ViewBag.totalVol = _MissionRepository.GetRecentUser(mission_id).Count();
                ViewBag.totalApplicant = _MissionRepository.getMissionApplicant().Count();
                ViewBag.pg_no = paging;
                return PartialView("_RecentVolunteer");
            }
            else
            {
                ViewBag.getVolunteer = _MissionRepository.GetRecentUser(mission_id).Skip(9 * paging).Take(9).ToList();
                ViewBag.totalVol = _MissionRepository.GetRecentUser(mission_id).Count();
                ViewBag.totalApplicant = _MissionRepository.getMissionApplicant().Count();
                ViewBag.pg_no = paging;
                return PartialView("_RecentVolunteer");
            }
        }

        /* ------------------------Volunteer story---------------------------------------------------*/
        public IActionResult VolunteeringStory()
        {
            ViewBag.sessionValue = HttpContext.Session.GetString("username");

           

            /*var MissionTheme = _MissionRepository.GetMissionTheme();
            ViewBag.missionTheme = MissionTheme;

            var SkillList = _MissionRepository.GetSkillsList();
            ViewBag.skillList = SkillList;*/

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
        public IActionResult StoryDetail(long id)
        {
            ViewBag.sessionValue = HttpContext.Session.GetString("username");
           

            return View();
        }
        public IActionResult UserDetail()
        {
            ViewBag.sessionValue = HttpContext.Session.GetString("username");


            var CountryList = _MissionRepository.GetCountryData();
            ViewBag.countryList = CountryList;
            return View();
        }
    }
}        
   
        
