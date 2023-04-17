using CI_PlatForm.Entities.Data;
using CI_PlatForm.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_PlatForm.Repository.Repositories
{
    public class AdminRepository
    {
        private readonly CiplatformContext _CiplatformDbContext;
        public AdminRepository(CiplatformContext CiplatformDbContext)
        {
            _CiplatformDbContext = CiplatformDbContext;
        }
        public List<User> getUserList()
        {
            var user = _CiplatformDbContext.Users.ToList();
            return user;
        }
    }
}
