namespace Sharplox;

public static class SharpLox
{
    private static bool _hadError;
    public static void Main(string[] args)
    {
        Console.WriteLine("Hello, from sharplox!");

        switch (args.Length)
        {
            case > 1:
                Console.WriteLine("Usage: sharplox [script]");
                Environment.Exit(64);
                break;
            case 1:
                RunFile(args[0]);
                break;
            default:
                RunPrompt();
                break;
        }
    }

    private static void RunFile(string path)
    {
        var bytes = File.ReadAllBytes(path);
        Run(System.Text.Encoding.UTF8.GetString(bytes));
        if (_hadError) Environment.Exit(65);
    }

    private static void RunPrompt()
    {
        while (true)
        {
            Console.Write("> ");
            var line = Console.ReadLine();
            if (line == null) break;
            Run(line);
            _hadError = false;
        }
    }

    private static void Run(string source)
    {
        var tokens = new Scanner(source).ScanTokens();
        foreach (var token in tokens)
        {
            Console.WriteLine(token);
        }
    }

    public static void Error(int line, string message)
    {
        Report(line, "", message);
    }

    private static void Report(int line, string where, string message)
    {
        Console.WriteLine($"[line {line}] Error {where}: {message}");
        _hadError = true;
    }
}