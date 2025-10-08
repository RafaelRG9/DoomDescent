public static class UIManager
{
    public static void SlowPrint(string text, ConsoleColor? color = null)
    {
        if (color.HasValue)
        {
            Console.ForegroundColor = color.Value;
        }

        foreach (char c in text)
        {
            Console.Write(c);
            Thread.Sleep(25);
        }

        Console.ResetColor();
        Console.WriteLine();
    }
}