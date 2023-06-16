using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp.Lib
{
    public class ConsoleCommand : IConsoleCommand
    {
        public string name;
        public string[] requiredArgs;
        public string[] requiredArgsDescriptions;
        public string[] optionalArgs;
        public string[] optionalArgsDescriptions;
        public bool hidden;
        public Action function;

        public string EXECUTABLE_NAME = "";

        public virtual CommandResult Invoke()
        {
            function.Invoke();
            return CommandResult.Succes;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(500);
            sb.Append("===== ").Append(EXECUTABLE_NAME).Append(" ").Append(name).Append(" =====").AppendLine();

            ToStringArguments(sb, "Required arguments: ", requiredArgs, requiredArgsDescriptions);
            ToStringArguments(sb, "Optional arguments: ", optionalArgs, optionalArgsDescriptions);

            return sb.ToString();
        }

        private void ToStringArguments(StringBuilder sb, string label, string[] args, string[] argsDescriptions)
        {
            if (args.Length > 0)
            {
                sb.AppendLine(label);

                for (int i = 0; i < args.Length; i++)
                {
                    sb.Append("-").Append(args[i]);
                    if (!string.IsNullOrEmpty(argsDescriptions[i]))
                        sb.Append("={").Append(argsDescriptions[i]).Append("} ");
                    else
                        sb.Append(" ");

                    sb.AppendLine();
                }

                sb.AppendLine();
            }
        }
    }
}
