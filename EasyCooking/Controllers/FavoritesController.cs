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
        private readonly IRecipeRepository _recipeRepository;
        public FavoritesController(IFavoriteRepository favoriteRepository, IRecipeRepository recipeRepository)
        {
            _favoriteRepository = favoriteRepository;
            _recipeRepository = recipeRepository;
        }
        // GET: FavoritesController
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Authorize]
        public IActionResult Create(int id)
        {
            Favorites favorite = new Favorites
            {
                RecipeId = id,
                UserProfileId = GetCurrentUserProfileId()
            };
            _favoriteRepository.Add(favorite);
            return RedirectToAction("Details", "Recipe", new { id = id });
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
        //public ActionResult Delete(int id)
        //{
        //    var recipe = _recipeRepository.GetById(id);
        //    ViewData["RecipeTitle"] = recipe.Title;
        //    var f = _favoriteRepository.GetById(id);
        //    return View(f);
        //}

        // POST: FavoritesController/Delete/5
        [Authorize]
        [HttpGet]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                var userId = GetCurrentUserProfileId();
                _favoriteRepository.Delete(id, userId);
                return RedirectToAction("Details", "Recipe", new { id = id });
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
