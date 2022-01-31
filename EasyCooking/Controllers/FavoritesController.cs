using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EasyCooking.Repositories;
using EasyCooking.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace EasyCooking.Controllers
{
    public class FavoritesController : Controller
    {
        private readonly IFavoriteRepository _favoriteRepository;
        public FavoritesController(IFavoriteRepository favoriteRepository)
        {
            _favoriteRepository = favoriteRepository;
        }
        // GET: FavoritesController
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        public IActionResult Create(int id)
        {
            Favorites favorite = new Favorites
            {
                RecipeId = id,
                UserProfileId = GetCurrentUserProfileId()
            };
            _favoriteRepository.Add(favorite);
            return RedirectToAction("Index", "Recipe");
        }

        // GET: FavoritesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: FavoritesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FavoritesController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: FavoritesController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: FavoritesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FavoritesController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: FavoritesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
