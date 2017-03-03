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
        private string myConnectionString = "";
        TcpListener listener = new TcpListener(IPAddress.Any, 19023);
        Thread tcpThread; 
        TcpClient client;

        

        private void serverRun()
        {
            
            if (!this.listener.Pending())
            {
                client = listener.AcceptTcpClient();
                tcpThread = new Thread(new ParameterizedThreadStart(tcpHandler));
                tcpThread.Start(client);
            }
            
        }

        private void tcpHandler(object client)
        {
           
            TcpClient mClient = (TcpClient)client;
            
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
                    Console.WriteLine("Connection Error");
                }

                Array.Resize(ref byteStream, i);
                string command = System.Text.Encoding.ASCII.GetString(byteStream);

                //Query from the SQL server
                MySqlConnection conn = new MySqlConnection(myConnectionString);
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

                tcpThread.Abort();
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
            SqlUtil util = new SqlUtil();

            gram.listener.Start();
            gram.myConnectionString = util.parseConnectionString();

                while (true){ 
                    gram.serverRun();
                }
        }
    }
}
 

