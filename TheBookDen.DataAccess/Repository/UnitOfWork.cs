using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheBookDen.DataAccess.Data;
using TheBookDen.DataAccess.Repository.IRepository;

namespace TheBookDen.DataAccess.Repository;

public class UnitOfWork : IUnitOfWork
{
    public ICategoryRepository CategoryRepository { get; private set; }
    public IProductRepository ProductRepository { get; private set; }
    private ApplicationDbContext _db;

    public UnitOfWork(ApplicationDbContext db)
    {
        _db = db;
        CategoryRepository = new  CategoryRepository(_db);
        ProductRepository = new ProductRepository(_db);
    }

    public void Save()
    {
        _db.SaveChanges();
    }
}
