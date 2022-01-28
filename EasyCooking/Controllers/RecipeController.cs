using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EasyCooking.Repositories;
using EasyCooking.Models;
using System.Collections.Generic;
using EasyCooking.Models.ViewModels;
using System.Security.Claims;
using System;

namespace EasyCooking.Controllers
{
    public class RecipeController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IRecipeRepository _recipeRepository;
        public RecipeController(IRecipeRepository recipeRepository, ICategoryRepository categoryRepository)
        // GET: RecipeController
            {
            _categoryRepository = categoryRepository;
            _recipeRepository = recipeRepository;
            }
        public ActionResult Index()
        {
            List<Recipe> recipe = _recipeRepository.GetAll();
            return View(recipe);
        }

        // GET: RecipeController/Details/5
        public ActionResult Details(int id)
        {
            var recipe = _recipeRepository.GetById(id);
            if (recipe != null)
            {
                return View(recipe);
            } 
            else
            {
                return NotFound();
            }
        }


        // GET: RecipeController/Create
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
            return View();
        }

        // POST: RecipeController/Delete/5
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
