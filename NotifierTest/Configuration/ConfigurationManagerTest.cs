using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;
using Notifier.Model;
using Notifier.Model.Monitor;
using System.IO;
using Notifier.Configuration;

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
            Assert.AreEqual(1, model.Items.Count);
            Assert.AreEqual(typeof(RecordCountMonitor), model.Items[0].GetType());
            RecordCountMonitor monitor = model.Items[0] as RecordCountMonitor;
            Assert.IsNotNull(monitor.DbConnection);
            Assert.AreEqual("Data Source=myServerAddress;Initial Catalog=myDataBase;User Id=myUsername;Password=myPassword;"
                , monitor.DbConnection.ConnectionString);
            Assert.AreEqual("SELECT * FROM TEST", monitor.CommandText);
            Assert.AreEqual(10, monitor.Limit);
        }
    }
}
