using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Lab5.Contexts;
using Lab5.Entities;

namespace Lab5;

public class ShopManager: ISearchable
{
    
    public IEnumerable<Product> GetProduct([Range(0, Int32.MaxValue)] int page, [Range(1, Int32.MaxValue)] int count)
    {
        return _dataContext.Products.Skip(page * count).Take(count);
    }

    public IEnumerable<Product> Find(Func<Product, bool> predicate)
    {
        return _dataContext.Products.Where(predicate);
    }

    public ShopClient? Login(string name, string password)
    {
        var user = _dataContext.Users.FirstOrDefault(e => e.Login == name && e.Password == password);
        return user is null 
            ? null 
            : new ShopClient(user);
    }

    private readonly DataContext _dataContext;

    public DataContext DataContext => _dataContext;

    public ShopManager(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public static ShopManager CreateInstance(Stream stream)
    {
        var serializer = GetSerializer();
        return new ShopManager((DataContext) serializer.Deserialize(stream)!);
    }

    public static void Save(Stream stream, ShopManager shopManager)
    {
        Save(stream, shopManager._dataContext);
    }

    public static void Save(Stream stream, DataContext dataContext)
    {
        var serializer = GetSerializer();
        serializer.Serialize(stream, dataContext);
    }

    private static XmlSerializer GetSerializer()
    {
        return new XmlSerializer(typeof(DataContext), new[]
        {
            typeof(User),
            typeof(Product),
            typeof(Order),
            typeof(OrderItem),
            typeof(List<User>),
            typeof(List<Product>),
            typeof(List<Order>),
            typeof(List<OrderItem>),
        });
    }
    
}