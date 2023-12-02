public static class Input
{
    public static string String(string title)
    {
        Console.Write("{0}: ", title);
        var result = Console.ReadLine()!.Trim();
        return result;
    }

    public static double? Double(string title)
    {
        while (true)
        {
            var text = String(title);
            if (string.IsNullOrEmpty(text))
                return null;

            if (double.TryParse(text, out var result))
            {
                return result;
            }
        }
    }

    public static decimal? Decimal(string title)
    {
        while (true)
        {
            var text = String(title);
            if (string.IsNullOrEmpty(text))
                return null;

            if (decimal.TryParse(text, out var result))
            {
                return result;
            }
        }
    }
}