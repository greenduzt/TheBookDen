using TheBookDen.Models.Models;

namespace TheBookDen.DataAccess.Repository.IRepository;

public interface IProductRepository : IRepository<Product>
{
    void Update(Product product);
}
