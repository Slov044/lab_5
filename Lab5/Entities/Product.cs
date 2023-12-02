namespace Lab5.Entities;
[Serializable]
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public string Category { get; set; }
    
    public Product(string name, decimal price, string category, string? description = null)
    {
        Name = name;
        Price = price;
        Category = category;
        Description = description;
    }

    public Product()
    {
        
    }
}