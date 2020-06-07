using System;
using Xunit;
using System.Collections.Generic;
using System.IO;
using CommandLine;

namespace LogToCsv.Tests
{
    public class DirectoryWalkerTest
    {
        [Fact]
        public void IsDirTreeWalked_ReturnTrue()
        {
            Console.WriteLine(Directory.GetCurrentDirectory());
            DirectoryInfo rootDirInfo = new DirectoryInfo(Directory.GetCurrentDirectory());
            var result = DirectoryWalker.WalkDirectoryTree(rootDirInfo, false);
            Assert.True(result, "Directory tree could be walked");
        }

        [Fact]
        public void IsDirTreeWalked_ReturnFalse()
        {
            DirectoryInfo rootDirInfo = new DirectoryInfo(Directory.GetCurrentDirectory()+"xxx");
            var result = DirectoryWalker.WalkDirectoryTree(rootDirInfo, false);
            Assert.False(result, "Directory tree could not be walked");
        }
    }
}
