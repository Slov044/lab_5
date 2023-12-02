using Lab5;
using Lab5.Entities;

public class ShopRunner
{
    private readonly ShopManager _manager = OpenDb(PathDb);
    private const string PathDb = "db.xml";
    private bool _isActive = true;
    private int _selectedIndex;
    private Order _order = new Order();
    private Func<Product, bool> _predicate = _ => true;
    private ShopClient _client;
    private readonly Dictionary<ConsoleKey, Command> _actionCommands;

    private int _count = 0;

    public ShopRunner()
    {
        _actionCommands = new()
        {
            [ConsoleKey.F] = Command.Create("Фільтрація", Filter),
            [ConsoleKey.U] = Command.Create("Додати продукт", AddProduct),
            [ConsoleKey.J] = Command.Create("Видалити продукт", RemoveProduct),
            [ConsoleKey.W] = Command.Create("Вверх", () => SelectedIndex--),
            [ConsoleKey.S] = Command.Create("Вниз", () => SelectedIndex++),
            [ConsoleKey.Q] = Command.Create("Зберегти та вийти", Exit),
            [ConsoleKey.Enter] = Command.Create("Замовити", Buy)
        };

        UpdateData();
    }

    private void Exit()
    {
        _isActive = false;
        Save(PathDb, _manager);
    }

    public int SelectedIndex
    {
        get => _selectedIndex;
        set
        {
            if (IsValidIndex(value))
                _selectedIndex = value;
        }
    }

    public void Run()
    {
        _client = Login();

        while (_isActive)
        {
            Console.Clear();
            PrintCommand();
            PrintBuyedProduct();
            PrintProduct();
            var command = InputCommand();
            command.Invoke();
        }
    }

    private void PrintBuyedProduct()
    {
        Console.WriteLine("Куплені продукти");
        foreach (var item in _order.Products)
        {
            Console.WriteLine($"{item.Product.Name,-30} {item.Count}");
        }

        Console.WriteLine($"Сума: {_order.TotalPrice,-10} Кількість: {_order.Count}");
    }

    private void PrintCommand()
    {
        foreach (var array in _actionCommands.Chunk(3))
        {
            foreach (var keyValuePair in array)
            {
                Console.Write($"{keyValuePair.Key,-5} - {keyValuePair.Value.Name,-30} ");
            }

            Console.WriteLine();
        }
    }

    private Command InputCommand()
    {
        while (true)
        {
            var key = Console.ReadKey(true).Key;

            if (_actionCommands.TryGetValue(key, out var action))
            {
                return action;
            }
        }
    }

    private void UpdateData()
    {
        _count = _manager.Find(_predicate).Count();
    }

    private void Filter()
    {
        var min = Input.Decimal("Введіть мінімальну ціну");
        var max = Input.Decimal("Введіть максимальну ціну");
        var category = Input.String("Введіть назву категорії");

        Func<Product, bool> function = _ => true;
        if (min.HasValue)
        {
            var minValue = min.Value;
            var copyFunction = function;
            function = p => copyFunction(p) && p.Price >= minValue;
        }

        if (max.HasValue)
        {
            var maxValue = max.Value;
            var copyFunction = function;
            function = p => copyFunction(p) && p.Price <= maxValue;
        }

        if (string.IsNullOrEmpty(category))
        {
            var copyFunction = function;
            function = p => copyFunction(p) && p.Category.Contains(category);
        }

        _predicate = function;

        UpdateData();
    }

    private void PrintProduct()
    {
        int index = 0;
        foreach (var product in _manager.Find(_predicate))
        {
            Console.Write(index == _selectedIndex ? " > " : "   ");
            Console.WriteLine($"{product.Id,-3} - {product.Name,-20} {product.Category,-20} ${product.Price,-5}");
            index++;
        }
    }

    private void Buy()
    {
        _client.AddOrder(_order);
        _order = new Order();
    }

    private void AddProduct()
    {
        if (!IsValidIndex())
            return;

        var product = _manager.Find(_predicate).ElementAt(_selectedIndex);
        var orderItem = _order.Products.FirstOrDefault(e => e.Product.Id == product.Id);
        if (orderItem is null)
        {
            orderItem = new OrderItem()
            {
                Product = product
            };
            _order.Products.Add(orderItem);
        }

        orderItem.Count++;
    }

    private bool IsValidIndex()
    {
        return IsValidIndex(SelectedIndex);
    }
    
    private bool IsValidIndex(int index)
    {
        return index >= 0 && index < _count;
    }

    private void RemoveProduct()
    {
        if (!IsValidIndex())
            return;

        var product = _manager.DataContext.Products[_selectedIndex];
        var orderItem = _order.Products.FirstOrDefault(e => e.Product.Id == product.Id);
        if (orderItem is null)
        {
            return;
        }

        orderItem.Count--;

        if (orderItem.Count <= 0)
        {
            _order.Products.Remove(orderItem);
        }
    }

    public ShopClient Login()
    {
        ShopClient? client = null;

        while (client is null)
        {
            var login = Input.String("Введіть логін");
            var password = Input.String("Введіть пароль");
            client = _manager.Login(login, password);
        }

        return client;
    }

    public static ShopManager OpenDb(string path)
    {
        if (!File.Exists(path))
        {
            var manager = new ShopManager(TemplateDataContext.GetTemplate());
            Save(path, manager);
        }

        using var fileStream = File.OpenRead(path);
        return ShopManager.CreateInstance(fileStream);
    }

    public static ShopManager Save(string path, ShopManager manager)
    {
        using var stream = File.Open(path, FileMode.Create, FileAccess.Write);
        ShopManager.Save(stream, manager);
        return manager;
    }
}