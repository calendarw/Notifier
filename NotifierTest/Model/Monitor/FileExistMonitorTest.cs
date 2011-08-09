using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Notifier.Model.Monitor;
using System.IO;
using System.Diagnostics;

namespace NotifierTest.Model.Monitor
{
    [TestFixture]
    public class FileExistMonitorTest : OverLimitMonitorTest
    {
        private FileExistMonitor mMonitor;
        private DirectoryInfo mDirInfo;

        private string mPath;

        [SetUp]
        public void Init()
        {
            mPath = Directory.GetCurrentDirectory();
            mDirInfo = new DirectoryInfo(mPath);

            mMonitor = new FileExistMonitor();
            mMonitor.Path = mPath;
        }

        [Test]
        public void TopDirectoryAllFilesTest()
        {
            string pattern = "*.*";
            FileInfo[] files = mDirInfo.GetFiles(pattern);

            mMonitor.SearchOption = SearchOption.TopDirectoryOnly;
            mMonitor.SearchPattern = pattern;

            LimitTest(mMonitor, files.Length);
        }

        [Test]
        public void TopDirectoryDllFilesTest()
        {
            string pattern = "*.dll";
            FileInfo[] files = mDirInfo.GetFiles(pattern);

            mMonitor.SearchOption = SearchOption.TopDirectoryOnly;
            mMonitor.SearchPattern = pattern;

            LimitTest(mMonitor, files.Length);
        }
    }
}
