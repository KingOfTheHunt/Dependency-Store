using Dapper;
using DependencyStore.Entities;
using DependencyStore.Repositories.Contracts;
using Microsoft.Data.SqlClient;

namespace DependencyStore.Repositories;

public class ProductRepository(SqlConnection connection) : IProductRepository
{
    public async Task<Product?> GetByIdAsync(int productId)
    {
        var query = "SELECT [Id], [Name], [Price] FROM [Products] WHERE [Id]=@id";
        var product = await connection.QueryFirstAsync<Product>(query, new { id = productId });
        
        return product;
    }
}