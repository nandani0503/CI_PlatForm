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
            var cms = _CiplatformDbContext.CmsPages.Where(x => x.DeletedAt == null).ToList();


            adminViewModel.UserList = user;
            adminViewModel.ThemeList = themes;
            adminViewModel.MissionList = missions;
            adminViewModel.CmsList = cms;
            adminViewModel.MissionApplicationList = missionApplication;
            adminViewModel.MissionSkillList = missionSkill;
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
            return false;
        }
    }
}
