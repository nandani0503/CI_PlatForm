using CI_Platform.Entities.Models;
using CI_Platform.Entities.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Interface
{
    public interface IMissionRepository
    {

        //-----------------------------second navbar------------------------------------------------
       

        public List<Country> GetCountryData();
        public List<City> GetCityFromCountry(List<string> countryId);
        public List<MissionTheme> GetMissionTheme();
        public List<Skill> GetSkillsList();
        //------------------------------------Mission card--------------------------------------------
        public List<Mission> GetMissionList();
        public List<Card> GetMissionList(string? search, string[] countries, string[] cities, string[] themes, string[] skills,int sortBy);
        public long getTotalMission();
        public List<Card> GetMissionCard();
        public string getThemeTitle(long themeID);
       public string getMediaName(long missionID);
        public string getCity(long CityID);
     
       public int getMissionRating(long missionId);


    }
}
