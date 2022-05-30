using UnityEngine;
using Mirror;

public class NetworkManager_TicTacToe : NetworkManager
{
    public override void Start()
    {
        base.Start();
    }

    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        base.OnServerConnect(conn);

        bool maxPlayersExceed = numPlayers >= maxConnections;
        if (maxPlayersExceed)
        {
            conn.Disconnect();
            return;
        }
    }
}
