using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EasyCooking.Repositories;
using EasyCooking.Models;
using System.Collections.Generic;
using EasyCooking.Models.ViewModels;
using System.Security.Claims;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace EasyCooking.Controllers
{
    public class RecipeController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IRecipeRepository _recipeRepository;
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IStepRepository _stepRepository;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IFavoriteRepository _faoriteRepository;
        public RecipeController(IRecipeRepository recipeRepository, 
            ICategoryRepository categoryRepository, 
            IIngredientRepository ingredientRepository,
            IStepRepository stepRepository,
            IUserProfileRepository userProfileRepository,
            IFavoriteRepository favoriteRepository)
            
        // GET: RecipeController
            {
            _ingredientRepository = ingredientRepository;
            _categoryRepository = categoryRepository;
            _recipeRepository = recipeRepository;
            _stepRepository = stepRepository;
            _userProfileRepository = userProfileRepository;
            _faoriteRepository = favoriteRepository;
            }
        public ActionResult Index(string searching)
        {
            List<Recipe> recipes = _recipeRepository.GetAll();
            if (String.IsNullOrEmpty(searching))
            {
                return View(recipes);
            }
            else
            {
                return View(recipes.Where(x => x.CategoryName.Contains(searching) || searching == null).ToList());
            }
        }

        // GET: RecipeController/Details/5
        public ActionResult Details(int id)
        {
            var vm = new RecipeViewModel();
            vm.Recipe = _recipeRepository.GetById(id);
            vm.ingredients = _ingredientRepository.GetAllByRecipeId(id);
            vm.steps = _stepRepository.GetAllByRecipeId(id);
            ViewData["IsSubscribed"] = _faoriteRepository.IsSubscribed(GetCurrentUserProfileId(), id);
            if (vm.Recipe != null)
            {
                return View(vm);
            } 
            else
            {
                return NotFound();
            }
        }


        // GET: RecipeController/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            var vm = new RecipeViewModel();
            vm.CategoryOptions = _categoryRepository.GetAll();
            return View(vm);
        }

        // POST: RecipeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RecipeViewModel vm)
        {
            try
            {
                vm.Recipe.UserProfileId = GetCurrentUserProfileId();

                _recipeRepository.Add(vm.Recipe);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                vm.CategoryOptions = _categoryRepository.GetAll();
                return View(vm);
            }
        }

        // GET: RecipeController/Edit/5
        public ActionResult Edit(int id)
        {
            Recipe recipe = _recipeRepository.GetById(id);
            recipe.CategoryOptions = _categoryRepository.GetAll();
            ViewData["RecipeId"] = recipe.Id;
            return View(recipe);
        }

        // POST: RecipeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Recipe recipe)
        {
            try
            {

                _recipeRepository.UpdateRecipe(recipe);
                recipe.CategoryOptions = _categoryRepository.GetAll();
                return RedirectToAction("Index");
            }
            catch
            {
                return View(recipe);
            }
        }

        // GET: RecipeController/Delete/5
        public ActionResult Delete(int id)
        {
            var recipe = _recipeRepository.GetById(id);
            int recipeId = recipe.Id;
            ViewData["RecipeId"] = recipeId;
            return View(recipe);
        }

        // POST: RecipeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Recipe recipe)
        {
            try
            {
                var getRecipeId = _recipeRepository.GetById(id);
                int recipeId = getRecipeId.Id;
                ViewData["RecipeId"] = recipeId;
                _recipeRepository.Remove(id);
                return RedirectToAction("Index");
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
