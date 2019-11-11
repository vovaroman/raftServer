using System;

namespace ServerRaft
{
    public enum ServerActions
    {
        Ping,
        GetClients,
        
        Election,

        VoteForLeader,

        Vote,

        KeepFollower,

        GetLeader,

        SendToLeader,

        GetFromLeader,
        SendDataToClient
    }
}
