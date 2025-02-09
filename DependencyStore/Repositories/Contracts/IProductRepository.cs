using DependencyStore.Entities;

namespace DependencyStore.Repositories.Contracts;

public interface IProductRepository
{
    public Task<Product?> GetByIdAsync(int id);
}