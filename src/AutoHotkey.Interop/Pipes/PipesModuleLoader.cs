using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoHotkey.Interop.Pipes
{
    public static class PipesModuleLoader
    {
        public static object lockObj = new object();
        internal static MessageHandlerPipeServer Server { get; private set; }

        public static void LoadPipesModule(Func<string, string> messageHandler) { 
            lock (lockObj) {
                var pipename = GeneratePipeName();
                InitPipeServer(pipename, messageHandler);
                InitPipeClient(pipename);
            }
        }

        private static void InitPipeClient(string pipename) {
            //only load pipe client once, by checking for pipeclient_getversion function
            if (!AutoHotkeyEngine.Instance.FunctionExists("pipeclient_getversion")) {
                var ahk_pipeclient_lib = Util.EmbededResourceHelper.ExtractToText(typeof(PipesModuleLoader).Assembly, "Pipes/pipeclient.ahk");
                AutoHotkeyEngine.Instance.LoadScript(ahk_pipeclient_lib);
            }
            else {
                AutoHotkeyEngine.Instance.LoadScript("A__PIPECLIENT.close()");
            }

            AutoHotkeyEngine.Instance.LoadScript(string.Format(
                    "A__PIPECLIENT := new pipeclient({0})",
                    AhkEscape.Quote(pipename)));
        }

        private static void InitPipeServer(string pipename, Func<string, string> messageHandler) {
            if (Server != null) {
                Server.Shutdown();
            }

            Server = new MessageHandlerPipeServer(pipename, messageHandler);
            Server.Start();
        }

        private static string GeneratePipeName() {
            return "AHK-PIPE-" + Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}
