using UnityEngine;
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

    public override void OnStopHost()
    {
        
    }
    public override void OnStopClient()
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
    public static void ExitLobby()
    {
        bool isServer = CharacterController.LocalPlayer.isServer;

        if (isServer)
            Instance.StopHost();
        else
            Instance.StopClient();
    }

    public void Network_ExitLobby()
    {
        
    }
}
