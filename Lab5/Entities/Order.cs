using Lab5.Entities;

[Serializable]
public class Order
{
    public List<OrderItem> Products { get; set; } = new();
    
    public int Count => Products
        .Select(e => e.Count)
        .DefaultIfEmpty()
        .Sum();

    public decimal TotalPrice => Products
        .Select(e => e.Count * e.Product.Price)
        .DefaultIfEmpty()
        .Sum();

    public OrderStatus Status { get; set; } = OrderStatus.Processed;
}