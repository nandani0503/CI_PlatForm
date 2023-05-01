using CI_PlatForm.Entities.ViewModel;
using CI_PlatForm.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CI_PlatForm.Controllers
{
    public class AdminController : Controller
    {
        public readonly IAdminRepository _adminRepository;
        public AdminController(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }
        public IActionResult UserCRUD()
        {
            var admin = _adminRepository.getAdminList();
            ViewBag.pg_no = 1;
          
            ViewBag.TotalPages = Math.Ceiling(admin.UserList.Count() / 10.00);
            ViewBag.TpCms = Math.Ceiling(admin.CmsList.Count() / 10.00);
            ViewBag.TpStory = Math.Ceiling(admin.StoryList.Count() / 10.00);
            ViewBag.TpApplication = Math.Ceiling(admin.MissionApplicationList.Count() / 10.00);
            ViewBag.TpSkill = Math.Ceiling(admin.MissionSkillList.Count() / 10.00);

            admin.UserList = admin.UserList.Take(10).ToList();
            admin.ThemeList = admin.ThemeList.Take(3).ToList();
            admin.MissionList = admin.MissionList.Take(3).ToList();
            admin.StoryList = admin.StoryList.Take(10).ToList();
            admin.MissionApplicationList = admin.MissionApplicationList.Take(10).ToList();
            admin.MissionSkillList = admin.MissionSkillList.Take(10).ToList();
            return View(admin);
        }
       
       public ActionResult Search(string? search, int pg, string who)
        {
            search = string.IsNullOrEmpty(search) ? "" : search.ToLower();
            AdminViewModel adminView = _adminRepository.getAdminList();
            
            if (who == "user")
            {
                AdminViewModel users = _adminRepository.getUserDetail(search, pg);
                ViewBag.pg_no = pg;
                ViewBag.TotalPages = Math.Ceiling(adminView.UserList.Count() / 10.0);
                return View("_UserCRUD", users);
            }
            if(who == "theme")
            {
                AdminViewModel themes = _adminRepository.getThemeDetail(search, pg);
                ViewBag.pg_no = pg;
                ViewBag.TotalPages = Math.Ceiling(adminView.ThemeList.Count() / 3.0);
                return View("_MissionTheme", themes);
            }
            if(who == "mission")
            {
                AdminViewModel missions = _adminRepository.getMissionDetail(search, pg);
                ViewBag.pg_no = pg;
                ViewBag.TotalPages = Math.Ceiling(adminView.MissionList.Count() / 3.0);
                return View("_Mission", missions);
            }
            if(who == "cmsPages")
            {
                AdminViewModel cms = _adminRepository.getCmsDetail(search, pg);
                ViewBag.pg_no = pg;
                ViewBag.TpCms = Math.Ceiling(adminView.CmsList.Count() / 10.0);
                return View("_CMSPage", cms);
            }
            if (who == "story")
            {
                AdminViewModel story = _adminRepository.getStoryDetail(search, pg);
                ViewBag.pg_no = pg;
                ViewBag.TpStory = Math.Ceiling(adminView.StoryList.Count() / 10.0);
                return View("_Story", story);
            }
            if (who == "application")
            {
                AdminViewModel application = _adminRepository.getApplicationDetail(search, pg);
                ViewBag.pg_no = pg;
                ViewBag.TpApplication = Math.Ceiling(adminView.MissionApplicationList.Count() / 10.0);
                return View("_MissionApplication", application);
            }
            if (who == "skill")
            {
                AdminViewModel skill = _adminRepository.getSkillDetail(search, pg);
                ViewBag.pg_no = pg;
                ViewBag.TpSkill = Math.Ceiling(adminView.MissionSkillList.Count() / 10.0);
                return View("_MissionSkill", skill);
            }
            return View();
        }
        public IActionResult deleteAdminData(long Id, string who)
        {
            bool delete = _adminRepository.deleteAdminData(Id, who);
            return Json(delete);
        }
        public IActionResult getDetails(long id, string who)
        {
            if (who == "theme")
            {
                var data = _adminRepository.getAdminModalInfo(id, who);
                return Json(data.ThemeList.FirstOrDefault());
            }
            if (who == "skill")
            {
                var data = _adminRepository.getAdminModalInfo(id, who);
                return Json(data.SkillList.FirstOrDefault());
            }
            return Json(null);
        }
        public IActionResult addDetails(Card userData)
        {
            var save = _adminRepository.saveDetailsAdmin(userData);
            return Json(save);
        }

        public IActionResult approveAdminData(long Id, string who)
        {
            bool status = _adminRepository.approveAdminData(Id, who);
            return Json(status);
        }
        [Route("/Admin/AddMission")]
        public IActionResult AddMission(CI_PlatForm.Entities.ViewModel.AdminViewModel model, string? search, int pg)
        {
            if (model.MissionModel is null)
            {

                CI_PlatForm.Entities.ViewModel.AdminViewModel missions = _adminRepository.getMissionDetails(search, 0);
                return View("UserCRUD", missions);
            }
            else
            {
                bool success = _adminRepository.AddMission(model.MissionModel);
                CI_PlatForm.Entities.ViewModel.AdminViewModel mis = _adminRepository.getMissionDetails(search, 0);
                return View("UserCRUD", mis);
            }

        }
    }
}
