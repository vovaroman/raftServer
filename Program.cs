using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using ServerRaft;

namespace ServerRaft
{
    class Program
    {
        public static List<Client> Clients = new List<Client>();
        static void Main(string[] args)
        {

            Console.WriteLine($"Server started at IP - {Helper.GetLocalIPAddress()} and Port - {Helper.UdpPort}");
            var udpServer = new UDPServer();
            var listenUDP = new Thread(() => udpServer.ListenUDP());
            var sendActiveClients = new Thread(() => udpServer.SendActiveClients());
            listenUDP.Start();
            sendActiveClients.Start();

        }
    }
}
