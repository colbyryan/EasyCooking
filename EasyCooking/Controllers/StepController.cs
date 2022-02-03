using EasyCooking.Models;
using EasyCooking.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyCooking.Controllers
{
    public class StepController : Controller
    {
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IRecipeRepository _recipeRepository;
        private readonly IStepRepository _stepRepository;
        public StepController(IStepRepository stepRepository, IIngredientRepository ingredientRepository, IRecipeRepository recipeRepository)
        // GET: RecipeController
        {
            _ingredientRepository = ingredientRepository;
            _recipeRepository = recipeRepository;
            _stepRepository = stepRepository;
        }
        // GET: StepController
        public ActionResult Index(int id)
        {
            var steps = _stepRepository.GetAllByRecipeId(id);
            return View(steps);
        }

        // GET: StepController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: StepController/Create
        public ActionResult Create(int id)
        {
            var recipe = _recipeRepository.GetById(id);
            ViewData["RecipeName"] = recipe.Title;
            ViewData["RecipeId"] = recipe.Id;
            return View();
        }

        // POST: StepController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int id, Step step)
        {
            try
            {
                step.RecipeId = id;
                _stepRepository.Add(step);
                return RedirectToAction("Details", "Recipe", new { id });
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: StepController/Edit/5
        public ActionResult Edit(int id)
        {
            Step step = _stepRepository.GetById(id);
            return View(step);
        }

        // POST: StepController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Step step)
        {
            try
            {
                step.Id = id;
                _stepRepository.UpdateStep(step);
                return RedirectToAction("Details", "Recipe", new { id = step.RecipeId });
            }
            catch
            {
                return View();
            }
        }

        // GET: StepController/Delete/5
        public ActionResult Delete(int id)
        {
            var step = _stepRepository.GetById(id);
            int recipeId = step.RecipeId;
            ViewData["RecipeId"] = recipeId;
            return View(step);
        }

        // POST: StepController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Step step)
        {
            try
            {
                var stepRecipeId = _stepRepository.GetById(id);
                int recipeId = stepRecipeId.RecipeId;
                ViewData["RecipeId"] = recipeId;
                _stepRepository.Remove(id);
                return RedirectToAction("Details", "Recipe", new { id = recipeId });
            }
            catch
            {
                return View();
            }
        }
    }
}
