namespace DependencyStore.Entities;

public class PromoCode
{
    public string Code { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public DateTime ExpireDate { get; set; }
}