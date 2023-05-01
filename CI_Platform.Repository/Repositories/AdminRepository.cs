using CI_PlatForm.Entities.Data;
using CI_PlatForm.Entities.Models;
using CI_PlatForm.Entities.ViewModel;
using CI_PlatForm.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_PlatForm.Repository.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly CiplatformContext _CiplatformDbContext;
        public AdminRepository(CiplatformContext ciplatformDbContext)
        {
            _CiplatformDbContext = ciplatformDbContext;
        }
      
       public AdminViewModel getAdminList()
        {
            AdminViewModel adminViewModel = new AdminViewModel();
            var user = _CiplatformDbContext.Users.Where(x => x.DeletedAt == null).ToList();
            var themes = _CiplatformDbContext.MissionThemes.ToList();
            var missions = _CiplatformDbContext.Missions.ToList();
            var missionApplication = _CiplatformDbContext.MissionApplications.ToList();
            var missionSkill = _CiplatformDbContext.MissionSkills.Include(s => s.Skill).ToList();
            var story = _CiplatformDbContext.Stories.Where(x => x.DeletedAt == null).ToList();
            var Skill = _CiplatformDbContext.Skills.Where(x => x.DeletedAt == null).ToList();
            var Country = _CiplatformDbContext.Countries.Where(x => x.DeletedAt == null).ToList();
            var skillList = _CiplatformDbContext.Skills.Where(s => s.DeletedAt == null).ToList();
            var cms = _CiplatformDbContext.CmsPages.Where(x => x.DeletedAt == null).ToList();


            adminViewModel.UserList = user;
            adminViewModel.ThemeList = themes;
            adminViewModel.MissionList = missions;
            adminViewModel.CmsList = cms;
            adminViewModel.MissionApplicationList = missionApplication;
            adminViewModel.MissionSkillList = missionSkill;
            adminViewModel.countryList = Country;
            adminViewModel.SkillList = skillList;
            adminViewModel.StoryList = story;
            return adminViewModel;
        }
        public AdminViewModel getUserDetail(string search, int paging)
        {
            var pageSize = 10;
            var adminModel = getAdminList();
            if(search != "")
            {
                adminModel.UserList = adminModel.UserList.Where(a => a.FirstName.ToLower().Contains(search) || a.LastName.ToLower().Contains(search)).ToList();
            }
            if(paging != 0)
            {
                adminModel.UserList = adminModel.UserList.Skip((paging - 1) * pageSize).Take(10).ToList();

            }
            return adminModel;
        }
        public AdminViewModel getThemeDetail(string search, int paging)
        {
            var pageSize = 3;
            var adminModel = getAdminList();
            if(search != "")
            {
                adminModel.ThemeList = adminModel.ThemeList.Where(t => t.Title.ToLower().Contains(search)).ToList();
            }
            if(paging != 0)
            {
                adminModel.ThemeList = adminModel.ThemeList.Skip((paging - 1) * pageSize).Take(3).ToList();
            }
            return adminModel;
        }
        public AdminViewModel getMissionDetail(string search, int paging)
        {
            var pageSize = 3;
            var adminModel = getAdminList();
            if(search != "")
            {
                adminModel.MissionList = adminModel.MissionList.Where(m => m.Title.ToLower().Contains(search)).ToList();
            }
            if(paging != 0)
            {
                adminModel.MissionList = adminModel.MissionList.Skip((paging - 1) * pageSize).Take(3).ToList();
            }
            return adminModel;
        }
        public AdminViewModel getBannerDetails(string search, int paging)
        {
            var pageSize = 10;
            //var adminModel = getAdminList();
            var adminModel = new AdminViewModel();
            adminModel.BannerList = _CiplatformDbContext.Banners.ToList();
            if (search != "")
            {
                adminModel.BannerList = adminModel.BannerList.Where(b => b.Text.ToLower().Contains(search)).ToList();
            }
            if (paging != 0)
            {
                adminModel.BannerList = adminModel.BannerList.Skip((paging - 1) * pageSize).Take(10).ToList();
            }
            return adminModel;
        }
        public AdminViewModel getCmsDetail(string search, int paging)
        {
            var pageSize = 3;
            var adminModel = getAdminList();
            if(search != "")
            {
                adminModel.CmsList = adminModel.CmsList.Where(c => c.Title.ToLower().Contains(search)).ToList();
            }
            if(paging != 0)
            {
                adminModel.CmsList = adminModel.CmsList.Skip((paging - 1) * pageSize).Take(3).ToList();
            }
            return adminModel;
        }
        public AdminViewModel getStoryDetail(string search, int paging)
        {
            var pageSize = 10;
            var adminModel = getAdminList();
            if (search != "")
            {
                adminModel.StoryList = adminModel.StoryList.Where(story => story.Title.ToLower().Contains(search) || story.User.FirstName.ToLower().Contains(search) || story.User.LastName.ToLower().Contains(search)).ToList();
            }
            if (paging != 0)
            {
                adminModel.StoryList = adminModel.StoryList.Skip((paging - 1) * pageSize).Take(10).ToList();
            }
            return adminModel;
        }
        public AdminViewModel getApplicationDetail(string search, int paging)
        {
            var pageSize = 10;
            var adminModel = getAdminList();
            if (search != "")
            {
                adminModel.MissionApplicationList = adminModel.MissionApplicationList.Where(missionApp => missionApp.Mission.Title.ToLower().Contains(search) || missionApp.User.FirstName.ToLower().Contains(search) || missionApp.User.LastName.ToLower().Contains(search)).ToList();
            }
            if (paging != 0)
            {
                adminModel.MissionApplicationList = adminModel.MissionApplicationList.Skip((paging - 1) * pageSize).Take(10).ToList();
            }
            return adminModel;
        }
        public AdminViewModel getSkillDetail(string search, int paging)
        {
            var pageSize = 10;
            var adminModel = getAdminList();
            if (search != "")
            {
                adminModel.MissionSkillList = adminModel.MissionSkillList.Where(skill => skill.Skill.SkillName.ToLower().Contains(search)).ToList();
            }
            if (paging != 0)
            {
                adminModel.MissionSkillList = adminModel.MissionSkillList.Skip((paging - 1) * pageSize).Take(10).ToList();
            }
            return adminModel;
        }
        public bool deleteAdminData(long id, string who)
        {
            if (who == "user")
            {
                var user = _CiplatformDbContext.Users.FirstOrDefault(user => user.UserId == id);
                if (user.UserId == id)
                {
                    user.DeletedAt = DateTime.Now;
                    _CiplatformDbContext.SaveChanges();
                    return true;
                }

            }
            if (who == "cms")
            {
                var cms = _CiplatformDbContext.CmsPages.FirstOrDefault(user => user.CmsPageId == id);
                if (cms.CmsPageId == id)
                {
                    cms.DeletedAt = DateTime.Now;
                    _CiplatformDbContext.SaveChanges();
                    return true;
                }

            }
            if (who == "mission")
            {
                var mission = _CiplatformDbContext.Missions.FirstOrDefault(m => m.MissionId == id);
                if (mission.MissionId == id)
                {
                    mission.DeletedAt = DateTime.Now;
                    _CiplatformDbContext.SaveChanges();
                    return true;
                }

            }
            if (who == "theme")
            {
                var theme = _CiplatformDbContext.MissionThemes.FirstOrDefault(m => m.MissionThemeId == id);
                if (theme.MissionThemeId == id)
                {
                    theme.DeletedAt = DateTime.Now;
                    _CiplatformDbContext.SaveChanges();
                    return true;
                }

            }
            if (who == "skill")
            {
                var skill = _CiplatformDbContext.Skills.FirstOrDefault(s => s.SkillId == id);
                if (skill.SkillId == id)
                {
                    skill.DeletedAt = DateTime.Now;
                    _CiplatformDbContext.SaveChanges();
                    return true;
                }

            }
            if (who == "story")
            {
                var story = _CiplatformDbContext.Stories.FirstOrDefault(s => s.StoryId == id);
                if (story.StoryId == id)
                {
                    story.DeletedAt = DateTime.Now;
                    _CiplatformDbContext.SaveChanges();
                    return true;
                }

            }
            return false;
        }
        public Skill getSkillDetails(long skillId)
        {
            var skillData = _CiplatformDbContext.Skills.FirstOrDefault(skill => skill.SkillId == skillId);
            return skillData;
        }
        public AdminViewModel getAdminModalInfo(long id, string who)
        {
            var AdminModal = getAdminList();
            if (who == "theme")
            {
                AdminModal.ThemeList = AdminModal.ThemeList.Where(t => t.MissionThemeId == id).ToList();
            }
            if (who == "skill")
            {
                AdminModal.SkillList = AdminModal.SkillList.Where(t => t.SkillId == id).ToList();
            }
            return AdminModal;
        }
        public bool saveDetailsAdmin(Card userdata)
        {
            if (userdata.whoType == "theme")
            {
                if (userdata.crudId != null)
                {
                    var theme = _CiplatformDbContext.MissionThemes.FirstOrDefault(m => m.MissionThemeId == userdata.crudId);
                    theme.Title = userdata.Theme;
                    theme.Status = userdata.status;
                    theme.UpdatedAt = DateTime.Now;
                    _CiplatformDbContext.Update(theme);
                }
                else
                {
                    var theme = new MissionTheme();
                    theme.Title = userdata.Theme;
                    theme.Status = userdata.status;
                    theme.CreatedAt = DateTime.Now;
                    _CiplatformDbContext.Add(theme);
                }
                _CiplatformDbContext.SaveChanges();
                return true;
            }
            if (userdata.whoType == "skill")
            {
                if (userdata.crudId != null)
                {
                    var skill = _CiplatformDbContext.Skills.FirstOrDefault(m => m.SkillId == userdata.crudId);
                    skill.SkillName = userdata.SkillName;
                    skill.Status = userdata.status;
                    skill.UpdatedAt = DateTime.Now;
                    _CiplatformDbContext.Update(skill);
                }
                else
                {
                    var skill = new Skill();
                    skill.SkillName = userdata.SkillName;
                    skill.Status = userdata.status;
                    skill.CreatedAt = DateTime.Now;
                    _CiplatformDbContext.Add(skill);
                }
                _CiplatformDbContext.SaveChanges();
                return true;
            }
            return false;
        }

        public bool approveAdminData(long id, string who)
        {
            if (who == "ApproveApp")
            {
                var application = _CiplatformDbContext.MissionApplications.FirstOrDefault(m => m.MissionApplicationId == id);
                application.ApprovalStatus = "APPROVE";
                _CiplatformDbContext.SaveChanges();
                return true;
            }
            if (who == "DeclineApp")
            {
                var application = _CiplatformDbContext.MissionApplications.FirstOrDefault(m => m.MissionApplicationId == id);
                application.ApprovalStatus = "DECLINE";
                _CiplatformDbContext.SaveChanges();
                return true;
            }
            if (who == "ApproveStory")
            {
                var story = _CiplatformDbContext.Stories.FirstOrDefault(m => m.StoryId == id);
                story.Status = "PUBLISHED";
                _CiplatformDbContext.SaveChanges();
                return true;
            }
            if (who == "DeclineStory")
            {
                var story = _CiplatformDbContext.Stories.FirstOrDefault(m => m.StoryId == id);
                story.Status = "DRAFT";
                _CiplatformDbContext.SaveChanges();
                return true;
            }

            return false;
        }
        public AdminViewModel getMissionDetails(string search, int paging)
        {
            var pageSize = 3;
            var adminModel = new AdminViewModel();
            adminModel.countryList = _CiplatformDbContext.Countries.ToList();
            adminModel.city_list = _CiplatformDbContext.Cities.ToList();
            adminModel.ThemeList = _CiplatformDbContext.MissionThemes.ToList();
            adminModel.SkillList = _CiplatformDbContext.Skills.ToList();
            adminModel.MissionList = _CiplatformDbContext.Missions.ToList();
            if (search is not null)
            {
                adminModel.MissionList = adminModel.MissionList.Where(mission => mission.Title.ToLower().Contains(search)).ToList();
            }
            if (paging != 0)
            {
                adminModel.MissionList = adminModel.MissionList.Skip((paging - 1) * pageSize).Take(3).ToList();
            }

            return adminModel;
        }
        public bool AddMission(MissionAdminViewModel model)
        {
            CI_PlatForm.Entities.Models.Mission mission = new Entities.Models.Mission();

            mission.CountryId = model.CountryId;
            mission.CityId = model.CityId;
            mission.ThemeId = model.ThemeId;
            mission.Title = model.Title;
            mission.Description = model.Description;
            mission.StartDate = model.StartDate;
            mission.EndDate = model.EndDate;
            mission.Deadline = model.Deadline;
            mission.OrganizationName = model.OrganizationName;
            mission.OrganizationDetail = model.OrganizationDetail;
            mission.MissionType = model.missiontype;
            /*mission.TotalSeats = model.TotalSeats;*/
            mission.Status = true;
            mission.SeatLeft = model.AvbSeat;
            mission.Avaibility = model.Availability;
            _CiplatformDbContext.Missions.Add(mission);
            _CiplatformDbContext.SaveChanges();

            CI_PlatForm.Entities.Models.GoalMission goal = new Entities.Models.GoalMission();
            goal.MissionId = mission.MissionId;
            goal.GoalValue = int.Parse(model.goalobject);
            goal.Achieved = model.Achieved;
            _CiplatformDbContext.GoalMissions.Add(goal);
            _CiplatformDbContext.SaveChanges();
            string[]? getskills = model.skillname?.Split(",");
            foreach (var skill in getskills)
            {
                _CiplatformDbContext.MissionSkills.Add(new MissionSkill
                {
                    MissionId = mission.MissionId,
                    SkillId = int.Parse(skill)
                });
            }
            _CiplatformDbContext.SaveChanges();
            int count = 1;
            foreach (var image in model.missionMediums)
            {
                FileInfo fileInfo = new FileInfo(image.FileName);
                string filename = $"mission{mission.MissionId}-image-{count}" + fileInfo.Extension;
                string rootpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "CI Platform Assets", filename);
                _CiplatformDbContext.MissionMedia.Add(new MissionMedium
                {
                    MissionId = mission.MissionId,
                    MediaPath = filename
                });
                using (Stream fileStream = new FileStream(rootpath, FileMode.Create))
                {
                    image.CopyTo(fileStream);
                }
                count++;
            }
            _CiplatformDbContext.SaveChanges();
            foreach (var doc in model.MissionDocuments)
            {
                FileInfo fileInfo = new FileInfo(doc.FileName);
                string filename = $"mission{mission.MissionId}-document-{count}" + fileInfo.Extension;
                string rootpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Documents", filename);
                _CiplatformDbContext.MissionDocuments.Add(new MissionDocument
                {
                    MissionId = mission.MissionId,
                    DocumentPath = filename
                });
                using (Stream fileStream = new FileStream(rootpath, FileMode.Create))
                {
                    doc.CopyTo(fileStream);
                }
                count++;
            }
            _CiplatformDbContext.SaveChanges();
            return true;
        }
    }
}
