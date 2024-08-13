using Microsoft.AspNetCore.Mvc;
using TheBookDen.DataAccess.Data;
using TheBookDen.Models.Models;

namespace TheBookDenWeb.Controllers;

public class CategoryController : Controller
{
    private readonly ApplicationDbContext _db;
    public CategoryController(ApplicationDbContext db)
    {
        _db = db;
    }

    public IActionResult Index()
    {
        List<Category> categoryList = _db.Categories.ToList();   

        return View(categoryList);
    }

    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Create(Category category)
    {
        if (category.DisplayOrder.ToString() == category.Name)
        {
            ModelState.AddModelError("name", "The DisplayOrder cannot exacrly match the name");
        }

        if (ModelState.IsValid)
        {
            _db.Categories.Add(category);
            _db.SaveChanges();
            TempData["success"] = "Category created successfully!";
            return RedirectToAction("Index");
        }

        return View();
    }
    public IActionResult Edit(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        Category? category = _db.Categories.Find(id);
        //Category? category2 = _db.Categories.Where(x=>x.Id == id).FirstOrDefault();
        //Category? category3 = _db.Categories.FirstOrDefault(x=>x.Id == id);


        if (category == null)
        {
            return NotFound();
        }

        return View(category);
    }
    [HttpPost]
    public IActionResult Edit(Category category)
    {
       

        if (ModelState.IsValid)
        {
            _db.Categories.Update(category);
            _db.SaveChanges();
            TempData["success"] = "Category updated successfully!";
            return RedirectToAction("Index");
        }

        return View();
    }

    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        Category? category = _db.Categories.Find(id);        

        if (category == null)
        {
            return NotFound();
        }

        return View(category);
    }
    [HttpPost, ActionName("Delete")]
    public IActionResult DeletePOST(int? id)
    {

        Category? category = _db.Categories.Find(id);
        if (category == null)
        {
            return NotFound();
        }

        _db.Categories.Remove(category);
        _db.SaveChanges();
        TempData["success"] = "Category deleted successfully!";
        return RedirectToAction("Index");
    }
}
