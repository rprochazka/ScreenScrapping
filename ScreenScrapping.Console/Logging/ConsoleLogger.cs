namespace ScreenScrapping.Console.Logging
{
    internal class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            System.Console.WriteLine();
            System.Console.WriteLine(message);
            System.Console.WriteLine();
        }
    }
}