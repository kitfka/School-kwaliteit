using ConsoleApp.Lib;

namespace ConsoleApp.Test;
public class ConsoleCommandHelperTest
{

    [Fact]
    public void GetTypeFromCommandName_WithTestName_ReturnCorrectType()
    {
        Type actualType;
        Type expectedType = typeof(TestCommandClass);

        actualType = ConsoleCommandHelper.GetTypeFromCommandName("Test");

        Assert.Equal(expectedType, actualType);
    }

    [Fact]
    public void GetDefaultMethodOfType_Calling_ShouldReturnDefault()
    {
        var member = ConsoleCommandHelper.GetDefaultMethodOfType(typeof(TestCommandClass));

        CommandResult actualResult = (CommandResult)member.Invoke(null, new object[] { null });

        Assert.Equal(CommandStatus.Succes, actualResult.Status);
    }

    [Fact]
    public void ExecuteCommand_Subcommand_ShouldReturn_asd()
    {
        CommandResult result = ConsoleCommandHelper.ExecuteCommand("Test", "Test2", Array.Empty<string>());

        Assert.Equal("asd", result.Result);
    }

    [Fact]
    public void GetCommandDescription_Test()
    {
        string Expected = "This is a demo description!";
        string actual;

        actual = ConsoleCommandHelper.GetCommandDescription("Test");

        Assert.Equal(Expected, actual);
    }

    [Fact]
    public void GetCommandDescription_FromCommandTestingThing_ShouldWork()
    {
        string Expected = "Other Description";
        string actual;

        actual = ConsoleCommandHelper.GetCommandDescription("Test", "Test2");

        Assert.Equal(Expected, actual);
    }
}



[StaticConsoleCommand(CommandName = "Test")]
public static class TestCommandClass
{
    [SubCommand(IsDefaultCommand = true)]
    [CommandDescription("This is a demo description!")]
    public static CommandResult DefaultThing(string[] args)
    {
        return CommandResult.Succes;
    }

    [SubCommand(CommandName = "Test2")]
    [CommandDescription("Other Description")]
    public static CommandResult CommandTestingThing(string[] args)
    {
        return new CommandResult()
        {
            Result = "asd",
        };
    }
}