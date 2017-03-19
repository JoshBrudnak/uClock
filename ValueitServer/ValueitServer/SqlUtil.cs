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
        public string getConnectionString()
        {
            System.IO.StreamReader file = new System.IO.StreamReader("DataSource.txt");

            string connection = file.ReadLine();

            return connection;
        }
    }
}
