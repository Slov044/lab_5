using Lab5.Entities;

[Serializable]
public class OrderItem
{
    public Product Product { get; set; }
    public int Count { get; set; }
}