using CI_PlatForm.Entities.Models;
using CI_PlatForm.Entities.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_PlatForm.Repository.Interface
{
    public interface IAdminRepository
    {
        
        public AdminViewModel getAdminList();
        public AdminViewModel getUserDetail(string search, int paging);
        public AdminViewModel getThemeDetail(string search, int paging);
        public AdminViewModel getMissionDetail(string search, int paging);
        public AdminViewModel getCmsDetail(string search, int paging);
        public AdminViewModel getStoryDetail(string search, int paging);
        public AdminViewModel getApplicationDetail(string search, int paging);
        public AdminViewModel getSkillDetail(string search, int paging);
        public bool deleteAdminData(long id, string who);
        



    }
}
