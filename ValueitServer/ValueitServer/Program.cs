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
        private static string connectionString = "";

        TcpListener listener = new TcpListener(IPAddress.Any, 19023);
        Thread tcpThread; 
        TcpClient client;

        public void setConnection()
        {
            parseConnectionString();
        }

        private void serverRun()
        {
            
            if (!this.listener.Pending())
            {
                client = listener.AcceptTcpClient();
                tcpThread = new Thread(new ParameterizedThreadStart(tcpHandler));
                tcpThread.Start(client);
            }
            
        }

        private string parseConnectionString()
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

            connect = "address=" + address + ";database=" + name + ";uid=" + uid + ";pwd=" + password;

            return connect;
        }
    

    private static void tcpHandler(object client)
    {
           
        TcpClient mClient = (TcpClient)client;
        Program gr = new Program();
        NetworkStream stream = mClient.GetStream();

        int d = 0;

        try
        {
            byte[] byteStream = new byte[1040];
            stream.Read(byteStream, 0, byteStream.Length);
            int[] bytesAsInts = byteStream.Select(x => (int)x).ToArray();
            int i = 0;
            int g;
            try
            {
                while (true)
                {
                    g = bytesAsInts[i];

                    if (g > 0)
                    {
                        i++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {

            }

            Array.Resize(ref byteStream, i);
            string command = System.Text.Encoding.ASCII.GetString(byteStream);

            //Query from the SQL server
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(command, conn);
            MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);

            //Serialize Query Results
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(dt);
            byte[] JSONbytes = Encoding.ASCII.GetBytes(JSONString);
            Console.WriteLine(JSONString);
            Console.WriteLine(JSONbytes);
            stream.Write(JSONbytes, 0, JSONbytes.Length);

            gr.tcpThread.Abort();
            conn.Close();

            mClient.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Connection Error");
        }
    
    }
    public static void Main(string[] args)
    {
        Program gram = new Program();
        gram.listener.Start();
        Program.connectionString = gram.parseConnectionString();

            while (true){ 
                gram.serverRun();
            }
        }
    }
}
 

