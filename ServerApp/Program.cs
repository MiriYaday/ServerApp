using MySql.Data.MySqlClient;
using System;
using System.Net;
using System.Net.Mail;

namespace ServerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server();
            server.Start();
        }
    }
}
