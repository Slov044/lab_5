// See https://aka.ms/new-console-template for more information

using System.Text;

internal class Program
{
    private static ShopRunner _runner = new ShopRunner();
    public static void Main()
    {
        Console.InputEncoding = Console.OutputEncoding = Encoding.UTF8;
        _runner.Run();
    }
}