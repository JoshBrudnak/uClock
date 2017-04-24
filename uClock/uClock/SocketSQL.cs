using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace uClock
{
    public class SocketSQL
    {
        string ip1;
        int tcp1;

        TcpClient client;

        public SocketSQL(string ip, int tcp)
        {
            ip1 = ip;
            tcp1 = tcp;
        }

        private SocketSQL() {
        }

        public bool sendPacket(NetworkStream stream, string data)
        {
            try
            {
                byte[] message = Encoding.ASCII.GetBytes(data);
                stream.Write(message, 0, message.Length);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }

        }

        public void readPacket(NetworkStream stream)
        {

        }

        public string NetSQLCommand(int packetNum, string comm, string[] parameters)
        {
            client = new TcpClient();
            client.Connect(IPAddress.Parse(ip1), tcp1);
            NetworkStream stream = client.GetStream();
            byte[] byteStream = new byte[2040];

            this.sendPacket(stream, packetNum.ToString());
            stream.Read(byteStream, 0, byteStream.Length);
            this.sendPacket(stream, comm);

            for (int j = 0; j < parameters.Length; j++)
            {
                this.sendPacket(stream, parameters[j]);
            }

            stream.Read(byteStream, 0, byteStream.Length);

            int[] bytesAsInts = new int[2040];
            bytesAsInts = byteStream.Select(x => (int)x).ToArray();

            int i = 0;
            int g = 1;

            while (g != 0)
            {
                g = bytesAsInts[i];
                i++;
            }

            Array.Resize(ref byteStream, i);

            string cmd = Encoding.ASCII.GetString(byteStream);
            return cmd;
            stream.Close();
            client.Close();

        }

        public string NetSQLCommand(string comm, string parameter)
        {
            client = new TcpClient();
            client.Connect(IPAddress.Parse(ip1), tcp1);
            NetworkStream stream = client.GetStream();
            byte[] bytestream = new byte[10];

            this.sendPacket(stream, "3");
            stream.Read(bytestream, 0, bytestream.Length);
            this.sendPacket(stream, comm);

            this.sendPacket(stream, parameter);
            stream.Read(bytestream, 0, bytestream.Length);

            byte[] byteStream = new byte[2040];
            stream.Read(byteStream, 0, byteStream.Length);

            int[] bytesAsInts = new int[2040];
            bytesAsInts = byteStream.Select(x => (int)x).ToArray();

            int i = 0;
            int g = 1;

            while (g != 0)
            {
                g = bytesAsInts[i];
                i++;
            }

            Array.Resize(ref byteStream, i);

            string cmd = Encoding.ASCII.GetString(byteStream);
            return cmd;
            stream.Close();
            client.Close();

        }

        public string NetSQLCommand(string comm)
        {
            client = new TcpClient();
            client.Connect(IPAddress.Parse(ip1), tcp1);
            NetworkStream stream = client.GetStream();
            byte[] bytestream = new byte[10];

            this.sendPacket(stream, "2");
            stream.Read(bytestream, 0, bytestream.Length);
            this.sendPacket(stream, comm);

            byte[] byteStream = new byte[2040];
            stream.Read(byteStream, 0, byteStream.Length);

            int[] bytesAsInts = new int[2040];
            bytesAsInts = byteStream.Select(x => (int)x).ToArray();

            int i = 0;
            int g = 1;

            while (g != 0)
            {
                g = bytesAsInts[i];
                i++;
            }

            Array.Resize(ref byteStream, i);

            string cmd = Encoding.ASCII.GetString(byteStream);
            return cmd;
            stream.Close();
            client.Close();
        }
    }
}
