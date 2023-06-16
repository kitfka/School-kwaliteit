using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ConsoleApp.Lib
{
    public class ConsoleCommandHelper: ConsoleCommand
    {
        public static string GetCommandList()
        {
            StringBuilder sb = new StringBuilder();
            var type =
                from t in Assembly.GetCallingAssembly().GetTypes()
                where t.IsDefined(typeof(StaticConsoleCommandAttribute), false)
                //where t.GetCustomAttribute<StaticConsoleCommandAttribute>()
                //    == GenericConfigType.Type1
                select t;

            foreach (var item in type)
            {
                sb.Append(item.GetCustomAttribute<StaticConsoleCommandAttribute>().CommandName);
                sb.Append(", ");

                if (Attribute.IsDefined(item, typeof(CommandDescriptionAttribute)))
                {
                    sb.AppendLine(item.GetCustomAttribute<CommandDescriptionAttribute>().Description);
                } 
                else
                {
                    sb.AppendLine("N/A");
                }
            }

            return sb.ToString();
        }

        public static CommandResult ExecuteCommand(string command, string subCommand = "", string[] args = null)
        {
            try
            {
                if (string.IsNullOrEmpty(subCommand))
                {
                    var t = GetTypeFromCommandName(command, Assembly.GetCallingAssembly());
                    if (t == null) { return CommandResult.CommandNotFound; }
                    return (CommandResult)GetDefaultMethodOfType(t).Invoke(null, new object[] { args });
                }
                else
                {
                    var t = GetTypeFromCommandName(command, Assembly.GetCallingAssembly());
                    if (t == null) { return CommandResult.CommandNotFound; }
                    MethodInfo m = GetMethodOfType(t, subCommand);
                    if (m == null) { return CommandResult.CommandNotFound; }
                    return (CommandResult)m.Invoke(null, new object[] { args });
                }
            }
            catch (Exception ex)
            {
                return new CommandResult(CommandStatus.Failed)
                {
                    ErrorMessage = ex.Message,
                };
            }
        }

        public static string GetCommandDescription(string command, string subCommand = "")
        {
            try
            {
                if (string.IsNullOrEmpty(subCommand))
                {
                    var t = GetTypeFromCommandName(command, Assembly.GetCallingAssembly());
                    MethodInfo m = GetDefaultMethodOfType(t);
                    return m.GetCustomAttribute<CommandDescriptionAttribute>().Description;
                }
                else
                {
                    var t = GetTypeFromCommandName(command, Assembly.GetCallingAssembly());
                    MethodInfo m = GetMethodOfType(t, subCommand);
                    return m.GetCustomAttribute<CommandDescriptionAttribute>().Description;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        
        
        public static Type GetTypeFromCommandName(string commandName) => GetTypeFromCommandName(commandName, Assembly.GetCallingAssembly());
        public static Type GetTypeFromCommandName(string commandName, Assembly assembly)
        {
            //var t = from type in GetTypesWithAttribute(typeof(StaticConsoleCommandAttribute), assembly)
            //       where type.GetCustomAttribute<StaticConsoleCommandAttribute>().CommandName == commandName
            //       select type;
            foreach (var item in GetTypesWithAttribute(typeof(StaticConsoleCommandAttribute), assembly))
            {
                if (item.GetCustomAttribute<StaticConsoleCommandAttribute>().CommandName == commandName)
                {
                    return item;
                }
            }

            return null;
            //return t.First();
        }

        private static IEnumerable<Type> GetTypesWithAttribute(Type attributeType, Assembly assembly)
        {
            foreach (var type in assembly.GetTypes())
            {
                if (type.IsDefined(attributeType, false))
                {
                    yield return type;
                }
            }

            yield break;
        }

        public static IEnumerable<MethodInfo> GetMethodsOfType(Type type)
        {
            return from member in type.GetMethods()
                   where member.GetCustomAttribute<SubCommandAttribute>(false) != null
                   select member;
        }

        public static MethodInfo GetDefaultMethodOfType(Type type)
        {
            var result = from member in GetMethodsOfType(type)
                         where member.GetCustomAttribute<SubCommandAttribute>(false).IsDefaultCommand
                         select member;

            return result.First();
        }

        public static MethodInfo GetMethodOfType(Type type, string Command)
        {
            var result = from member in GetMethodsOfType(type)
                         where member.GetCustomAttribute<SubCommandAttribute>(false).CommandName == Command
                         select member;

            return result.First();
        }

        public override CommandResult Invoke()
        {
            function.Invoke();
            return CommandResult.Succes;
        }
    }
}
