using AutoHotkey.Interop.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AutoHotkey.Interop.Tests.Util
{
    public class CurrentDirectorySaverTests
    {
        [Fact]
        public void can_revert_current_directory_changes() {

            string before = Environment.CurrentDirectory;

            using(new CurrentDirectorySaver()) {
                Environment.CurrentDirectory = Path.GetTempPath();
                string changed = Environment.CurrentDirectory;

                //assert directory was changed
                Assert.Equal(changed, Environment.CurrentDirectory);
            }
            
            //assert that changes were reverted
            Assert.Equal(before, Environment.CurrentDirectory);
        }
    }
}
