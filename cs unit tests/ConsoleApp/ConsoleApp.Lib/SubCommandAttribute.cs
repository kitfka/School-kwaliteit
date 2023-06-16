using System;


namespace ConsoleApp.Lib
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public class SubCommandAttribute : Attribute
    {
        // See the attribute guidelines at 
        //  http://go.microsoft.com/fwlink/?LinkId=85236
       

        // This is a positional argument
        public SubCommandAttribute()
        {
           
        }

        public string CommandName { get; set; }
        public bool IsDefaultCommand { get; set; } = false;
    }
}
