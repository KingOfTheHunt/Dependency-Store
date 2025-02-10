namespace DependencyStore.Services.Contracts;

public interface IDeliveryFeeService
{
    Task<decimal> CalculateAsync(string zipCode);
}