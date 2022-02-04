using EasyCooking.Models;
using EasyCooking.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EasyCooking.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserProfileRepository _userProfileRepository;
        public CategoryController(ICategoryRepository categoryRepository, IUserProfileRepository userProfileRepository)
        // GET: RecipeController
        {
            _categoryRepository = categoryRepository;
            _userProfileRepository = userProfileRepository;
        }
        // GET: CategoryController
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var ListCategory = _categoryRepository.GetAll();
            return View(ListCategory);
        }

        // GET: CategoryController/Details/5
        public ActionResult Details(int id)
        {
            var category = _categoryRepository.GetCategoryById(id);
            return View(category);
        }

        // GET: CategoryController/Create
        public ActionResult Create()
        {
            int currentUserId = GetCurrentUserProfileId();
            UserProfile user = _userProfileRepository.GetById(currentUserId);
              if (user.UserTypeId == 0)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        // POST: CategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {
            try
            {
                _categoryRepository.CreateCategory(category);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CategoryController/Edit/5
        public ActionResult Edit(int id)
        {
            Category category = _categoryRepository.GetCategoryById(id);
            return View(category);
        }

        // POST: CategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Category category)
        {
            try
            {
                category.Id = id;
                _categoryRepository.Update(category);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(category);
            }

        }

        // GET: CategoryController/Delete/5
        public ActionResult Delete(int id)
        {
            var c = _categoryRepository.GetCategoryById(id);
            return View(c);
        }

        // POST: CategoryController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            {
                try
                {
                    _categoryRepository.Delete(id);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    return View();
                }
            }
        }
        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
