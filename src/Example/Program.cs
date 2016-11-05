using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoHotkey.Interop;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            //grab a copy of the AutoHotkey singleton instance
            var ahk = AutoHotkeyEngine.Instance;

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
            ahk.LoadFile("functions.ahk");

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
            Console.WriteLine("Eval: Result of 5 with Add5 func is {0}", add5Results);

            //you can also return results with the ExecFunction 
            add5Results = ahk.ExecFunction("Add5", "5");
            Console.WriteLine("ExecFunction: Result of 5 with Add5 func is {0}", add5Results);

            //you can have AutoHotkey communicate with the hosting environment 
            // 1 - Create Handler for your ahk code 
            // 2 - Initalize Pipes Module, passing in your handler
            // 3 - Use 'SendPipeMessage(string)' from your AHK code
            var ipcHandler = new Func<string, string>(fromAhk => {
                Console.WriteLine("received message from ahk " + fromAhk);
                System.Threading.Thread.Sleep(3000); //simulating lots of work
                return ".NET: I LIKE PIE!";
            });

            //the initalize pipes module only needs to be called once per application
            ahk.InitalizePipesModule(ipcHandler); 

            ahk.ExecRaw(@"serverResponce := SendPipeMessage(""Hello from ahk"")
                          MsgBox, responce from server was -- %serverResponce% ");

            Console.WriteLine("Press enter to exit...");
            Console.ReadLine();
        }
    }
}
