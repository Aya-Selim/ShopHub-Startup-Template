using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using myshop.Business.DTOs;
using myshop.Business.Services.IServices;
using myshop.Entities.ViewModels;
using System;
using System.IO;
using System.Linq;

namespace myshop.Web.Areas.Admin.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IProductService productService, ICategoryService categoryService, IWebHostEnvironment webHostEnvironment)
        {
            _productService = productService;
            _categoryService = categoryService;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetData()
        {
            var products = _productService.GetAllProducts()
                .Select(x => new
                {
                    id = x.Id,
                    name = x.Name,
                    description = x.Description,
                    price = x.Price,
                    categoryName = x.CategoryName
                })
                .ToList();

            return Json(new { data = products });
        }

        [HttpGet]
        public IActionResult Create()
        {
            ProductVM productVM = new ProductVM()
            {
                Product = new ProductDto(),
                CategoryList = _categoryService.GetAllCategories().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };
            return View(productVM);
        }

        [HttpPost]
        public IActionResult Create(ProductVM productVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string RootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string filename = Guid.NewGuid().ToString();
                    var Upload = Path.Combine(RootPath, @"Images\Products");
                    var ext = Path.GetExtension(file.FileName);

                    using (var filestream = new FileStream(Path.Combine(Upload, filename + ext), FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }
                    productVM.Product.Img = @"Images\Products\" + filename + ext;
                }
                else
                {
                    // Use a fallback placeholder image to satisfy the non-nullable DB constraint
                    productVM.Product.Img = @"Images\Products\02a7ea31-1096-4acc-99ad-d708a75c6688.jpg";
                }

                _productService.CreateProduct(productVM.Product);
                TempData["Create"] = "Item has Created Successfully";
                return RedirectToAction("Index");
            }

            productVM.CategoryList = _categoryService.GetAllCategories().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            return View(productVM);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var productDto = _productService.GetProductById(id.Value);
            if (productDto == null)
            {
                return NotFound();
            }

            ProductVM productVM = new ProductVM()
            {
                Product = productDto,
                CategoryList = _categoryService.GetAllCategories().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };

            return View(productVM);
        }

        [HttpPost]
        public IActionResult Edit(ProductVM productVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string RootPath = _webHostEnvironment.WebRootPath;

                if (file != null)
                {
                    string filename = Guid.NewGuid().ToString();
                    var Upload = Path.Combine(RootPath, @"Images\Products");
                    var ext = Path.GetExtension(file.FileName);

                    if (productVM.Product.Img != null)
                    {
                        var oldimg = Path.Combine(RootPath, productVM.Product.Img.TrimStart('\\'));

                        if (System.IO.File.Exists(oldimg))
                        {
                            System.IO.File.Delete(oldimg);
                        }
                    }

                    using (var filestream = new FileStream(Path.Combine(Upload, filename + ext), FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }

                    productVM.Product.Img = @"Images\Products\" + filename + ext;
                }

                _productService.UpdateProduct(productVM.Product);
                TempData["Update"] = "Data has Updated Successfully";
                return RedirectToAction("Index");
            }

            productVM.CategoryList = _categoryService.GetAllCategories().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            return View(productVM);
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }

            var productDto = _productService.GetProductById(id.Value);
            if (productDto == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }

            _productService.DeleteProduct(id.Value);

            if (!string.IsNullOrEmpty(productDto.Img))
            {
                var oldimg = Path.Combine(_webHostEnvironment.WebRootPath, productDto.Img.TrimStart('\\'));

                if (System.IO.File.Exists(oldimg))
                {
                    System.IO.File.Delete(oldimg);
                }
            }

            return Json(new { success = true, message = "file has been Deleted" });
        }
    }
}
