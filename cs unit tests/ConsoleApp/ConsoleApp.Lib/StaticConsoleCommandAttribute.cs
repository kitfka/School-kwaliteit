using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp.Lib
{
    [System.AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class StaticConsoleCommandAttribute : Attribute
    {
        // See the attribute guidelines at 
        //  http://go.microsoft.com/fwlink/?LinkId=85236


        // This is a positional argument
        public StaticConsoleCommandAttribute()
        {
        }

        //public string PositionalString
        //{
        //    get { return positionalString; }
        //}

        // This is a named argument
        public string CommandName { get; set; } = "";

    }
}
