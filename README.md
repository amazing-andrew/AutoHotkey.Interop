AutoHotkey.Interop
==================

This project is a basic wrapper around [HotKeyIt's AHKDLL](https://github.com/HotKeyIt/ahkdll) (aka **AutoHotkey_H**) for .Net Projects.

This was made to be intended to use as is, without needing to register any of the com components or any other special deployment tasks. The required (AutoHotkey_H) AutoHotkey.dll file can be deployed via XCOPY. The application can automatically deploy the required AutoHotkey.dll file if it is missing, as it stores a copy as an embeded resource.

All the thanks goes to HotKeyIt's work 

Usage & Examples
====================
To use, just include my DLL and it will deploy any files that are missing if needed.


```cs

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


```




Links
=============

[HotKeyIt's AutoHotkey_H Github Repository](https://github.com/HotKeyIt/ahkdll)  
[AutoHotKey_H Dll Documentation](http://www.autohotkey.net/~HotKeyIt/AutoHotkey/files/AutoHotkey-dll-txt.html)  




