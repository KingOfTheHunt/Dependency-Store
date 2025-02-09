using DependencyStore.Entities;
using DependencyStore.Repositories;
using DependencyStore.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace DependencyStore.Controllers;

public class OrderController : ControllerBase
{
    private readonly SqlConnection _connection;
    private readonly ICustomerRepository _customerRepository;
    private readonly IProductRepository _productRepository;
    private readonly IPromoCodeRepository _promoCodeRepository;

    public OrderController(ICustomerRepository customerRepository, IProductRepository productRepository, 
        IPromoCodeRepository promoCodeRepository)
    {
        _connection = new SqlConnection("ConnectionString");
        _customerRepository = customerRepository;
        _productRepository = productRepository;
        _promoCodeRepository = promoCodeRepository;
    }
    
    [HttpPost]
    [Route("v1/order")]
    public async Task<IActionResult> Place(int customerId, string zipCode, string promoCode, int[] products)
    {
        // 1 - Recupera o cliente
        var customer = await _customerRepository.GetByIdAsync(customerId);

        if (customer is null)
            return NotFound("Customer not found");
        
        // 2 - Calcula o frete
        var deliveryFee = 0m;
        HttpClient httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("http://consultafrete.io");
        var request = await httpClient.PostAsync($"{httpClient.BaseAddress}/v1/deliveryFee", new StringContent(zipCode));
        deliveryFee = await request.Content.ReadFromJsonAsync<decimal>();
        
        if (deliveryFee < 5)
            deliveryFee = 5;
        
        // 3 - Calcula o total dos produtos
        var subtotal = 0m;
        for (int i = 0; i < products.Length; i++)
        {
            var product = await _productRepository.GetByIdAsync(products[i]);
            subtotal += product.Price;
        }
        
        // 4 - Aplica o desconto
        var discount = 0m;
        var promo = await _promoCodeRepository.GetByCodeAsync(promoCode);

        if (promo!.ExpireDate > DateTime.Now)
            discount = promo.Value;
        
        // 5 - Gera o pedido
        var order = new Order();
        order.Code = Guid.NewGuid().ToString().ToUpper()[..8];
        order.Date = DateTime.Now;
        order.DeliveryFee = deliveryFee;
        order.Discount = discount;
        order.Products = products;
        order.SubTotal = subtotal;
        
        // 7 - Calcula o total
        order.Total = subtotal - discount;

        return Ok(new
        {
            message = $"Pedido {order.Code} gerado com sucesso!"
        });
    }
}