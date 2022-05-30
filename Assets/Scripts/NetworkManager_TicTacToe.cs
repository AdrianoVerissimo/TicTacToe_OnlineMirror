﻿using UnityEngine;
using Mirror;

public class NetworkManager_TicTacToe : NetworkManager
{
    public static NetworkManager_TicTacToe Instance;

    public override void Start()
    {
        base.Start();
        ShouldCreateSingleton();
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

    public void ShouldCreateSingleton()
    {
        bool hasSingleton = Instance != null;
        if (hasSingleton)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void HandleClientConnected()
    {

    }
    private void HandleClientDisconnected()
    {

    }

    public static void HostLobby()
    {
        Instance.StartHost();
    }
    public static void JoinLobby()
    {
        string ipAddress = "localhost";

        Instance.networkAddress = ipAddress;
        Instance.StartClient();
    }
}
