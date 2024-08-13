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
    private ApplicationDbContext _db;

    public UnitOfWork(ApplicationDbContext db)
    {
        _db = db;
        CategoryRepository = new  CategoryRepository(_db);
    }

    public void Save()
    {
        _db.SaveChanges();
    }
}
