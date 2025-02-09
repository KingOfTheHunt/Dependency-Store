using Dapper;
using DependencyStore.Entities;
using DependencyStore.Repositories.Contracts;
using Microsoft.Data.SqlClient;

namespace DependencyStore.Repositories;

public class CustomerRepository(SqlConnection connection) : ICustomerRepository
{
    public async Task<Customer?> GetByIdAsync(int customerId)
    {
        var query = "SELECT [Id], [Name], [Email] FROM [Customers] WHERE [Id] = @customerId";
        var customer = await connection.QueryFirstOrDefaultAsync<Customer>(query, 
            new { CustomerId = customerId });
        
        return customer;
    }
}