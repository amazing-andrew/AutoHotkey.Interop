using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AutoHotkey.Interop
{
    internal static class Util
    {
        public static string FindEmbededResourceName(Assembly assembly, string path)
        {
            path = Regex.Replace(path, @"[/\\]", ".");

            if (!path.StartsWith("."))
                path = "." + path;
            
            var names = assembly.GetManifestResourceNames();

            foreach (var name in names)
            {
                if (name.EndsWith(path))
                {
                    return name;
                }
            }

            return null;
        }

        public static void ExtractEmbededResourceToFile(Assembly assembly, string embededResourcePath, string targetFileName)
        {
            //ensure directory exists
            var dir = Path.GetDirectoryName(targetFileName);

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            using (var readStream = assembly.GetManifestResourceStream(embededResourcePath))
            using (var writeStream = File.Open(targetFileName, FileMode.Create))
            {
                readStream.CopyTo(writeStream);
                readStream.Flush();
            }
        }

        public static bool Is64Bit()
        {
            return IntPtr.Size == 8;
        }
        public static bool Is32Bit()
        {
            return IntPtr.Size == 4;
        }



        public static void EnsureAutoHotkeyLoaded()
        {
            if (dllHandle.IsValueCreated)
                return;

            var handle = dllHandle.Value;
        }

        private static Lazy<SafeLibraryHandle> dllHandle = new Lazy<SafeLibraryHandle>(
            () => Util.LoadAutoHotKeyDll());
        private static SafeLibraryHandle LoadAutoHotKeyDll()
        {
            //Locate and Load 32bit or 64bit version of AutoHotkey.dll
            string tempFolderPath = Path.Combine(Path.GetTempPath(), "AutoHotkey.Interop");
            string path32 = @"x86\AutoHotkey.dll";
            string path64 = @"x64\AutoHotkey.dll";

            var loadDllFromFileOrResource = new Func<string, SafeLibraryHandle>(relativePath =>
            {
                if (File.Exists(relativePath))
                {
                    return SafeLibraryHandle.LoadLibrary(relativePath);
                }
                else
                {
                    var assembly = typeof(AutoHotkeyEngine).Assembly;
                    var resource = Util.FindEmbededResourceName(assembly, relativePath);

                    if (resource != null)
                    {
                        var target = Path.Combine(tempFolderPath, relativePath);
                        Util.ExtractEmbededResourceToFile(assembly, resource, target);
                        return SafeLibraryHandle.LoadLibrary(target);
                    }

                    return null;
                }
            });


            if (Util.Is32Bit())
            {
                return loadDllFromFileOrResource(path32);
            }
            else if (Util.Is64Bit())
            {
                return loadDllFromFileOrResource(path64);
            }
            else
            {
                return null;
            }
        }
    }
}
