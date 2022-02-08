using EasyCooking.Models;
using EasyCooking.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using System.Security.Claims;
using System.Linq;
using System;

namespace EasyCooking.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IRecipeRepository _recipeRepository;
        public HomeController(IUserProfileRepository userProfileRepository, IRecipeRepository recipeRepository)
        {
//This give us the ability to access different methods inside of the UserProfileRepo
            _userProfileRepository = userProfileRepository;
            _recipeRepository = recipeRepository;
        }
        
        [Authorize]
        public IActionResult Index(string searching)
        {
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
