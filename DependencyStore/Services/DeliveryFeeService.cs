using DependencyStore.Services.Contracts;

namespace DependencyStore.Services;

public class DeliveryFeeService(HttpClient httpClient) : IDeliveryFeeService
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<decimal> CalculateAsync(string zipCode)
    {
        _httpClient.BaseAddress = new Uri("http://consultafrete.io");
        var request = await httpClient.PostAsync($"{httpClient.BaseAddress}/v1/deliveryFee", 
            new StringContent(zipCode));
        
        var deliveryFee = await request.Content.ReadFromJsonAsync<decimal>();
        
        if (deliveryFee < 5)
            deliveryFee = 5;
        
        return deliveryFee;
    }
}