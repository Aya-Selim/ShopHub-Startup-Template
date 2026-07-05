using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myshop.Business.DTOs;
using myshop.Business.Services.IServices;

namespace myshop.Web.Areas.Admin.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
            var categories = _categoryService.GetAllCategories();
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CategoryDto categoryDto)
        {
            if (ModelState.IsValid)
            {
                _categoryService.CreateCategory(categoryDto);
                TempData["Create"] = "Item has Created Successfully";
                return RedirectToAction("Index");
            }
            return View(categoryDto);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var categoryDto = _categoryService.GetCategoryById(id.Value);
            if (categoryDto == null)
            {
                return NotFound();
            }
            return View(categoryDto);
        }

        [HttpPost]
        public IActionResult Edit(CategoryDto categoryDto)
        {
            if (ModelState.IsValid)
            {
                _categoryService.UpdateCategory(categoryDto);
                TempData["Update"] = "Data has Updated Successfully";
                return RedirectToAction("Index");
            }
            return View(categoryDto);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var categoryDto = _categoryService.GetCategoryById(id.Value);
            if (categoryDto == null)
            {
                return NotFound();
            }
            return View(categoryDto);
        }

        [HttpPost]
        public IActionResult DeleteCategory(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var categoryDto = _categoryService.GetCategoryById(id.Value);
            if (categoryDto == null)
            {
                return NotFound();
            }
            _categoryService.DeleteCategory(id.Value);
            TempData["Delete"] = "Item has Deleted Successfully";
            return RedirectToAction("Index");
        }
    }
}
