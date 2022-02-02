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
        public ActionResult Create()
        {
            return View();
        }

        // POST: StepController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: StepController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: StepController/Edit/5
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

        // GET: StepController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: StepController/Delete/5
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
    }
}
