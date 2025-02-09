using Dapper;
using DependencyStore.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace DependencyStore.Controllers;

public class OrderController : ControllerBase
{
    [HttpPost]
    [Route("v1/order")]
    public async Task<IActionResult> Place(string customerId, string zipCode, string promoCode, int[] products)
    {
        // 1 - Recupera o cliente
        Customer? customer = null;

        await using var connection = new SqlConnection("ConnectionString");
        const string query = "SELECT [Id], [Name], [Email] FROM Customer WHERE [Id]=@id";
        customer = await connection.QuerySingleOrDefaultAsync<Customer>(query, new {id = customerId});
        
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
        const string getProductQuery = "SELECT [Id], [Name], [Price] FROM Product WHERE [Id]=@id";
        for (int i = 0; i < products.Length; i++)
        {
            Product product = await connection.QueryFirstAsync(getProductQuery, new { id = products[i] });
            subtotal += product.Price;
        }
        
        // 4 - Aplica o desconto
        var discount = 0m;
        const string getPromoQuery = "SELECT [Code], [Value], [ExpireDate] FROM Promo_Codes WHERE [Code]=@code"; 
        var promo = await connection.QueryFirstAsync<PromoCode>(getPromoQuery, new { code = promoCode });

        if (promo.ExpireDate > DateTime.Now)
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