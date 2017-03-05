namespace ValueitServer
{
    using System;
    using System.Data;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;
    using System.Xml;
    using MySql.Data.MySqlClient;
    using Newtonsoft.Json;

    class Program
    {
        public static void Main(string[] args)
        {
            SqlUtil util = new SqlUtil();
            Server server = new Server();

            server.startListener();
            server.myConnectionString = util.parseConnectionString();

                while (true){ 
                    server.serverRun();
                }
        }
    }
}
 

