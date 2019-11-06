using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

public static class Helper
{
    public static int UdpPort = 616;

    public static int TcpPort = 13000;

    public static string GetLocalIPAddress()
    {
       string localIP;
        using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
        {
            socket.Connect("8.8.8.8", 65530);
            IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
            localIP = endPoint.Address.ToString();
        }
        return localIP;
    }
    public static string ExternalIp()
    {
        string externalip = new WebClient().DownloadString("http://icanhazip.com");            
        return externalip;
    }
}