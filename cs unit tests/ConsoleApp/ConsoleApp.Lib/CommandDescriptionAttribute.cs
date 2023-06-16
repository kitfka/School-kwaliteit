using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp.Lib
{
    [System.AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public class CommandDescriptionAttribute : Attribute
    {
        // See the attribute guidelines at 
        //  http://go.microsoft.com/fwlink/?LinkId=85236
        readonly string description = "";

        // This is a positional argument
        public CommandDescriptionAttribute(string description)
        {
            this.description = description;

        }

        public string Description
        {
            get { return description; }
        }
    }
}
