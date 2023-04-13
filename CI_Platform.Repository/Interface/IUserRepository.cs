using CI_PlatForm.Entities.Models;
using CI_PlatForm.Entities.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_PlatForm.Repository.Interface
{
    public interface IUserRepository
    {
        public List<User> UserList();
        public void Registration(User objUser);

        public Boolean IsEmailAvailable(string email);

        public User IsPasswordAvailable(string password, string email);

        public long GetUserID(string Email);


        public bool ResetPassword(long userId, string OldPassword, string NewPassword);

        public Boolean ChangePassword(long UserId, Reset_Password model);
        public ProfileViewModel getProfile(long UserId);
        public bool changePassword(string oldPass, string newPass, long userId);
        public bool addProfile(ProfileViewModel ViewModel, long userId, int cityId);
  


    }
}
