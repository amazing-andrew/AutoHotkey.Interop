using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            //create an autohtkey engine.
            var ahk = new AutoHotkey.Interop.AutoHotkeyEngine();

            //execute any raw ahk code
            ahk.ExecRaw("MsgBox, Hello World!");

            //create new hotkeys
            ahk.ExecRaw("^a::Send, Hello World");
            
            //programmatically set variables
            ahk.SetVar("x", "1");
            ahk.SetVar("y", "4");

            //execute statements
            ahk.ExecRaw("z:=x+y");

            //return variables back from ahk
            string zValue = ahk.GetVar("z");
            Console.WriteLine("Value of z is {0}", zValue); // "Value of z is 5"

            //Load a library or exec scripts in a file
            ahk.Load("functions.ahk");

            //execute a specific function (found in functions.ahk), with 2 parameters
            ahk.ExecFunction("MyFunction", "Hello", "World");

            //execute a label 
            ahk.ExecLabel("DOSTUFF");

            //create a new function
            string sayHelloFunction = "SayHello(name) \r\n { \r\n MsgBox, Hello %name% \r\n return \r\n }";
            ahk.ExecRaw(sayHelloFunction);

            //execute's newly made function\
            ahk.ExecRaw(@"SayHello(""Mario"") ");


            //execute a function (in functions.ahk) that adds 5 and return results
            var add5Results = ahk.Eval("Add5( 5 )");
            Console.WriteLine("Result of 5 with Add5 Method is {0}", add5Results);

            
            Console.WriteLine("Press enter to exit...");
            Console.ReadLine();
        }
    }
}
