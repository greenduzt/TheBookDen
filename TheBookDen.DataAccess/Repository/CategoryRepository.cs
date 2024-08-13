using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TheBookDen.DataAccess.Data;
using TheBookDen.DataAccess.Repository.IRepository;
using TheBookDen.Models.Models;

namespace TheBookDen.DataAccess.Repository;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    private ApplicationDbContext _db;
    public CategoryRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public void Save()
    {
        _db.SaveChanges();
    }

    public void Update(Category category)
    {
        _db.Categories.Update(category);
    }
}
