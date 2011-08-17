using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;
using Notifier.Model;
using Notifier.Model.Monitor;
using System.IO;
using Notifier.Configuration;
using System;

namespace NotifierTest.Model
{
    [TestFixture]
    public class NotificationModelTest
    {
        string fileExistTestFilePath;
        string recordCountTestFilePath;

        [SetUp]
        public void Init()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            fileExistTestFilePath = currentDirectory + @"\FileExistTest.xml";
            recordCountTestFilePath = currentDirectory + @"\RecordCountTest.xml";
        }

        [Test]
        public void FromXmlFileExistTest()
        {
            INotificationModel model = ConfigurationManager.FromXml(fileExistTestFilePath);
            Assert.AreEqual(1, model.Items.Count);
            Assert.AreEqual(typeof(FileExistMonitor), model.Items[0].GetType());
            FileExistMonitor monitor = model.Items[0] as FileExistMonitor;
            Assert.AreEqual("C:", monitor.Path);
            Assert.AreEqual(0, monitor.Limit);
        }

        [Test]
        public void FromXmlRecordCountTest()
        {
            INotificationModel model = ConfigurationManager.FromXml(recordCountTestFilePath);
            Assert.AreEqual(3, model.Items.Count);

            // basic
            Assert.AreEqual(typeof(RecordCountMonitor), model.Items[0].GetType());
            RecordCountMonitor monitor = model.Items[0] as RecordCountMonitor;
            Assert.IsNotNull(monitor.DbConnection);
            Assert.AreEqual("Data Source=myServerAddress;Initial Catalog=myDataBase;User Id=myUsername;Password=myPassword;"
                , monitor.DbConnection.ConnectionString);
            Assert.AreEqual("SELECT * FROM TEST", monitor.CommandText);
            Assert.AreEqual(10, monitor.Limit);

            // with parameters
            Assert.AreEqual(typeof(RecordCountMonitor), model.Items[1].GetType());
            monitor = model.Items[1] as RecordCountMonitor;
            Assert.IsNotNull(monitor.DbConnection);
            Assert.AreEqual("Data Source=myServerAddress;Initial Catalog=myDataBase;User Id=myUsername;Password=myPassword;"
                , monitor.DbConnection.ConnectionString);
            Assert.AreEqual("SELECT * FROM TEST WHERE PKEY=@PKEY", monitor.CommandText);
            Assert.AreEqual(3, monitor.Parameters.Count);
            Assert.AreEqual(monitor.Parameters["@PKEY"], 1);
            Assert.AreEqual(monitor.Parameters["@STRING"], "Test String");
            Assert.AreEqual(monitor.Parameters["@DATE"], new DateTime(2011, 1, 1, 12, 34, 56));
            Assert.AreEqual(10, monitor.Limit);

            // commandtext from file
            Assert.AreEqual(typeof(RecordCountMonitor), model.Items[2].GetType());
            monitor = model.Items[2] as RecordCountMonitor;
            Assert.IsNotNull(monitor.DbConnection);
            Assert.AreEqual("Data Source=myServerAddress;Initial Catalog=myDataBase;User Id=myUsername;Password=myPassword;"
                , monitor.DbConnection.ConnectionString);
            Assert.AreEqual("SELECT GETDATE()", monitor.CommandText);
            Assert.AreEqual(0, monitor.Limit);
        }
    }
}
