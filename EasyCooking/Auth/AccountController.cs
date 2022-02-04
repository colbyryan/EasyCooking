using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using EasyCooking.Auth.Models;
using EasyCooking.Repositories;
using EasyCooking.Models;
using Microsoft.Extensions.Configuration;

namespace EasyCooking.Auth
{
    public class AccountController : Controller
    {
        private readonly IFirebaseAuthService _firebaseAuthService;
        private readonly IUserProfileRepository _userProfileRepository;

        public AccountController(IFirebaseAuthService firebaseAuthService, IUserProfileRepository userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
            _firebaseAuthService = firebaseAuthService;
        }

        public IActionResult Login()
        {
            ViewData["IsAdmin"] = false;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Credentials credentials)
        {
            if (!ModelState.IsValid)
            {

                return View(credentials);
            }

            var fbUser = await _firebaseAuthService.Login(credentials);
            if (fbUser == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return View(credentials);
            }

            var userProfile = _userProfileRepository.GetByFirebaseUserId(fbUser.FirebaseUserId);
            if (userProfile == null)
            {
                ModelState.AddModelError(string.Empty, "Unable to Login.");
                return View(credentials);
            }

            await LoginToApp(userProfile);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Registration registration)
        {
            if (!ModelState.IsValid)
            {
                return View(registration);
            }

            var fbUser = await _firebaseAuthService.Register(registration);

            if (fbUser == null)
            {
                ModelState.AddModelError(string.Empty, "Unable to register, do you already have an account?");
                return View(registration);
            }

            var newUserProfile = new UserProfile
            {
                Email = fbUser.Email,
                FirebaseUserId = fbUser.FirebaseUserId,
                FirstName = registration.FirstName,
                LastName  = registration.LastName,
                DisplayName = registration.DisplayName,
            };
            _userProfileRepository.Add(newUserProfile);

            await LoginToApp(newUserProfile);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            var GetCurrentUserId = GetCurrentUserProfileId();
            var CurrentUser = _userProfileRepository.GetById(GetCurrentUserId);
            ViewData["IsAdmin"] = CurrentUser.UserTypeId == 1;
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        private async Task LoginToApp(UserProfile userProfile)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userProfile.Id.ToString()),
                new Claim(ClaimTypes.Email, userProfile.Email),
                new Claim(ClaimTypes.Role, userProfile.UserTypeId == 1 ? "Admin" : "Viewer")
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));
        }
        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
