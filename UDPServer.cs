using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using ServerRaft;

namespace ServerRaft
{
    
    public class UDPServer
    {
        private static UdpClient udpServer = new UdpClient(Helper.UdpPort);

        private static Client Leader = new Client();
        public void ListenUDP()
        {
            while (true)
            {
                IPEndPoint RemoteIpEndPoint = new IPEndPoint(
                    IPAddress.Any,
                    Helper.UdpPort
                    );
                Byte[] receiveBytes = udpServer.Receive(ref RemoteIpEndPoint);
                string returnData = Encoding.ASCII.GetString(receiveBytes);
                JObject data = new JObject();
                data = JObject.Parse(returnData);
                Console.WriteLine(data);
                Enum.TryParse(data["action"].ToString(), out ServerActions action);

                switch(action)
                {
                    case ServerActions.GetClients:
                        var clients = data["clients"];
                        if(Program.Clients.FirstOrDefault(x => x.IP == clients["IP"].ToString() && x.Port == int.Parse(clients["Port"].ToString())) == null)
                            Program.Clients.Add(
                                new Client(){
                                    IP = clients["IP"].ToString(),
                                    Port = int.Parse(clients["Port"].ToString())
                                }
                            );
                        Console.WriteLine(clients);
                        break;
                    case ServerActions.GetLeader:
                        Leader.IP = RemoteIpEndPoint.Address.ToString();
                        Leader.Port = RemoteIpEndPoint.Port;
                        break;
                    case ServerActions.GetFromLeader:
                        var returnMessage = data["data"];
                        SendData(returnMessage, RemoteIpEndPoint.Address.ToString(), RemoteIpEndPoint.Port);
                        break;
                    case ServerActions.SendToLeader:
                        var message = data["data"];
                        SendData(message, Leader.IP, Leader.Port);
                        break;
                }
            }
        }

        public void SendData(object data, string IP, int Port)
        {
            var dataToSend = new Dictionary<string, object>();
            dataToSend.Add("action","SendToLeader");
            dataToSend.Add("data",data);
            var messageToSend = Newtonsoft.Json.JsonConvert.SerializeObject(dataToSend);
            byte[] byteMessage = Encoding.UTF8.GetBytes(messageToSend);
            UdpClient udpClient = new UdpClient();
            udpClient.Send(byteMessage, byteMessage.Length, IP, Port);
        }

        public void SendActiveClients()
        {
            while(true)
            {
                foreach (var client in Program.Clients)
                {
                    UdpClient udpClient = new UdpClient();

                    var dataToSend = new Dictionary<string, object>();
                    dataToSend.Add("action","GetClients");
                    // var clients = Newtonsoft.Json.JsonConvert.SerializeObject(node);
                    dataToSend.Add("clients",Program.Clients);
                    var message = Newtonsoft.Json.JsonConvert.SerializeObject(dataToSend);
                    byte[] data = Encoding.UTF8.GetBytes(message);
                    udpClient.Send(data, data.Length, client.IP, client.Port);
                    udpClient.Close();
                }
                new System.Threading.ManualResetEvent(false).WaitOne(1);

            }
        }
    }
}
