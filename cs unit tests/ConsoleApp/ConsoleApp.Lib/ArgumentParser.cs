using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp.Lib
{
    public class ArgumentParser
    {
        private string Arguments { get; set; }
        private List<string> Flags { get; set; }

        private Dictionary<string, string> argumentDict { get; set; }

        private string[] argStringSplitted;

        public List<string> Overflow { get; set; }

        public string this[int index]
        {
            get { return argStringSplitted[index]; }
        }

        public string this[string index]
        {
            get { return GetArgument(index); }
            set { argumentDict[index] = value; }
        }

        public ArgumentParser(string[] argList)
        {
            Arguments = "";
            Flags = new List<string>();
            argumentDict = new Dictionary<string, string>();
            argStringSplitted = argList;
            Overflow = new List<string>();

            bool skiping = false;

            foreach (var arg in argList)
            {
                _ = TrySingleParse(arg, out bool isFlag, out bool isDoubleFlag, out bool isSkip, out bool hasValue);

                if (skiping)
                {
                    // Everithing after --
                    // Example: -a -b -- -c -d
                    // Result: -c -d
                    Overflow.Add(arg);
                    continue;
                }


                if (isSkip)
                {
                    // Trigger --
                    skiping = true;
                    // TODO: add rest of parameters to a list and place it somewhere!
                    break;
                }

                if (isDoubleFlag)
                {
                    // Trigger --*
                    if (hasValue)
                    {
                        // Trigger --*=
                        // We need to check if this should be part of argumentDict!
                        if (TryParseArgDictEntry(arg, out string argKey, out string argValue))
                        {
                            argumentDict.Add(argKey, argValue);
                        }
                        continue;
                    } 
                    else
                    {
                        Flags.Add(arg.Substring(2));
                    }
                }

                if (isFlag)
                {
                    Arguments += arg.Substring(1);
                }
            }
        }




        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="arg">The argument to test, wil not be modified!</param>
        ///// <param name="flag">Is this a flag></param>
        ///// <param name="nextParam">Check if this is a new statement!</param>
        ///// <returns>If success!</returns>
        //private bool TrySingleParse(in string arg, out bool flag, out bool nextParam)
        //{
        //    flag = false;
        //    nextParam = false;
        //    if (arg.Contains("--"))
        //    {
        //        flag = true;
        //        nextParam = true;
        //        for (int i = 0; i < arg.Length; i++)
        //        {
        //            if (arg[i] == '=')
        //            {
        //                flag = false; // whait, why do we do this?
        //                nextParam = false;
        //            }
        //        }
        //    }
        //    else if (arg.Contains("-"))
        //    {
        //        flag = true;
        //    }
        //    return true;
        //}

        /// <summary>
        /// Pas a console argument and retrieve information in a simple manner!
        /// </summary>
        /// <param name="arg">The console argument!</param>
        /// <param name="isFlag">is this a flag - or --</param>
        /// <param name="isDoubleFlag">is this a --</param>
        /// <param name="isSkip">is this a stand alone --</param>
        /// <returns></returns>
        public static bool TrySingleParse(in string arg, out bool isFlag, out bool isDoubleFlag, out bool isSkip, out bool hasValue)
        {
            isFlag = false;
            isDoubleFlag = false;
            isSkip = false;
            hasValue = false;

            if (string.IsNullOrEmpty(arg)) return false;

            if (arg == "--") { isSkip = true; return true; }
            if (arg.Contains("-")) isFlag = true;
            if (arg.Contains("--")) isDoubleFlag = true;
            if (arg.Contains("=")) { hasValue = true; isFlag = false; }

            return true;
        }

        public static bool TryParseArgDictEntry(in string arg, out string key, out string value)
        {
            value = "";
            key = "";

            if (string.IsNullOrEmpty(arg)) return false;

            int isPosition = arg.IndexOf('=');

            if (isPosition == -1) return false;
            
            if (arg.Length < 6) return false;

            // I got lazy and just asumed the proceeding code made the rest save XD
            // TODO: check if this won't chrash in a big fire ball!
            var result = arg.Split('=');

            key = result[0].Substring(2);
            value = result[1].Trim(new char[] { ' ', '"', '\'' });

            return true;
        }


        public ArgumentParser(string argString) : this(TerSplit_mk2(argString))
        {
        }

        public string getFirst()
        {
            return argStringSplitted[0];
        }

        public bool HasArgument(string arg)
        {
            return Arguments.Contains(arg);
        }

        /// <summary>
        /// Get a key argument. if it does not exist it wil return an empty string instead!
        /// </summary>
        /// <param name="arg">Key of the string aka 'stuff' to get info from --stuff="Hello World!"</param>
        /// <returns></returns>
        public string GetArgument(string arg)
        {
            string result;
            argumentDict.TryGetValue(arg, out result);
            return result ?? "";
        }

        public static string[] TerSplit(string args)
        {
            return args.Split(' ');
        }

        public static string[] TerSplit_mk2(string args)
        {
            StringBuilder sb = new StringBuilder();

            List<string> arguments = new List<string>();

            bool flag = false;
            bool flag2 = false;

            bool combine = false;
            bool ignoreNext = false;
            bool ignoreNext2 = false;

            for (int i = 0; i < args.Length; i++)
            {
                if (ignoreNext) { ignoreNext = false; }
                if (ignoreNext2) { ignoreNext2 = false; ignoreNext = true; }

                switch (args[i])
                {
                    case '-':
                        if (flag) { flag2 = true; }
                        flag = true;
                        goto default;

                    case '=':
                        if (ignoreNext) { goto default; }
                        goto default;

                    case '"':
                        if (ignoreNext) { goto default; }
                        combine = !combine;

                        break;
                    case '\\':
                        ignoreNext2 = true;

                        break;
                    case ' ':
                        if (ignoreNext) { goto default; }
                        if (combine)    { goto default; }

                        arguments.Add(sb.ToString());

                        sb.Length = 0;
                        flag = false;
                        flag2 = false;

                        break;
                    default:
                        sb.Append(args[i]);
                        break;
                }
            }


            if (sb.Length > 0)
            {
                arguments.Add(sb.ToString());
            }



            return arguments.ToArray();
        }

        /// <summary>
        /// Register an argument with value.
        /// </summary>
        /// <param name="argument">The argument key we register</param>
        /// <param name="args">raw argument that we use for parsing.</param>
        private void registerArgument(string argument, string[] args)
        {
            string searchFor = "--" + argument + "=";
            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i];
                if (arg.StartsWith(searchFor) && arg.Length > searchFor.Length)
                {
                    string result = arg.Substring(searchFor.Length);
                    if (result.Length >= 2 && ((result[0] == '"' && result[result.Length - 1] == '"') || (result[0] == '\'' && result[result.Length - 1] == '\'')))
                        result = result.Substring(1, result.Length - 2);
                    argumentDict.Add(argument, result);
                }
            }

            // TODO why did we fail?
        }
    }
}
