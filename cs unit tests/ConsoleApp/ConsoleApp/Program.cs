using ConsoleApp.Lib;

internal class Program
{
    public static ArgumentParser ArgumentParser { get; set; }
    private static void Main(string[] args)
    {
        // for testing use next line to force a command!
        //args = new string[] { "help" };

        ArgumentParser = new(args);

        string command = ArgumentParser.getFirst();
        if (ArgumentParser.HasArgument("version")) { command = "version"; }
        if (ArgumentParser.HasArgument("help")) { command = "help"; }

        CommandResult result = ConsoleCommandHelper.ExecuteCommand(command, "", Array.Empty<string>());

        switch (result.Status)
        {
            case CommandStatus.Default:
            case CommandStatus.Succes:
                Console.WriteLine(result.Result);
                break;
            case CommandStatus.Failed:
                Console.WriteLine($"Failed: {result.ErrorMessage}");
                Console.WriteLine(result.Result);
                break;
            default:
                Console.WriteLine("Hit the default, how???");
                break;
        }
    }
}