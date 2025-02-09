using DependencyStore.Entities;

namespace DependencyStore.Repositories.Contracts;

public interface IPromoCodeRepository
{
    Task<PromoCode?> GetByCodeAsync(string code); 
}