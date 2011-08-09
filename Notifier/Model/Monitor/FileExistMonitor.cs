using System;
using System.Diagnostics;
using System.IO;

namespace Notifier.Model.Monitor
{
    public class FileExistMonitor : OverLimitMonitor
    {
        private DirectoryInfo mDirInfo;
        private string mPath;

        public FileExistMonitor()
        {
            SearchPattern = "*";
            SearchOption = System.IO.SearchOption.TopDirectoryOnly;
        }

        public string Path
        {
            get { return mPath; }
            set { mPath = value; mDirInfo = null; }
        }

        public SearchOption SearchOption { get; set; }

        public string SearchPattern { get; set; }

        protected override void PerformCheck()
        {
            Debug.Assert(!string.IsNullOrEmpty(Path), "Path should not be empty");

            if (mDirInfo == null)
                mDirInfo = new DirectoryInfo(Path);

            FileInfo[] files = mDirInfo.GetFiles(SearchPattern, SearchOption);

            Current = files.Length;
        }
    }
}