﻿using CI_PlatForm.Entities.Models;
using CI_PlatForm.Entities.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_PlatForm.Repository.Interface
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
        public List<Card> GetMissionList(string? search, string[] countries, string[] cities, string[] themes, string[] skills, int sortBy, int paging, long user);

        public List<Card> GetMissionCard(long user_id);

        public string getThemeTitle(long themeID);
        public string getCity(long CityID);
        public string getGoalObject(long missionID);

        public string getMediaName(long missionID);


        public int getMissionRating(long missionId);

        //-------------------------------------------Mission Volunteering-----------------------------------------------------------------------
        public bool addToFavourite(long missionId, long userId);

        public List<CommentViewModel> getComment(long missionId);

        public void PostComment(string comment, long userId, long missionId);

        public String GetSkillName(long missionId);

        public void AddToRecent(long missionId, long userId);
        public List<CommentViewModel> GetRecentUser(long missionId);

        public List<MissionApplication> getMissionApplicant(long mission_id);

        public bool Recommend(long user_id, long mission_id, List<long> co_workers);
        public bool checkFavourite(long missionId, long userId);
        public bool checkApplied(long missionId, long userId);

        /*--------------------------------------Volunteer Story-------------------------------------------------------------------------*/

        public long countVolunteers(long missionId);
        public int getRating(long missionId, long userId);
        public bool PostRating(byte rate, long missionId, long userId);
        public List<StoryViewModel> GetStoryDetails();
        public List<StoryViewModel> GetStoryList(string? search, string[] countries, string[] cities, string[] themes, string[] skills, int paging);
        public List<Mission> getStoryMission(long userid);
        public void AddStory(StoryViewModel model, long UserId, string Submit);
        public StoryViewModel getStory(long story_id, long user_id);
        public StoryViewModel GetDraftDetails(long mission_id, long userId);


        /*-----------------------------Volunteering Timesheet---------------------------------------------------------------------------------*/


        public TimesheetViewModel GetSheetDetails(long userId);
        public bool addTimeSheet(long mission_id, long user_id, DateTime date, int hour, int minutes, string message, long timeSheetId);
        public bool addGoalSheet(long mission_id, long user_id, DateTime date, int action, string message, long timeSheetId);
        public Timesheet getTimeSheet(long timeSheetId);
        public bool deleteTimeSheet(long timeSheetId);
        public bool CheckSameDate(long userId, long missionId, DateTime newDate);

    }
}
