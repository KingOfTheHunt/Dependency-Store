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