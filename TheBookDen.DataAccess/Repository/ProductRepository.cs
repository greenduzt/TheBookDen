using TheBookDen.DataAccess.Data;
using TheBookDen.DataAccess.Repository.IRepository;
using TheBookDen.Models.Models;

namespace TheBookDen.DataAccess.Repository;

public class ProductRepository : Repository<Product>, IProductRepository
{
    private ApplicationDbContext _db;

    public ProductRepository(ApplicationDbContext db) :base(db)
    {
        _db = db;
    }
    public void Update(Product product)
    {
        var objDb = _db.Products.FirstOrDefault(x=>x.Id == product.Id);
        if (objDb != null)
        {
            objDb.Title = product.Title;
            objDb.ISBN = product.ISBN;
            objDb.Price = product.Price;
            objDb.ListPrice = product.ListPrice;
            objDb.Price100 = product.Price100;
            objDb.Price50 = product.Price50;
            objDb.Description = product.Description;
            objDb.CategoryId = product.CategoryId;
            objDb.Author = product.Author;
            if(objDb.ImageUrl != null)
            {
                objDb.ImageUrl = product.ImageUrl;
            }
        }
    }
}
