﻿

using CI_PlatForm.Entities.Data;
using CI_PlatForm.Entities.Models;
using CI_PlatForm.Entities.ViewModel;
using CI_PlatForm.Repository.Interface;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace CI_PlatForm.Repository.Repositories
{
    public class UserRepository : IUserRepository
    {
        public readonly CiplatformContext _CiplatformDbContext;
    

        public UserRepository(CiplatformContext ciplatformDbContext)
        {
            _CiplatformDbContext = ciplatformDbContext;
         
        }
        public List<User> UserList()
        {
            List<User> objUserList = _CiplatformDbContext.Users.ToList();
            return objUserList;
        }
 
        public Boolean IsEmailAvailable(string email)
        {
            return _CiplatformDbContext.Users.Any(x => x.Email == email);
        }

        public User IsPasswordAvailable(string password, string email)
        {
            return _CiplatformDbContext.Users.Where(x => x.Password == password && x.Email == email).FirstOrDefault();
        }



        public long GetUserID(string Email)
        {
            User user = _CiplatformDbContext.Users.Where(x => x.Email == Email).FirstOrDefault();
            if (user == null)
            {

                return -1;
            }
            else
            {

                return user.UserId;
            }
        }
        public void Registration(User objUser)
        
            
            {
                _CiplatformDbContext.Users.Add(objUser);
                _CiplatformDbContext.SaveChanges();
            }
        

        public bool ResetPassword(long userId, string OldPassword, string NewPassword)
        {
            try
            {
                User user = _CiplatformDbContext.Users.Find(userId);
                string pass = (user.Password);
                if (pass == OldPassword)
                {
                    user.Password = (NewPassword);
                    _CiplatformDbContext.Users.Update(user);
                    _CiplatformDbContext.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public Boolean ChangePassword(long UserId, Reset_Password model)
        {
            User user = _CiplatformDbContext.Users.FirstOrDefault(x => x.UserId == model.UserId);
            user.Password = model.Password;
            user.UpdatedAt = DateTime.Now;
            _CiplatformDbContext.Users.Update(user);
            _CiplatformDbContext.SaveChanges();
            return true;

        }
        public User GetUser(int userID)
        {
            try
            {

                User user = _CiplatformDbContext.Users.Where(x => x.UserId == userID).FirstOrDefault();
                if (user != null)
                {
                    return user;
                }
                else
                {

                    return null;

                }


            }
            catch (Exception ex)
            {

                return null;
            }
        }
        
        public ProfileViewModel getProfile(long userId)
        {
            User user = _CiplatformDbContext.Users.FirstOrDefault(u => u.UserId == userId);
            ProfileViewModel model = new ProfileViewModel();
            {
                model.CountryId = user.CountryId;
                model.CityId = user.CityId;
                model.Avatar = user.Avatar;
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;
                model.WhyIVolunteer = user.WhyIVolunteer; 
                model.EmployeeId = user.EmployeeId;
                model.Title = user.Title;
                model.Department = user.Department;
                model.ProfileText = user.ProfileText;
                model.userSkills = _CiplatformDbContext.UserSkills.Where(a => a.UserId == userId).ToList();
                
            }
            return model;
        }
        public bool addProfile(ProfileViewModel ViewModel, long userId,int cityId)
        {
            User model = _CiplatformDbContext.Users.FirstOrDefault(u => u.UserId == userId);
            if(ViewModel.profile != null && !string.IsNullOrEmpty(ViewModel.profile.FileName))
            {
                model.Avatar = ViewModel.profile.FileName;
            }
          
            model.FirstName = ViewModel.FirstName;
            model.LastName= ViewModel.LastName;
            model.EmployeeId= ViewModel.EmployeeId;
            model.Title= ViewModel.Title;
            model.Department = ViewModel.Department;
            model.ProfileText= ViewModel.ProfileText;
            model.WhyIVolunteer = ViewModel.WhyIVolunteer;
            model.CountryId = ViewModel.CountryId;
            model.CityId = ViewModel.CityId;
            model.LinkedInUrl = ViewModel.LinkedInUrl;
         
            if(!string.IsNullOrEmpty(ViewModel.selected_skills) && ViewModel.selected_skills.Contains(","))
            {
               string[] skills = ViewModel.selected_skills.Split(',');
                foreach (string skill in skills)
                {
                   
                    if (!_CiplatformDbContext.UserSkills.Any(us => us.UserId == userId && us.SkillId == int.Parse(skill))){
                        _CiplatformDbContext.UserSkills.Add(new UserSkill
                        {
                            SkillId = int.Parse(skill),
                            UserId = userId
                        });
                    
                    }
                
                }
            }
         
            _CiplatformDbContext.Users.Update(model);
            _CiplatformDbContext.SaveChanges();
            return true;
        }
        public bool changePassword(string oldPass, string newPass, long userId)
        {
            User userList = _CiplatformDbContext.Users.FirstOrDefault(u => u.UserId == userId);
            if(oldPass == userList.Password)
            {
                userList.Password = newPass;
                _CiplatformDbContext.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }

        }
        public bool contactUs(string name, string email, string subject, string message, long userId)
        {
            var contact = _CiplatformDbContext.ContactUs.Add(new ContactU
            {
                Name = name,
                Email = email,
                Subject = subject,
                Message = message,
                UserId = userId
            });
            _CiplatformDbContext.SaveChanges();
            return true;
        }


    }



}
