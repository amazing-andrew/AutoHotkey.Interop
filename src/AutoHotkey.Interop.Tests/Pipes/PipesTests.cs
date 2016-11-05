using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AutoHotkey.Interop.Tests.Pipes
{
    public class PipesTests
    {
        AutoHotkeyEngine ahk = AutoHotkeyEngine.Instance;

        public void init_pipes() {
            Func<string, string> EchoHandler = new Func<string, string>(msg => "SERVER:" + msg);
            AutoHotkey.Interop.Pipes.PipesModuleLoader.LoadPipesModule(EchoHandler);
        }

        [Fact]
        public void loading_pipes_module_mutliple_times_has_no_errors() {
            init_pipes();
            init_pipes();
            init_pipes();
            init_pipes();
            init_pipes();
        }

        [Fact]
        public void loading_pipes_library_has_SendPipeMessage_function() {
            init_pipes();
            Assert.True(ahk.FunctionExists("SendPipeMessage"));
        }

        [Fact]
        public void test_pipe_communication() {
            init_pipes();

            string ahk_code =
                @"serverMessage := SendPipeMessage(""Hello"")
                 ";
            ahk.LoadScript(ahk_code);
            Assert.Equal("SERVER:Hello", ahk.GetVar("serverMessage"));
        }
    }
}
