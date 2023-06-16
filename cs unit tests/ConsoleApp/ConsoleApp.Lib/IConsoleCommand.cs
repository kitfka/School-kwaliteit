namespace ConsoleApp.Lib
{
    public interface IConsoleCommand
    {
        CommandResult Invoke();
        string ToString();
    }
}