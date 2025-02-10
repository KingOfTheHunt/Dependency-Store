namespace DependencyStore.Entities;

public class Order
{
    public string Code { get; set; }
    public DateTime Date { get; set; }
    public decimal DeliveryFee { get; set; }
    public decimal Discount { get; set; }
    public List<Product> Products { get; set; } = [];
    public decimal SubTotal => Products.Sum(x => x.Price);
    public decimal Total => SubTotal - Discount + DeliveryFee;

    public Order(decimal deliveryFee, decimal discount, List<Product> products)
    {
        Code = Guid.NewGuid().ToString().ToUpper()[..8];
        Date = DateTime.Now;
        DeliveryFee = deliveryFee;
        Discount = discount;
    }
}