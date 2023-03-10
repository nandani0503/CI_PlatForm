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
       

        public List<Country> CountryList();
        public List<City> CityList();
        public List<MissionTheme> MissionThemeList();

        public List<Skill> SkillList();
        public List<Mission> GetMissionList();
        public string getThemeTitle(long themeID);
       public string getMediaName(long missionID);
        public string getCity(long CityID);
     
       public int getMissionRating(long missionId);


    }
}
