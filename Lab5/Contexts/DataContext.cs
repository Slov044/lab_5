using Lab5.Entities;

namespace Lab5.Contexts;

public class DataContext
{
    public List<User> Users { get; set; }
    public List<Product> Products { get; set; }
}