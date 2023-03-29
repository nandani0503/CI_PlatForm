using CI_Platform.Entities.Data;
using CI_Platform.Entities.Models;
using CI_Platform.Entities.ViewModel;
using CI_Platform.Repository.Interface;
using CI_PlatForm.Entities.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
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

        public List<Card> GetMissionList(string? search, string[] countries, string[] cities, string[] themes, string[] skills, int sortBy, int paging, long user)
        {
            var pageSize = 6;

            /*List<Mission> mission = new List<Mission>();*/

            List<Card> missions = GetMissionCard(user);
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
                    missions = missions.OrderBy(i => i.Avaibility).ToList();
                    break;

                case 5:
                    missions = missions.OrderByDescending(i => i.Avaibility).ToList();
                    break;

                case 6:
                    missions = missions.OrderBy(i => i.FavouriteMissionId).ToList();
                    break;
            }
            if (paging != null)
            {
                missions = missions.Skip((paging - 1) * pageSize).Take(6).ToList();
            }
            return missions;
        }

       /* public long getTotalMission()
        {
            var total = _CiplatformDbContext.Missions.Count();
            return total;
        }*/


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

        public string getGoalObject(long missionID)
        {
            var goaltext = _CiplatformDbContext.GoalMissions.FirstOrDefault(u => u.MissionId == missionID);
            if (goaltext != null)
                return goaltext.GoalObjectiveText;
            return null;
        }

        public List<MissionRating> /*int*/ getMissionRating(long missionId)
        {
            /*List<MissionRating>*/
            var rating = _CiplatformDbContext.MissionRatings.Where(u => u.MissionId == missionId).ToList();/*FirstOrDefault(u => u.MissionId == missionId);*/
            return rating/*.Rating*/;
        }
        public List<Card> GetMissionCard(long user_id)
        {
            List<Card> missionAllDetails = new List<Card>();
            var missions = _CiplatformDbContext.Missions.ToList();
            foreach (var allDetailsCard in missions)
            {
                Card cardDetails = new Card();
                var fav = _CiplatformDbContext.FavoriteMissions.FirstOrDefault(u => u.MissionId == allDetailsCard.MissionId && u.UserId == user_id);
                if(fav != null)
                {
                    cardDetails.checkFav = true;
                }
                else
                {
                    cardDetails.checkFav = false;
                }

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
                cardDetails.Rating = (int)getMissionRating(allDetailsCard.MissionId).Average(a => a.Rating);
                cardDetails.CountryId = allDetailsCard.CountryId;
                cardDetails.ThemeId = allDetailsCard.ThemeId;
                cardDetails.Avaibility = allDetailsCard.Avaibility;
                cardDetails.MissionType = allDetailsCard.MissionType;
                cardDetails.Description = allDetailsCard.Description;
                /*cardDetails.missionIntro = allDetailsCard.Description;*/
                cardDetails.aboutOrganization = allDetailsCard.OrganizationDetail;
                cardDetails.GoalObjectiveText = getGoalObject(allDetailsCard.MissionId);

                var missionSkill = _CiplatformDbContext.MissionSkills.FirstOrDefault(u => u.MissionId == allDetailsCard.MissionId);
                cardDetails.skillId = missionSkill.MissionSkillId;
                missionAllDetails.Add(cardDetails);
            }

            return missionAllDetails;
        }



        //---------------------------------------Mission Volunteering----------------------------------------------------------------------


        public bool checkFavourite(long missionId, long userId)
        {
            var fav = _CiplatformDbContext.FavoriteMissions.FirstOrDefault(a => a.MissionId == missionId && a.UserId == userId);
            if (fav != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool checkApplied(long missionId, long userId)
        {
            var apply = _CiplatformDbContext.MissionApplications.FirstOrDefault(a => a.MissionId == missionId && a.UserId == userId);
            if(apply != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool addToFavourite(long missionId, long userId)
        {
            FavoriteMission favourite = new();
            favourite.MissionId = missionId;
            favourite.UserId = userId;

            var FavMission = _CiplatformDbContext.FavoriteMissions.FirstOrDefault(i => i.MissionId == missionId && i.UserId == userId);
            if (FavMission == null)
            {
                _CiplatformDbContext.FavoriteMissions.Add(favourite);
                _CiplatformDbContext.SaveChanges();
                return true;
            }
            else
            {
                _CiplatformDbContext.FavoriteMissions.Remove(FavMission);
                _CiplatformDbContext.SaveChanges();
                return false;
            }
        }

        public List<CommentViewModel> getComment(long missionId)
        {
            List<Comment> comments = _CiplatformDbContext.Comments.Where(c => c.MissionId == missionId && c.ApprovalStatus == /*"PUBLISHED"*/ "PENDING").ToList();
            List<CommentViewModel> commentView = new List<CommentViewModel>();

            foreach (var comment in comments)
            {
                CommentViewModel commentList = new CommentViewModel();
                User user = _CiplatformDbContext.Users.FirstOrDefault(a => a.UserId == comment.UserId);
                commentList.Comment = comment.Comments;
                commentList.Month = comment.CreatedAt.ToString("MMMM");
                commentList.Time = comment.CreatedAt.ToString("h:mm tt");
                commentList.Day = comment.CreatedAt.Day.ToString();
                commentList.WeekDay = comment.CreatedAt.DayOfWeek.ToString();
                commentList.Year = comment.CreatedAt.Year.ToString();
                commentList.Avatar = user.Avatar;
                commentList.UserName = user.FirstName + " " + user.LastName;

                commentView.Add(commentList);
            }
            return commentView;
        }


        public void PostComment(string comment, long userId, long missionId)
        {
            Comment newComment = new Comment();
            newComment.Comments = comment;
            newComment.UserId = userId;
            newComment.MissionId = missionId;

            _CiplatformDbContext.Comments.Add(newComment);
            _CiplatformDbContext.SaveChanges();
        }


        public String GetSkillName(long missionId)
        {
            return string.Join(", ", _CiplatformDbContext.Missions.Where(x => x.MissionId == missionId).SelectMany(x => x.MissionSkills).Select(x => x.Skill.SkillName).ToList());
        }

        public List<CommentViewModel> GetRecentUser(long missionId)
        {
            List<MissionApplication> missionApplication = _CiplatformDbContext.MissionApplications.Where(a => a.MissionId == missionId && a.ApprovalStatus == "APPROVE").ToList();
            List<CommentViewModel> missionView = new List<CommentViewModel>();
            foreach (MissionApplication application in missionApplication)
            {
                CommentViewModel missionviewModel = new CommentViewModel();
                User user = _CiplatformDbContext.Users.FirstOrDefault(a => a.UserId == application.UserId);
                missionviewModel.MissionId = missionId;
                missionviewModel.Avatar = user.Avatar;
                missionviewModel.UserName = user.FirstName + " " + user.LastName;

                missionView.Add(missionviewModel);

            }
            return missionView;
        }
        /*public bool checkFavourite(long missionId, long userId)
        {
            var fav = _CiplatformDbContext.FavoriteMissions.FirstOrDefault(a => a.MissionId == missionId && a.UserId == userId);
            if(fav != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }*/

        public void AddToRecent(long missionId, long userId)
        {
            MissionApplication missionapplication = new MissionApplication();
            missionapplication.UserId = userId;
            missionapplication.MissionId = missionId;
            missionapplication.AppliedAt = DateTime.Now;
            /*missionapplication.ApprovalStatus = "APPROVE";*/
            _CiplatformDbContext.MissionApplications.Add(missionapplication);
            _CiplatformDbContext.SaveChanges();
        }
        public List<MissionApplication> getMissionApplicant()
        {
            List<MissionApplication> missionapplications = _CiplatformDbContext.MissionApplications.ToList();
            return missionapplications;
        }
        public bool Recommend(long user_id, long mission_id, List<long> co_workers)
        {
            foreach (var user in co_workers)
            {
                _CiplatformDbContext.MissionInvites.Add(new MissionInvite
                {
                    FromUserId = user_id,
                    ToUserId = user,
                    MissionId = mission_id
                });
            }
            _CiplatformDbContext.SaveChanges();
            User from_user = _CiplatformDbContext.Users.FirstOrDefault(c => c.UserId.Equals(user_id));
            List<string> Email_users = (from u in _CiplatformDbContext.Users
                                        where co_workers.Contains(u.UserId)
                                        select u.Email).ToList();
            foreach (var email in Email_users)
            {
                var senderEmail = new MailAddress("dummye754@gmail.com", "CI_PlatForm");
                var receiverEmail = new MailAddress(email, "Receiver");
                var password = "cfwhnexvaurcxgkd";
                var sub = "Recommendation";
                var body = "Recommend By " + from_user?.FirstName + " " + from_user?.LastName + "\n" + $"https://localhost:7217/Mission/MissionVolunteering/{mission_id}";
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(senderEmail.Address, password)
                };
                using (var mess = new MailMessage(senderEmail, receiverEmail)
                {
                    Subject = sub,
                    Body = body
                })
                {
                    smtp.Send(mess);
                }
            }
            return true;
        }
        /*----------------------------------------Story Listing----------------------------------------------------------------*/

        public string getStoryImg(long storyId)
        {
            var imagePath = _CiplatformDbContext.StoryMedia.FirstOrDefault(u => u.StoryId == storyId);
            return imagePath.Path;
        }



        public string GetType(long storyId)
        {
            var type = _CiplatformDbContext.StoryMedia.FirstOrDefault(u => u.StoryId == storyId);
            return type.Type;
        }



        public List<StoryViewModel> GetStoryDetails()
        {
            List<Story> PublishedStory = _CiplatformDbContext.Stories.Where(s => s.Status == "PUBLISHED").ToList();


            List<StoryViewModel> stories = new List<StoryViewModel>();

            foreach (var story in PublishedStory)
            {

                StoryViewModel storyInfo = new StoryViewModel();
                User user = _CiplatformDbContext.Users.FirstOrDefault(a => a.UserId == story.UserId);
                storyInfo.Type = GetType(story.StoryId);
                storyInfo.StoryId = story.StoryId;
                storyInfo.Title = story.Title;
                storyInfo.Description = story.Description;
                storyInfo.storyImage = getStoryImg(story.StoryId);
                storyInfo.Avatar = user.Avatar;
                storyInfo.UserName = user.FirstName + " " + user.LastName;
                stories.Add(storyInfo);
            }

            return stories;
        }

        public List<StoryViewModel> GetStoryData(string? searchStory)
        {
            List<Story> story = new List<Story>();
            List<StoryViewModel> stories = GetStoryDetails();
            if(searchStory != "")
            {
                stories = stories.Where(a => a.Title.ToLower().Contains(searchStory)).ToList();
            }
            return stories;
        }

       
        public List<Story> GetStoryData()
        {
            List<Story> storyDetail = _CiplatformDbContext.Stories.ToList();
            return storyDetail;
        }
       public List<Mission> GetStoryList()
        {
            List<Mission> missionStory = _CiplatformDbContext.Missions.ToList();
            return missionStory;
        }
        
        
    }

    }
