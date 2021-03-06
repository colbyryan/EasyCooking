using EasyCooking.Repositories;
using EasyCooking.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyCooking.Controllers
{
    public class IngredientController : Controller
    {
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IRecipeRepository _recipeRepository;
        public IngredientController(IIngredientRepository ingredientRepository, IRecipeRepository recipeRepository)
        // GET: RecipeController
        {
            _ingredientRepository = ingredientRepository;
            _recipeRepository = recipeRepository;
        }
        // GET: IngredientController
        [HttpGet]
        public ActionResult Index(int id)
        {
            var ingredients = _ingredientRepository.GetAllByRecipeId(id);
            return View(ingredients);
        }

        // GET: IngredientController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        [HttpGet]
        // GET: IngredientController/Create
        public ActionResult Create(int id)
        {
            var recipe = _recipeRepository.GetById(id);
            ViewData["RecipeName"] = recipe.Title;
            ViewData["RecipeId"] = recipe.Id;
            return View();
        }

        // POST: IngredientController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int id, Ingredient ingredient)
        {
            try
            {
                ingredient.RecipeId = id;
                _ingredientRepository.Add(ingredient);
                return RedirectToAction("Details", "Recipe", new { id });
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: IngredientController/Edit/5
        public ActionResult Edit(int id)
        {
            Ingredient ingredient = _ingredientRepository.GetById(id);          
            return View(ingredient);
        }

        // POST: IngredientController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Ingredient ingredient)
        {
            try
            {
                ingredient.Id = id;
                _ingredientRepository.UpdateIngredient(ingredient);
                return RedirectToAction("Details","Recipe", new { id = ingredient.RecipeId });
            }
            catch
            {
                return View();
            }
        }

        // GET: IngredientController/Delete/5
        public ActionResult Delete(int id)
        {
            var ingredient = _ingredientRepository.GetById(id);
            int recipeId = ingredient.RecipeId;
            ViewData["RecipeId"] = recipeId;
            return View(ingredient);
        }

        // POST: IngredientController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Ingredient ingredient)
        {
            try
            {
                var ingredientRecipeId = _ingredientRepository.GetById(id);
                int recipeId = ingredientRecipeId.RecipeId;
                ViewData["RecipeId"] = recipeId;
                _ingredientRepository.Remove(id);
                return RedirectToAction("Details", "Recipe", new { id = recipeId });
            }
            catch
            {
                return View();
            }
        }
    }
}
