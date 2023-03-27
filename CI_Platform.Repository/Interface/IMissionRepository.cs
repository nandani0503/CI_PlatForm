﻿using CI_Platform.Entities.Models;
using CI_Platform.Entities.ViewModel;
using CI_PlatForm.Entities.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Interface
{
    public interface IMissionRepository
    {

        //-----------------------------second navbar---------------------------------------------------------------------------------------------
       

        public List<Country> GetCountryData();
        public List<City> GetCityFromCountry(List<string> countryId);
        public List<MissionTheme> GetMissionTheme();
        public List<Skill> GetSkillsList();
        //------------------------------------Mission card---------------------------------------------------------------------------------------
        public List<Mission> GetMissionList();
        public List<Card> GetMissionList(string? search, string[] countries, string[] cities, string[] themes, string[] skills,int sortBy, int paging);
        public long getTotalMission();
        public List<Card> GetMissionCard();
   
        public string getThemeTitle(long themeID);
       public string getMediaName(long missionID);
        public string getCity(long CityID);
     
       public List<MissionRating> getMissionRating(long missionId);

        //-------------------------------------------Mission Volunteering-----------------------------------------------------------------------
        public bool addToFavourite(long missionId, long userId);

        public List<CommentViewModel> getComment(long missionId);

        public void PostComment(string comment, long userId, long missionId);

        public String GetSkillName(long missionId);

        public void AddToRecent(long missionId, long userId);
        public List<CommentViewModel> GetRecentUser(long missionId);

        public List<MissionApplication> getMissionApplicant();

        public bool Recommend(long user_id, long mission_id, List<long> co_workers);

        /*--------------------------------------Volunteer Story-------------------------------------------------------------------------*/

        public List<StoryViewModel> GetStoryDetails();
        /*public List<StoryViewModel> GetStoryData(string? searchStory, /*string[] countries, string[] cities, string[] themes, string[] skills);*/
        public List<StoryViewModel> GetStoryData(string? searchStory);
        /*public List<StoryViewModel> GetStoryData();*/
        public string GetType(long storyId);
        public List<Mission> GetStoryList();
        public bool checkFavourite(long missionId, long userId);

    }
}
