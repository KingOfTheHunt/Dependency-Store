using Dapper;
using DependencyStore.Entities;
using DependencyStore.Repositories.Contracts;
using Microsoft.Data.SqlClient;

namespace DependencyStore.Repositories;

public class PromoCodeRepository(SqlConnection connection) : IPromoCodeRepository
{
    public async Task<PromoCode?> GetByCodeAsync(string code)
    {
        var query = "SELECT [Code], [Value], [ExpireDate] FROM [PromoCodes] WHERE [Code] = @code";
        var promoCode = await connection.QueryFirstOrDefaultAsync<PromoCode>(query, new { code });

        return promoCode;
    }
}