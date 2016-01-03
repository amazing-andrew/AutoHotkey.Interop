using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoHotkey.Interop.Util
{
    /// <summary>
    /// The newer version of the AHK DLL will often change the Enviorment.CurrentDirectory values.
    /// This is a simple utility class to ensure that we always change it back :)
    /// </summary>
    internal class CurrentDirectorySaver : IDisposable
    {
        private string current_directory = null;

        public CurrentDirectorySaver() {
            current_directory = Environment.CurrentDirectory;
        }

        public void Dispose() {
            if (Environment.CurrentDirectory != current_directory)
                Environment.CurrentDirectory = current_directory;
        }
    }
}
