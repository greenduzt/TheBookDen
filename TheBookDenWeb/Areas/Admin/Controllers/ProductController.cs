using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using TheBookDen.DataAccess.Repository.IRepository;
using TheBookDen.Models.Models;
using TheBookDen.Models.ViewModels;

namespace TheBookDenWeb.Areas.Admin.Controllers;

[Area("Admin")]
public class ProductController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
    {
        _unitOfWork = unitOfWork;
        _webHostEnvironment = webHostEnvironment;
    }

    public IActionResult Index()
    {
        List<Product> productList = _unitOfWork.ProductRepository.GetAll(includeProperties:"Category").ToList();

        return View(productList);
    }

    public IActionResult Upsert(int? id)
    {
        ProductViewModel productVm = new()
        {
            CategoryList = _unitOfWork.CategoryRepository.GetAll().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()

            }),
            Product = new Product()
        };
        if (id == null || id == 0)
        {
            return View(productVm);
        }
        else
        {
            //Update
            productVm.Product = _unitOfWork.ProductRepository.Get(x=>x.Id == id);
            return View(productVm);
        }

        //ViewBag.CategoryList = CategoryList;
        //ViewData["CategoryList"] = CategoryList;

        return View(productVm);
    }
    [HttpPost]
    public IActionResult Upsert(ProductViewModel productVm, IFormFile? file)
    {
        if (ModelState.IsValid)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            if(file != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string productPath = Path.Combine(wwwRootPath, @"images\product");

                if (!string.IsNullOrEmpty(productVm.Product.ImageUrl))
                {
                    //delete the old image
                    var oldImagePath =
                        Path.Combine(wwwRootPath, productVm.Product.ImageUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                using (var fileStream = new FileStream(Path.Combine(productPath,fileName),FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                productVm.Product.ImageUrl = @"\images\product\" + fileName;
            }

            if (productVm.Product.Id == 0)
            {
                _unitOfWork.ProductRepository.Add(productVm.Product);
            }
            else
            {
                _unitOfWork.ProductRepository.Update(productVm.Product);
            }

            _unitOfWork.Save();
            TempData["success"] = "Product created successfully!";
            return RedirectToAction("Index");
        }
        else
        {
            productVm.CategoryList = _unitOfWork.CategoryRepository.GetAll().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()

            });           
            return View(productVm);
        }

       
    }
    
   

    #region API calls
    [HttpGet]
    public IActionResult GetAll()
    {
        List<Product> productList = _unitOfWork.ProductRepository.GetAll(includeProperties: "Category").ToList();
        return Json(new { data = productList });
    }

    [HttpDelete]
    public IActionResult Delete(int? id)
    {
        var productToBeDeleted = _unitOfWork.ProductRepository.Get(x=>x.Id==id);
        if (productToBeDeleted == null)
        {
            return Json(new {success = false, Message = "Error while deleting"});
        }

        var oldImagePath =
                        Path.Combine(_webHostEnvironment.WebRootPath, productToBeDeleted.ImageUrl.TrimStart('\\'));

        if (System.IO.File.Exists(oldImagePath))
        {
            System.IO.File.Delete(oldImagePath);
        }

        _unitOfWork.ProductRepository.Remove(productToBeDeleted);
        _unitOfWork.Save();
        
        return Json(new { success=true, message = "Delete Successful" });
    }
    #endregion
}
