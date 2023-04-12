


using CI_PlatForm.Entities.Data;
using CI_PlatForm.Entities.Models;
using CI_PlatForm.Entities.ViewModel;
using CI_PlatForm.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CI_PlatForm.Controllers
{
    public class UserController : Controller
    {
        public readonly CiplatformContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration configuration;



        private readonly IUserRepository _UserRepository;
        private readonly IMissionRepository _MissonRepository;
        public UserController(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor, IConfiguration _configuration,CiplatformContext db,IMissionRepository MissionRepository)
        {
            _UserRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            configuration = _configuration;
            _db = db;
            _MissonRepository = MissionRepository;
        }

        public IActionResult UserList()
        {
            var listOfUsers = _UserRepository.UserList();
            return View(listOfUsers);
        }
        //---------------------------------Login control---------------------------------------------------------//
        public IActionResult Index()
        {
           return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(User objLogin)
        {


            var objUser = _db.Users.FirstOrDefault(u => u.Email == objLogin.Email && u.Password == objLogin.Password);

            if (objUser != null)
            {

                HttpContext.Session.SetString("username", objUser.FirstName + " " + objUser.LastName);
                HttpContext.Session.SetString("userId", objUser.UserId.ToString());
                return RedirectToAction("PlatformLanding", "Mission");
            }
           else {
                return NotFound("User not Found");
            }
            return View();

        }
  //-----------------------------------------------------------------------------Registration Control------------------------------------------------//
        public IActionResult Registration()
        {
           
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Registration(User objUser)
        {
            
            
            var objReg = _UserRepository.UserList().Exists(u => u.Email.Equals(objUser.Email)); 
            if(objReg == true)
            {
                return View();
            }

            if (objUser.Password == objUser.ConfirmPassword)
            {
                _UserRepository.Registration(objUser);
               /* return (RedirectToAction("Index", "User"));*/
            }
            return RedirectToAction("Index", "User");
        }
    
        //---------------------------------------------------------Forgot Password control-----------------------------------------------------------------//
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ValidateForgotDetails(ForgotPassword fpm)
        {
            if (_UserRepository.IsEmailAvailable(fpm.email))
            {
                try
                {
                    long UserId = _UserRepository.GetUserID(fpm.email);
                    string welcomeMessage = "Welcome to CI platform, <br/> You can Reset your password using below link. <br/>";
                    // string path = "<a href=\"" + " https://" + _httpContextAccessor.HttpContext.Request.Host.Value + "/Account/Reset_Password/" + UserId.ToString() + " \"  style=\"font-weight:500;color:blue;\" > Reset Password </a>";
                    string path = "<a href=\"https://" + _httpContextAccessor.HttpContext.Request.Host.Value + "/User/Reset/" + UserId.ToString() + "\"> Reset Password </a>";
                    MailHelper mailHelper = new MailHelper(configuration);
                    ViewBag.sendMail = mailHelper.Send(fpm.email, welcomeMessage + path);
                    ModelState.Clear();
                    return RedirectToAction("Index", new { UserId = UserId });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            else
            {
                ModelState.AddModelError("email", "Plase Enter Register Email Address..");
                ViewBag.isForgetPasswordOpen = true;
                return View("ForgotPassword");
            }
            return View("Index");
         }

    //----------------------------------------------------Reset password control-------------------------------------------------------------------//
        [HttpGet]
        public IActionResult Reset(long id)
        {
            Reset_Password model = new Reset_Password();
            model.UserId = id;
            return View(model);
        }

        [HttpPost]
        public IActionResult Reset(Reset_Password model, long id)
        {
            if (ModelState.IsValid)
            {

                if (_UserRepository.ChangePassword(id, model))
                {
                    ModelState.Clear();
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    ModelState.AddModelError("", "Enter Same Password");
                }
            }

            return View();
        }





        public IActionResult Reset()
        {
            return View();
        }

       
        public IActionResult UserDetail()
        {
            long userId = (long)Convert.ToInt64(HttpContext.Session.GetString("userId"));
            ViewBag.sessionValue = HttpContext.Session.GetString("username");
            ProfileViewModel model = _UserRepository.getProfile(userId);
            model.countries = _MissonRepository.GetCountryData();
            model.skill = _MissonRepository.GetSkillsList();
            return View(model);
        }
        
       [HttpPost]
       public IActionResult UserDetail(ProfileViewModel model)
        {
            long userId = (long)Convert.ToInt64(HttpContext.Session.GetString("userId"));
            ViewBag.sessionValue = HttpContext.Session.GetString("username");
            bool addUser = _UserRepository.addProfile(model, userId);
            return RedirectToAction("UserDetail");

        }
        public IActionResult changePassword(String oldpassword, String newpassword)
        {
            long userId = (long)Convert.ToInt64(HttpContext.Session.GetString("userId"));
            ViewBag.sessionValue = HttpContext.Session.GetString("username");
            var updatePass = _UserRepository.changePassword(oldpassword, newpassword, userId);
            return Json(updatePass);
        }
    }
}
