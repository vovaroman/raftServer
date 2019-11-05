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
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        throw new Exception("No network adapters with an IPv4 address in the system!");
    }
    public static string ExternalIp()
    {
        string externalip = new WebClient().DownloadString("http://icanhazip.com");            
        return externalip;
    }
}