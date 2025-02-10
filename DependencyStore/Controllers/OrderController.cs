using DependencyStore.Entities;
using DependencyStore.Repositories;
using DependencyStore.Repositories.Contracts;
using DependencyStore.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace DependencyStore.Controllers;

public class OrderController : ControllerBase
{
    private readonly SqlConnection _connection;
    private readonly ICustomerRepository _customerRepository;
    private readonly IProductRepository _productRepository;
    private readonly IPromoCodeRepository _promoCodeRepository;
    private readonly IDeliveryFeeService _deliveryFeeService;

    public OrderController(ICustomerRepository customerRepository, IProductRepository productRepository, 
        IPromoCodeRepository promoCodeRepository, IDeliveryFeeService deliveryFeeService)
    {
        _connection = new SqlConnection("ConnectionString");
        _customerRepository = customerRepository;
        _productRepository = productRepository;
        _promoCodeRepository = promoCodeRepository;
        _deliveryFeeService = deliveryFeeService;
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
        var deliveryFee = await _deliveryFeeService.CalculateAsync(zipCode);
        var coupon = await _promoCodeRepository.GetByCodeAsync(promoCode);
        var discount = coupon?.Value ?? 0m;
        List<Product> productsList = [];

        foreach (var productId in products)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product is not null)
                productsList.Add(product);
        }
        
        var order = new Order(deliveryFee, discount, productsList);

        return Ok(new
        {
            message = $"Pedido {order.Code} criado com sucesso!",
        });
    }
}