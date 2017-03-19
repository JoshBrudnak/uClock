namespace ValueitServer
{
    class Program
    {
        public static void Main(string[] args)
        {
            SqlUtil util = new SqlUtil();
            Server server = new Server();

            server.startListener();
            server.myConnectionString = util.getConnectionString();

                while (true){ 
                    server.serverRun();
                }
        }
    }
}
 

