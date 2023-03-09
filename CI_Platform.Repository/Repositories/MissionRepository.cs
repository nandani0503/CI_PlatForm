using CI_Platform.Entities.Data;
using CI_Platform.Entities.Models;
using CI_Platform.Entities.ViewModel;
using CI_Platform.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Repositories
{
    public class MissionRepository : IMissionRepository
    {
        public readonly CiplatformContext _CiplatformDbContext;
        
        public MissionRepository(CiplatformContext ciplatformDbContext)
        {
            _CiplatformDbContext = ciplatformDbContext;
       
        }
        public List<Country> CountryList()
        {
            List<Country> objCountryList = _CiplatformDbContext.Countries.ToList();
            return objCountryList;
        }
        public List<City> CityList()
        {
            List<City> objCityList = _CiplatformDbContext.Cities.ToList();
            return objCityList;
        }
        public List<MissionTheme> MissionThemeList()
        {
            List<MissionTheme> objMissionThemeList = _CiplatformDbContext.MissionThemes.ToList();
            return objMissionThemeList;
        }
        public List<Skill> SkillList()
        {
            List<Skill> objSkillList = _CiplatformDbContext.Skills.ToList();
            return objSkillList;
        }
        public List<Mission> GetMissionList()
        {
            List<Mission> missionDetail = _CiplatformDbContext.Missions.ToList();
            return missionDetail;
        }


        public string getThemeTitle(long themeID)
        {
            var theme = _CiplatformDbContext.MissionThemes.FirstOrDefault(u => u.MissionThemeId == themeID);
            return theme.Title;
        }
        public string getCountry(long CountryID)
        {
            var CountryName = _CiplatformDbContext.Countries.FirstOrDefault(u => u.CountryId == CountryID);
            return CountryName.CountryName;
        }

    }
}
