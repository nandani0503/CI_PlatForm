﻿using CI_PlatForm.Entities.Data;
using CI_PlatForm.Entities.Models;
using CI_PlatForm.Entities.ViewModel;
using CI_PlatForm.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CI_PlatForm.Repository.Repositories
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

               /* case 6:
                    missions = missions.OrderBy(i => i.FavouriteMissionId).ToList();
                    break;*/
            }
            if (paging != null)
            {
                missions = missions.Skip((paging - 1) * pageSize).Take(6).ToList();
            }
            return missions;
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

        public string getGoalObject(long missionID)
        {
            var goaltext = _CiplatformDbContext.GoalMissions.FirstOrDefault(u => u.MissionId == missionID);
            if (goaltext != null)
                return goaltext.GoalObjectiveText;
            return null;
        }

        public int getMissionRating(long missionId)
        {
            var a = _CiplatformDbContext.MissionRatings.Where(a => a.MissionId == missionId).ToList();
            int rating = 0;
            if(a.Count > 0)
            {
                rating = (int)_CiplatformDbContext.MissionRatings.Where(a => a.MissionId == missionId).Average(a => a.Rating);
            }
            /*var rating = _CiplatformDbContext.MissionRatings.Where(a => a.MissionId == missionId).Average(a => a.Rating);*/
            return (int)rating;
        }
        public long countVolunteers(long missionId)
        {
            long ratingVolunteers = _CiplatformDbContext.MissionRatings.Where(r => r.MissionId == missionId).Count();
            return ratingVolunteers;
        }
        public List<Card> GetMissionCard(long user_id)
        {
            List<Card> missionAllDetails = new List<Card>();
            var missions = _CiplatformDbContext.Missions.ToList();
            var missionGoalList = _CiplatformDbContext.GoalMissions.ToList();
            foreach (var allDetailsCard in missions)
            {
                Card cardDetails = new Card();
                var fav = _CiplatformDbContext.FavoriteMissions.FirstOrDefault(u => u.MissionId == allDetailsCard.MissionId && u.UserId == user_id);
                if (fav != null)
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
                cardDetails.Rating = getMissionRating(allDetailsCard.MissionId);
                cardDetails.CountryId = allDetailsCard.CountryId;
                cardDetails.ThemeId = allDetailsCard.ThemeId;
                cardDetails.Avaibility = allDetailsCard.Avaibility;
                cardDetails.SeatLeft = allDetailsCard.SeatLeft;
                cardDetails.MissionType = allDetailsCard.MissionType;
                cardDetails.Description = allDetailsCard.Description;
                cardDetails.aboutOrganization = allDetailsCard.OrganizationDetail;
                cardDetails.GoalObjectiveText = getGoalObject(allDetailsCard.MissionId);
                cardDetails.Deadline = allDetailsCard.Deadline;
                if(allDetailsCard.MissionType == false)
                {
                    cardDetails.GoalValue = missionGoalList.FirstOrDefault(x => x.MissionId == allDetailsCard.MissionId).GoalValue;
                    cardDetails.Achieved = missionGoalList.FirstOrDefault(x => x.MissionId == allDetailsCard.MissionId).Achieved;
                    cardDetails.Progress = cardDetails.Achieved * 100 / cardDetails.GoalValue;
                }
                var missionSkill = _CiplatformDbContext.MissionSkills.FirstOrDefault(u => u.MissionId == allDetailsCard.MissionId);
                cardDetails.skillId = missionSkill.SkillId;
                var allUser = _CiplatformDbContext.Users.Where(x => x.DeletedAt == null).ToList();
                var AlreadyInvite = _CiplatformDbContext.MissionInvites.Where(i => i.FromUserId == user_id).Include(y => y.ToUser).ToList();
                foreach(var item in AlreadyInvite)
                {
                    allUser = allUser.Where(x => x.UserId != item.ToUserId).ToList();
                }
                cardDetails.allUser = allUser;
                cardDetails.AlreadyInvite = AlreadyInvite;

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
            var apply = _CiplatformDbContext.MissionApplications.Where(a => a.MissionId == missionId && a.UserId == userId).ToList().Count;
            if (apply != 0)
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
        /*---------------------------------Rating------------------------------------*/


        public int getRating(long missionId, long userId)
        {
            if (_CiplatformDbContext.MissionRatings.FirstOrDefault(r => r.MissionId == missionId && r.UserId == userId) is not null)
            {
                int getRate = _CiplatformDbContext.MissionRatings.FirstOrDefault(r => r.MissionId == missionId && r.UserId == userId).Rating;
                return getRate;
            }
            else
            {
                return 0;
            }
        }
        public bool PostRating(byte rate, long missionId, long userId)
        {
            var entry = _CiplatformDbContext.MissionRatings.Where(m => m.MissionId == missionId && m.UserId == userId);
            if (entry.ToList().Count == 0)
            {
                var data = new MissionRating()
                {
                    UserId = userId,
                    MissionId = missionId,
                    Rating = rate
                };
                _CiplatformDbContext.MissionRatings.Add(data);
                _CiplatformDbContext.SaveChanges();
                return true;
            }
            else
            {
                var data = new MissionRating()
                {
                    Rating = rate,
                };
                entry.First().Rating = rate;
                entry.First().UpdatedAt = DateTime.Now;
                _CiplatformDbContext.SaveChanges();
                return true;
            }
        }

        public List<CommentViewModel> getComment(long missionId)
        {
            List<Comment> comments = _CiplatformDbContext.Comments.Where(c => c.MissionId == missionId && c.ApprovalStatus == true).ToList();
            List<CommentViewModel> commentView = new List<CommentViewModel>();

            foreach (var comment in comments)
            {
                CommentViewModel commentList = new CommentViewModel();
                User user = _CiplatformDbContext.Users.FirstOrDefault(a => a.UserId == comment.UserId);
                commentList.Comment1 = comment.Comment1;
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
            newComment.Comment1 = comment;
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


        public void AddToRecent(long missionId, long userId)
        {
            MissionApplication missionapplication = new MissionApplication();
            missionapplication.UserId = userId;
            missionapplication.MissionId = missionId;
            missionapplication.AppliedAt = DateTime.Now;
            missionapplication.ApprovalStatus = "APPROVE";
            _CiplatformDbContext.MissionApplications.Add(missionapplication);
            _CiplatformDbContext.SaveChanges();
        }
        public List<MissionApplication> getMissionApplicant(long mission_id)
        {
            List<MissionApplication> missionapplications = _CiplatformDbContext.MissionApplications.Where(c => c.MissionId == mission_id && c.ApprovalStatus == "APPROVE").ToList();
            return missionapplications;
        }
        public bool Recommend(long user_id, long mission_id, List<long> co_workers)
        {
            foreach (var user in co_workers)
            {
                _CiplatformDbContext.MissionInvites.Add(new MissionInvite
                {
                    FromUserId = user_id,
                    ToUserId = Convert.ToInt64(user),
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
            var imagePath = _CiplatformDbContext.StoryMedia.FirstOrDefault(u => u.StoryId == storyId && u.Type == "png");
            return imagePath.Path;
        }



        /*public string GetType(long storyId)
        {
            var type = _CiplatformDbContext.StoryMedia.FirstOrDefault(u => u.StoryId == storyId);
            return type.Type;
        }*/



        public List<StoryViewModel> GetStoryDetails()
        {
            List<Story> PublishedStory = _CiplatformDbContext.Stories.Where(s => s.Status == "PUBLISHED").ToList();


            List<StoryViewModel> stories = new List<StoryViewModel>();

            foreach (var story in PublishedStory)
            {
                Mission mission = _CiplatformDbContext.Missions.FirstOrDefault(a => a.MissionId == story.MissionId);
                StoryViewModel storyInfo = new StoryViewModel();
                User user = _CiplatformDbContext.Users.FirstOrDefault(a => a.UserId == story.UserId);


                storyInfo.StoryId = story.StoryId;
                storyInfo.MissionId = mission.MissionId;
                storyInfo.CountryId = mission.CountryId;
                storyInfo.ThemeId = mission.ThemeId;
                var skillId = _CiplatformDbContext.MissionSkills.FirstOrDefault(a => a.MissionId == story.MissionId);
                storyInfo.SkillId = skillId.SkillId;
                storyInfo.Theme = _CiplatformDbContext.MissionThemes.FirstOrDefault(a => a.MissionThemeId == _CiplatformDbContext.Missions.FirstOrDefault(a => a.MissionId == story.MissionId).ThemeId).Title;
                storyInfo.Title = story.Title;
                storyInfo.Description = story.Description;
                storyInfo.storyImage = getStoryImg(story.StoryId);
                storyInfo.Avatar = user.Avatar;
                storyInfo.UserName = user.FirstName + " " + user.LastName;
                stories.Add(storyInfo);
            }

            return stories;
        }

        public List<StoryViewModel> GetStoryList(string? search, string[] countries, string[] cities, string[] themes, string[] skills, int paging)
        {
            var pageSize = 6;

            List<StoryViewModel> stories = GetStoryDetails();
            if (search != "")
            {
                stories = stories.Where(a => a.Title.ToLower().Contains(search)).ToList();
            }
            if (countries.Length > 0)
            {
                stories = stories.Where(a => countries.Contains(a.CountryId.ToString())).ToList();
            }
            if (cities.Length > 0)
            {
                stories = stories.Where(a => cities.Contains(a.CityId.ToString())).ToList();
            }
            if (themes.Length > 0)
            {
                stories = stories.Where(a => themes.Contains(a.ThemeId.ToString())).ToList();
            }
            if (skills.Length > 0)
            {
                stories = stories.Where(a => skills.Contains(a.SkillId.ToString())).ToList();
            }
            if (paging != null)
            {
                stories = stories.Skip((paging - 1) * pageSize).Take(6).ToList();
            }
            return stories;
        }

        public List<Mission> getStoryMission(long userid)
        {
            var storymission = _CiplatformDbContext.MissionApplications.Where(i => i.UserId == userid && i.ApprovalStatus == "APPROVE").ToList();
            var story = new List<long>();
            foreach (var item in storymission)
            {
                story.Add(item.MissionId);
            }
            var storymissions = _CiplatformDbContext.Missions.Where(i => story.Contains(i.MissionId)).ToList();
            return storymissions;
        }
        public void AddStory(StoryViewModel model, long UserId, string submit)
        {
            //model.UserId = UserId;
            if (model.StoryId != null)
            {
                var updateStory = _CiplatformDbContext.Stories.FirstOrDefault(s => s.StoryId == model.StoryId);
                updateStory.UserId = UserId;
                updateStory.MissionId = model.MissionId;
                updateStory.Description = model.Description;
                updateStory.Title = model.Title;
                updateStory.Status = "DRAFT";
                if (submit == "Submit")
                {
                    updateStory.Status = "PUBLISHED";
                }
                updateStory.PublishedAt = model.PublishDate;
                updateStory.UpdatedAt = DateTime.Now;
                _CiplatformDbContext.Update(updateStory);
                _CiplatformDbContext.SaveChanges();

                if (model.Url[0] != null)
                {

                    List<string> url = model.Url;
                    foreach (var i in url)
                    {
                        StoryMedium storyMedium = new StoryMedium();
                        storyMedium.StoryId = updateStory.StoryId;
                        storyMedium.Type = "mp4";
                        storyMedium.Path = i;
                        _CiplatformDbContext.StoryMedia.Update(storyMedium);

                    }
                    _CiplatformDbContext.SaveChanges();
                }
                string[] imgs = model.imgs.Split(",");
                int count = 1;
                foreach (var i in imgs)
                {
                    if (i.Length < 300)
                    {
                        count++;
                    }
                    else
                    {
                        StoryMedium storyMedium = new StoryMedium();
                        storyMedium.StoryId = _CiplatformDbContext.Stories.FirstOrDefault(u => u.UserId == UserId && u.MissionId == model.MissionId).StoryId;
                        storyMedium.Type = "png";
                        string filename = $"story-{updateStory.StoryId}-image-{count}.png";
                        storyMedium.Path = filename;
                        _CiplatformDbContext.StoryMedia.Add(storyMedium);
                        _CiplatformDbContext.SaveChanges();
                        if (i.Length > 0)
                        {
                            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/CI Platform Assets/StoryImages", filename);
                            // filePath.Add(path);
                            byte[] bytes = Convert.FromBase64String(i);
                            File.WriteAllBytes(path, bytes);
                        }
                        count++;
                    }
                }

            }
            else
            {
                if (model.Description != null)
                {
                    Story addData = new Story();

                    {
                        addData.UserId = UserId;
                        addData.MissionId = model.MissionId;
                        addData.Description = model.Description;
                        addData.Title = model.Title;
                        addData.Status = "DRAFT";
                        if (submit == "Submit")
                        {
                            addData.Status = "PUBLISHED";
                        }

                        addData.PublishedAt = model.PublishDate;

                        _CiplatformDbContext.Add(addData);
                        _CiplatformDbContext.SaveChanges();

                        if (model.Url[0] != null)
                        {

                            List<string> url = model.Url;
                            foreach (var i in url)
                            {
                                StoryMedium storyMedium = new StoryMedium();
                                storyMedium.StoryId = addData.StoryId;
                                storyMedium.Type = "mp4";
                                storyMedium.Path = i;
                                _CiplatformDbContext.StoryMedia.Add(storyMedium);

                            }
                            _CiplatformDbContext.SaveChanges();
                        }

                        string[] imgs = model.imgs.Split(",");
                        int count = 1;
                        foreach (var i in imgs)
                        {
                            StoryMedium storyMedium = new StoryMedium();
                            storyMedium.StoryId = _CiplatformDbContext.Stories.FirstOrDefault(u => u.UserId == UserId && u.MissionId == model.MissionId).StoryId;
                            storyMedium.Type = "png";
                            string filename = $"story-{addData.StoryId}-image-{count}.png";
                            storyMedium.Path = filename;
                            _CiplatformDbContext.StoryMedia.Add(storyMedium);
                            _CiplatformDbContext.SaveChanges();
                            if (i.Length > 0)
                            {
                                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/CI Platform Assets/StoryImages", filename);
                                
                                byte[] bytes = Convert.FromBase64String(i);
                                File.WriteAllBytes(path, bytes);
                            }
                            count++;
                        }

                    }


                }
            }
        }
      

        public StoryViewModel getStory(long story_id, long user_id)
        {
            List<StoryMedium> medias = _CiplatformDbContext.StoryMedia.ToList();
            List<StoryView> views = _CiplatformDbContext.StoryViews.ToList();
            if(_CiplatformDbContext.StoryViews.FirstOrDefault(c => c.StoryId == story_id && c.UserId == user_id) is null)
            {
                _CiplatformDbContext.StoryViews.Add(new StoryView
                {
                    UserId = user_id,
                    StoryId = story_id,
                });
                _CiplatformDbContext.SaveChanges();
            }

            Story story = _CiplatformDbContext.Stories.FirstOrDefault(s => s.StoryId == story_id);
            var whyIVolunteer = _CiplatformDbContext.Users.FirstOrDefault(u => u.UserId == story.UserId).WhyIVolunteer;
           


            if (story == null)
            {
                return null;
            }
            else
            {
                StoryViewModel mystory = new StoryViewModel()
                {
                    MissionId = story.MissionId,
                    StoryId = story_id,
                    Title = story.Title,
                    Description = story.Description,
                    storymedia = story.StoryMedia.ToList(),
                    users = _CiplatformDbContext.Users.ToList(),
                    Avatar = story.User.Avatar,
                    UserName = story.User.FirstName + " " + story.User.LastName,
                    Views = story.StoryViews.Count(),
                    WhyIVolunteer = whyIVolunteer
                };
                return mystory;
            }
        }
        public StoryViewModel GetDraftDetails(long mission_id, long userId)
        {
            Story get_draft = _CiplatformDbContext.Stories.FirstOrDefault(m => m.MissionId == mission_id && m.Status == "DRAFT" && m.UserId == userId);
            if(get_draft != null)
            {
                List<StoryMedium> mediaImages = new List<StoryMedium>();
                var imageList = _CiplatformDbContext.StoryMedia.Where(m => m.StoryId == get_draft.StoryId && m.Type == "png").ToList();
                foreach (var image in imageList)
                {
                    StoryMedium items = new StoryMedium();
                    items.Path = image.Path;
                    mediaImages.Add(items);
                }
                StoryViewModel draft_model = new StoryViewModel();
                {
                    draft_model.Title = get_draft.Title;
                    draft_model.PublishDate = get_draft.PublishedAt;
                    draft_model.Description = get_draft.Description;
                    draft_model.storymedia = mediaImages;
                    draft_model.StoryId = get_draft.StoryId;
                }
                return draft_model;
            }
            return null;
        }
        /*-----------------------Volunteering Timesheet------------------------------------------------------------*/
        public TimesheetViewModel GetSheetDetails(long userId)
        {
            var getSheetDetails = _CiplatformDbContext.Timesheets.Where(u => u.UserId == userId && u.DeletedAt == null).Include(x => x.Mission).ToList();
            var getMission = _CiplatformDbContext.MissionApplications.Where(m => m.UserId == userId && m.ApprovalStatus == "APPROVE").Include(x => x.Mission).ToList();
            var model = new TimesheetViewModel();
            {
                model.TimeMission = getMission.Where(x => x.Mission.MissionType == true).ToList();
                model.GoalMission = getMission.Where(x => x.Mission.MissionType == false).ToList();

                model.TimeSheets = getSheetDetails.Where(m => m.Mission?.MissionType == true).ToList();
                model.GoalSheets = getSheetDetails.Where(m => m.Mission?.MissionType == false).ToList();
            }

            return model;
        }

        public bool addTimeSheet(long mission_id, long user_id, DateTime date, int hour, int minutes, string message, long timeSheetId)
        {
            if (timeSheetId == 0)
            {
                var addTime = _CiplatformDbContext.Timesheets.Add(new Timesheet
                {
                    Time = new TimeSpan(hour, minutes, 0),
                    UserId = user_id,
                    MissionId = mission_id,
                    Status = "SUBMIT_FOR_APPROVAL",
                    DateVolunteered = date,
                    Notes = message,
                    CreatedAt = DateTime.Now,
                });

                _CiplatformDbContext.SaveChanges();
                return true;
            }
            else
            {
                var update = _CiplatformDbContext.Timesheets.FirstOrDefault(m => m.TimesheetId == timeSheetId);
                update.MissionId = mission_id;
                update.UserId = user_id;
                update.DateVolunteered = date;
                update.Time = new TimeSpan(hour, minutes, 0);
                update.Notes = message;
                update.UpdatedAt = DateTime.Now;

                _CiplatformDbContext.Timesheets.Update(update);
                _CiplatformDbContext.SaveChanges();
                return true;
            }

        }

        public bool addGoalSheet(long mission_id, long user_id, DateTime date, int action, string message, long timeSheetId)
        {
            if (timeSheetId == 0)
            {
                var addTime = _CiplatformDbContext.Timesheets.Add(new Timesheet
                {
                    //Time = new TimeSpan(hour, minutes, 0),
                    UserId = user_id,
                    MissionId = mission_id,
                    Status = "SUBMIT_FOR_APPROVAL",
                    Action = action,
                    DateVolunteered = date,
                    Notes = message,
                    CreatedAt = DateTime.Now,
                });

                _CiplatformDbContext.SaveChanges();
                return true;
            }
            else
            {
                var update = _CiplatformDbContext.Timesheets.FirstOrDefault(m => m.TimesheetId == timeSheetId);
                update.UserId = user_id;
                update.MissionId = mission_id;
                update.Action = action;
                update.DateVolunteered = date;
                update.Notes = message;
                update.UpdatedAt = DateTime.Now;

                _CiplatformDbContext.Timesheets.Update(update);
                _CiplatformDbContext.SaveChanges();
                return true;
            }

        }

        public Timesheet getTimeSheet(long timeSheetId)
        {
            var timesheet = _CiplatformDbContext.Timesheets.FirstOrDefault(t => t.TimesheetId == timeSheetId);
            return timesheet;
        }
        public bool deleteTimeSheet(long timeSheetId)
        {
            var delete = _CiplatformDbContext.Timesheets.FirstOrDefault(u => u.TimesheetId == timeSheetId);
            delete.DeletedAt = DateTime.Now;
            _CiplatformDbContext.SaveChanges();
            return true;
        }
        public bool CheckSameDate(long userId, long missionId, DateTime newDate)
        {
            var sameDate = _CiplatformDbContext.Timesheets.Where(date => date.UserId == userId && date.MissionId == missionId && date.DateVolunteered == newDate).ToList();
            if (sameDate.Count != 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        

    }
}
