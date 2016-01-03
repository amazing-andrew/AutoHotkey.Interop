using AutoHotkey.Interop.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AutoHotkey.Interop.Tests.Util
{
    public class EmbededAhkDllFilesTests
    {
        [Fact]
        public void has_x86_ahkdll_embeded() {
            VerifyEmbededResource("AutoHotkey.Interop.x86.AutoHotkey.dll");
        }

        [Fact]
        public void has_x64_ahkdll_embeded() {
            VerifyEmbededResource("AutoHotkey.Interop.x64.AutoHotkey.dll");
        }

        private static void VerifyEmbededResource(string embededResourceFullName) {
            var interopAssembly = typeof(AutoHotkeyEngine).Assembly;
            var assemblyResourceNames = interopAssembly.GetManifestResourceNames();
            bool foundResource = assemblyResourceNames.Contains(embededResourceFullName);

            Assert.True(foundResource,
                string.Format("Could not find the embeded resource '{0}'", embededResourceFullName));
        }

       
    }
}
