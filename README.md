AutoHotkey.Interop
==================

This project is a basic wrapper around [HotKeyIt's AHKDLL](https://github.com/HotKeyIt/ahkdll) (aka **AutoHotkey_H**) for .Net Projects.

This also includes a simple communication module to allow AHK to communicate with the .NET hosting environment using a simple method `SendPipeMessage(string)` from AutoHotkey. *see example below*

This was made to be intended to use as is, without needing to register any of the com components or any other special deployment tasks. The required (AutoHotkey_H) AutoHotkey.dll can either be deployed via XCOPY or the application will automatically deploy the required AutoHotkey.dll file if it is missing, as it stores a copy as an embeded resource.

All the thanks goes to [HotKeyIt's](https://github.com/HotKeyIt) work 

Usage & Examples
====================
To use, just include my DLL and it will deploy any files that are missing if needed.

```cs
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

ahk.ExecRaw(@"serverResponse := SendPipeMessage(""Hello from ahk"")
              MsgBox, response from server was -- %serverResponse% ");

```

Links
=============

[HotKeyIt's AutoHotkey\_H Github Repository](https://github.com/HotKeyIt/ahkdll)  
[AutoHotKey\_H Dll Documentation](http://hotkeyit.github.io/v2/)  




