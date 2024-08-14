﻿using TheBookDen.DataAccess.Data;
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
        _db.Products.Update(product);
    }
}
