using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp.Lib
{
    public class CommandResult
    {
        public string ErrorMessage { get; set; } // might be obsolete
        public string Result { get; set; }
        public CommandStatus Status { get; set; } = CommandStatus.Succes;
        public static CommandResult Succes
        {
            get => new CommandResult(CommandStatus.Succes);
        }

        public static CommandResult CommandNotFound
        {
            get => new CommandResult() { ErrorMessage = "Command Not Found", Status = CommandStatus.Failed };
        }

        public CommandResult() :this(CommandStatus.Default) { }

        public CommandResult(CommandStatus status) 
        {
            Status = status;
            ErrorMessage = "None";
        }


    }

    public enum CommandStatus
    {
        Default,
        Succes,
        Failed,
    }
}
