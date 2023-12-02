using Lab5.Contexts;
using Lab5.Entities;

namespace Lab5;

public static class TemplateDataContext
{
    public static DataContext GetTemplate()
    {
        var dataContext = new DataContext();
        dataContext.Products = new()
        {
            new Product("Ноутбук", 1200, "Електроніка", "Високий клас") {Id = 1},
            new Product("Книга", 20, "Книги", "Загальна") {Id = 2},
            new Product("Смартфон", 800, "Електроніка", "Середній клас") {Id = 3},
            new Product("Кофе", 5, "Продукти") {Id = 4},
            new Product("Футболка", 25, "Одяг", "Літня колекція") {Id = 5},
            new Product("Іграшка", 15, "Іграшки", "Для дітей") {Id = 6},
            new Product("Груша", 2, "Продукти") {Id = 7},
            new Product("Косметика", 50, "Краса", "Натуральна") {Id = 8},
            new Product("Футбольний м'яч", 30, "Спорт", "Стандартний") {Id = 9},
            new Product("Інструменти", 80, "Інструменти", "Для будівництва") {Id = 10}
        };

        dataContext.Users = new()
        {
            new User()
            {
                Id = 1,
                Login = "admin",
                Password = "admin",
                Orders = new List<Order>()
                {
                    new Order()
                    {
                        Products = new List<OrderItem>()
                        {
                            new OrderItem()
                            {
                                Count = 4,
                                Product = dataContext.Products.First()
                            }
                        },
                        Status = OrderStatus.Submitted
                    }
                }
            }
        };
        return dataContext;
    }
}