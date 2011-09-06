using System.IO;
using Notifier.Model.Monitor;
using NUnit.Framework;

namespace NotifierTest.Model.Monitor
{
    [TestFixture]
    public class FileExistMonitorTest : OverLimitMonitorTest<FileExistMonitor>
    {
        private DirectoryInfo mDirInfo;
        private string mPath;

        protected override FileExistMonitor GetExceptionMonitor()
        {
            return new FileExistMonitor();
        }

        protected override FileExistMonitor GetNormalMonitor()
        {
            FileExistMonitor monitor = new FileExistMonitor();
            monitor.Path = mPath;
            return monitor;
        }

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            mPath = Directory.GetCurrentDirectory();
            mDirInfo = new DirectoryInfo(mPath);
        }

        [Test]
        public void TopDirectoryAllFilesTest()
        {
            string pattern = "*.*";
            FileInfo[] files = mDirInfo.GetFiles(pattern);
            FileExistMonitor monitor = GetNormalMonitor();
            monitor.SearchOption = SearchOption.TopDirectoryOnly;
            monitor.SearchPattern = pattern;
            monitor.Check();
            Assert.AreEqual(files.Length, monitor.Current);
        }

        [Test]
        public void TopDirectoryDllFilesTest()
        {
            string pattern = "*.dll";
            FileInfo[] files = mDirInfo.GetFiles(pattern);
            FileExistMonitor monitor = GetNormalMonitor();
            monitor.SearchOption = SearchOption.TopDirectoryOnly;
            monitor.SearchPattern = pattern;
            monitor.Check();
            Assert.AreEqual(files.Length, monitor.Current);
        }
    }
}
