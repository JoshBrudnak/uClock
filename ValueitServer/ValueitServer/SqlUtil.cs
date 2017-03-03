using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ValueitServer
{
    class SqlUtil
    {
       
        public string parseConnectionString()
        {
            string connect;
            string name = "";
            string address = "";
            string uid = "";
            string password = "";

            XmlReader doc = XmlReader.Create("datasource.xml");
            doc.Read();
            doc.ReadStartElement("connectionString");

            doc.ReadStartElement("name");
            name = doc.ReadString();
            doc.ReadEndElement();

            doc.ReadStartElement("address");
            address = doc.ReadString();
            doc.ReadEndElement();

            doc.ReadStartElement("uid");
            uid = doc.ReadString();
            doc.ReadEndElement();

            doc.ReadStartElement("password");
            password = doc.ReadString();
            doc.ReadEndElement();

            doc.ReadEndElement();

            connect = "server=" + name + ";database=" + address + ";uid=" + uid + ";pwd=" + password + ";";

            return connect;
        }
    }
}
