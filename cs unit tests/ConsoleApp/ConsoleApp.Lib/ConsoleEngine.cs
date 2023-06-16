using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp.Lib
{
    /// <summary>
    /// This is dump.
    /// 
    /// Oke this could be used as a custom engine thingy
    /// </summary>
    public class ConsoleEngine
    {
        private Dictionary<string, ConsoleCommand> _commands;

        public Dictionary<string, ConsoleCommand> Commands
        {
            get { return _commands; }
            set { _commands = value; }
        }

        public CommandResult Execute(string commandName)
        {
            return _commands[commandName].Invoke();
        }

        public void Execute(ArgumentParser argumentParser)
        {
            //argumentParser.getFirst
            //_commands[]
        }
    }
}
