using System;

namespace ServerRaft
{
    public enum ServerActions
    {
        Ping,
        GetClients,

        GetLeader,

        SendToLeader,

        GetFromLeader
    }
}
