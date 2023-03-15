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

//---------------------Second Nav bar Drop down----------------------------------------------------------------
   
        public List<Country> GetCountryData()
        {
            List<Country> objCountryList = _CiplatformDbContext.Countries.ToList();
            return objCountryList;
        }
        public List<City> GetCityFromCountry(List<string> countryId)
        {
            List<City> city = _CiplatformDbContext.Cities.Where(i => countryId.Contains(i.CountryId.ToString())).ToList();
            return city;
        }
        public List<MissionTheme> GetMissionTheme()
        {
            List<MissionTheme> missionTitle = _CiplatformDbContext.MissionThemes.ToList();
            return missionTitle;
        }
        public List<Skill> GetSkillsList()
        {
            List<Skill> skillsList = _CiplatformDbContext.Skills.ToList();
            return skillsList;
        }

//---------------------------Mission card--------------------------------------------------------------------
        public List<Mission> GetMissionList()
        {
            List<Mission> missionDetail = _CiplatformDbContext.Missions.ToList();
            return missionDetail;
        }
        
        public List<Card> GetMissionList(string? search, string[] countries, string[] cities, string[] themes, string[] skills, int sortBy)
        {
            List<Mission> mission = new List<Mission>();

            List<Card> missions = GetMissionCard();
            if (search != "")
            {
                missions = missions.Where(a => a.Title.ToLower().Contains(search) || a.OrganizationName.ToLower().Contains(search)).ToList();

            }
            if (countries.Length > 0)
            {

                missions = missions.Where(a => countries.Contains(a.CountryId.ToString())).ToList();

            }
            if (cities.Length > 0)
            {

                missions = missions.Where(a => cities.Contains(a.CityId.ToString())).ToList();

            }
            if (themes.Length > 0)
            {

                missions = missions.Where(a => themes.Contains(a.ThemeId.ToString())).ToList();

            }
            if (skills.Length > 0)
            {

                missions = missions.Where(a => skills.Contains(a.skillId.ToString())).ToList();

            }
            switch (sortBy)
            {
                case 1:
                    missions = missions.OrderBy(i => i.StartDate).ToList();
                    break;

                case 2:
                    missions = missions.OrderByDescending(i => i.StartDate).ToList();
                    break;

                case 3:
                    missions = missions.OrderBy(i => i.Deadline).ToList();
                    break;

                case 4:
                    missions = missions.OrderBy(i => i.TotalSeat).ToList();
                    break;
            }
            return missions;
        }

        public long getTotalMission()
        {
            var total = _CiplatformDbContext.Missions.Count();
            return total;
        }
      

        public string getThemeTitle(long themeID)
        {
            var theme = _CiplatformDbContext.MissionThemes.FirstOrDefault(u => u.MissionThemeId == themeID);
            return theme.Title;
        }
        public string getCity(long CityID)
        {
            var CityName = _CiplatformDbContext.Cities.FirstOrDefault(u => u.CityId == CityID);
            return CityName.Name;
        }
        public string getMediaName(long missionID)
        {
            var imagePath = _CiplatformDbContext.MissionMedia.FirstOrDefault(u => u.MissionId == missionID);
            return imagePath.MediaName;
        }
     
        public int getMissionRating(long missionId)
        {
            var rating = _CiplatformDbContext.MissionRatings.FirstOrDefault(u => u.MissionId == missionId);
            return rating.Rating;
        }
        public List<Card> GetMissionCard()
        {
            List<Card> missionAllDetails = new List<Card>();
            var missions = _CiplatformDbContext.Missions.ToList();
            foreach (var allDetailsCard in missions)
            {
                Card cardDetails = new Card();


                cardDetails.Theme = getThemeTitle(allDetailsCard.ThemeId);
                cardDetails.Title = allDetailsCard.Title;
                cardDetails.MissionId = allDetailsCard.MissionId;
                cardDetails.CityName = getCity(allDetailsCard.CityId);
                cardDetails.ShortDescription = allDetailsCard.ShortDescription;
                cardDetails.CityId = allDetailsCard.CityId;
                cardDetails.OrganizationName = allDetailsCard.OrganizationName;
                cardDetails.StartDate = allDetailsCard.StartDate;
                cardDetails.EndDate = allDetailsCard.EndDate;
                cardDetails.MediaName = getMediaName(allDetailsCard.MissionId);
                cardDetails.Rating = getMissionRating(allDetailsCard.MissionId);
                cardDetails.CountryId= allDetailsCard.CountryId;
                cardDetails.ThemeId=allDetailsCard.ThemeId;
                /*cardDetails.TotalSeat = allDetailsCard.Avaibility;*/

                var missionSkill = _CiplatformDbContext.MissionSkills.FirstOrDefault(u => u.MissionId == allDetailsCard.MissionId);
                cardDetails.skillId = missionSkill.MissionSkillId;
                missionAllDetails.Add(cardDetails);
            }

            return missionAllDetails;
        }

        /*public string getCountry(long CountryID)
        {
            var CountryName = _CiplatformDbContext.Countries.FirstOrDefault(u => u.CountryId == CountryID);
            return CountryName.CountryName;
        }*/

    }
}
