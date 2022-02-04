using EasyCooking.Models;
using EasyCooking.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using System.Security.Claims;

namespace EasyCooking.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserProfileRepository _userProfileRepository;
        public HomeController(IUserProfileRepository userProfileRepository)
        {
//This give us the ability to access different methods inside of the UserProfileRepo
            _userProfileRepository = userProfileRepository;
        }
        
        [Authorize]
        public IActionResult Index()
        {

//Don't forget to ask Josh about adding VideoLink to the DB or not :3

            var userProfileId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var userProfile = _userProfileRepository.GetById(userProfileId);
            ViewData["IsAdmin"] = userProfile.UserTypeId == 1;
            return View(userProfile);
//This will return the "Index View" and pass in the user that we got by Id and stored in userProfile
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
