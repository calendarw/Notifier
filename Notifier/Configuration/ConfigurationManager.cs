using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using Notifier.Model;
using Notifier.Model.Monitor;

namespace Notifier.Configuration
{
    public class ConfigurationManager
    {
        private const string Salt = @"Notifier";

        public static INotificationModel FromXml(string path)
        {
            XDocument loaded = XDocument.Load(path);

            NotificationModel model = new NotificationModel();

            foreach (XElement e in loaded.Element("settings").Elements("monitor"))
            {
                string type = e.Attribute("type").Value;
                string typeName = string.Format(@"Notifier.Model.Monitor.{0}", type);
                
                Type monitorType = Type.GetType(typeName);  // exception should throw invalid type

                IMonitor monitor = Activator.CreateInstance(monitorType) as IMonitor;

                if (monitor == null)
                    throw new NotSupportedException("Invalid type attribute for monitor");

                PropertyInfo[] properties = monitorType.GetProperties();
                foreach (XElement p in e.Elements())
                {
                    string elementName = p.Name.LocalName;
                    object value = p.Value;

                    if (value == null)
                        throw new ArgumentNullException(elementName, string.Format("Value is empty for element: {0}", elementName));

                    PropertyInfo property = properties.First(o => o.Name.ToLower().Equals(elementName.ToLower()));
                    if (property == null)
                        throw new ArgumentException(string.Format("Property not found for element: {0}", elementName), elementName);

                    if (property.PropertyType.Equals(typeof(System.Data.IDbConnection)))
                    {
                        value = new System.Data.SqlClient.SqlConnection(value.ToString().Decrypt(Salt));
                    }
                    else if (property.PropertyType.Equals(typeof(Dictionary<string, object>))){
                        Dictionary<string, object> parameters = new Dictionary<string, object>();

                        foreach (XElement pa in p.Elements("parameter"))
                        {
                            string pName = null;
                            string pType = null;

                            foreach (XAttribute a in pa.Attributes())
                            {
                                switch (a.Name.LocalName.ToLower())
                                {
                                    case "name":
                                        pName = a.Value;
                                        break;

                                    case "type":
                                        pType = a.Value;
                                        break;
                                }
                            }

                            if (pName == null)
                                throw new NullReferenceException("name attribute is missing value in {0}");

                            parameters.Add(pName, Convert.ChangeType(pa.Value, Type.GetType(string.Format(@"System.{0}", pType ?? "String"), true, true)));
                        }

                        value = parameters;
                    }
                    else
                    {
                        if (!value.GetType().Equals(property.PropertyType))
                        {
                            value = Convert.ChangeType(value, property.PropertyType);
                        }
                    }
                    property.SetValue(monitor, value, null);
                }
                model.Add(monitor);
            }

            return model;
        }

        public static string EncryptConnectionString(string plain)
        {
            return plain.Encrypt(Salt);
        }
    }
}
