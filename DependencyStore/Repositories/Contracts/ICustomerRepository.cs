using DependencyStore.Entities;

namespace DependencyStore.Repositories.Contracts;

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(int id);
}