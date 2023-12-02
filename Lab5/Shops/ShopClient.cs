namespace Lab5;

public class ShopClient
{
    private readonly User _user;

    public ShopClient(User user)
    {
        _user = user;
    }

    public void AddOrder(Order order)
    {
        _user.Orders.Add(order);
    }
}