using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoHotkey.Interop.Util
{
    internal static class ProcessorType
    {
        public static bool Is64Bit() {
            return IntPtr.Size == 8;
        }
        public static bool Is32Bit() {
            return IntPtr.Size == 4;
        }

    }
}
