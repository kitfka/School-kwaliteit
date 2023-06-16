using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp.Lib;

namespace ConsoleApp;

[StaticConsoleCommand(CommandName = "help")]
[CommandDescription("The current Command that is used to display this list!")]
public static class Commands
{
    [SubCommand(IsDefaultCommand = true)]
    public static CommandResult DefaultMethod(string[] args)
    {

        return new CommandResult()
        {
            ErrorMessage = "",
            //Result = "Hello World, You called the DefaultMethod correctly!",
            Result = $"Commands: \n\n{ConsoleCommandHelper.GetCommandList()}",
            Status = CommandStatus.Succes,
        };
    }
}

[StaticConsoleCommand(CommandName = "version")]
[CommandDescription("version!!!")]
public static class CommandVersion
{
    [SubCommand(IsDefaultCommand = true)]
    public static CommandResult DefaultMethod(string[] args)
    {
        return new CommandResult()
        {
            ErrorMessage = "",
            Result = "Version!",
            Status = CommandStatus.Succes,
        };
    }
}
