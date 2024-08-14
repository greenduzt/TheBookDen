using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        List<Product> productList = _unitOfWork.ProductRepository.GetAll().ToList();

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
                using (var fileStream = new FileStream(Path.Combine(productPath,fileName),FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                productVm.Product.ImageUrl = @"\images\product\" + fileName;
            }

            _unitOfWork.ProductRepository.Add(productVm.Product );
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
    
    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        Product? product = _unitOfWork.ProductRepository.Get(x => x.Id == id);

        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }
    [HttpPost, ActionName("Delete")]
    public IActionResult DeletePOST(int? id)
    {

        Product? product = _unitOfWork.ProductRepository.Get(x => x.Id == id);
        if (product == null)
        {
            return NotFound();
        }

        _unitOfWork.ProductRepository.Remove(product);
        _unitOfWork.Save();
        TempData["success"] = "Product deleted successfully!";
        return RedirectToAction("Index");
    }
}
