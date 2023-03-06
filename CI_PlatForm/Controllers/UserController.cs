
using CI_Platform.Entities.Data;
using CI_Platform.Entities.Models;
using CI_Platform.Entities.ViewModel;
using CI_Platform.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CI_PlatForm.Controllers
{
    public class UserController : Controller
    {
        public readonly CiplatformContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration configuration;



        private readonly IUserRepository _UserRepository;
        public UserController(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor, IConfiguration _configuration,CiplatformContext db)
        {
            _UserRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            configuration = _configuration;
            _db = db;
        }

        public IActionResult UserList()
        {
            var listOfUsers = _UserRepository.UserList();
            return View(listOfUsers);
        }

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
               HttpContext.Session.SetString("username",objUser.FirstName+" "+objUser.LastName);
                return RedirectToAction("PlatformLanding", "Home");
            }
            {
                return NotFound("User not Found");
            }
            return View();

        }

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
                return (RedirectToAction("PlatformLanding", "User"));
            }
            return RedirectToAction("Registration", "User");
        }
    

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
       
   
        
    }
}
