using UnityEngine;
using Mirror;

public class BattleController_Network : NetworkBehaviour
{
    private BattleController matchController;
    private NetworkManager networkManager;

    [SyncVar] public CharacterController playerOne;
    [SyncVar] public CharacterController playerTwo;

    public static BattleController_Network Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        matchController = GetComponent<BattleController>();
        networkManager = NetworkManager.singleton;
    }

    public void Network_RegisterPlayerOnMatch(NetworkIdentity playerNetworkIdentity)
    {
        if (isServer)
            Rpc_RegisterPlayerOnMatch(playerNetworkIdentity);
        else
            Cmd_RegisterPlayerOnMatch(playerNetworkIdentity);
    }
    public void RegisterPlayerOnMatch(NetworkIdentity playerNetworkIdentity)
    {
        CharacterController player = playerNetworkIdentity.GetComponent<CharacterController>();

        if (playerOne == null)
            playerOne = player;
        else if (playerTwo == null)
            playerTwo = player;
    }
    [Command]
    private void Cmd_RegisterPlayerOnMatch(NetworkIdentity playerNetworkIdentity)
    {
        Debug.Log("Cmd_RegisterPlayerOnMatch");
        Rpc_RegisterPlayerOnMatch(playerNetworkIdentity);
    }

    [ClientRpc]
    private void Rpc_RegisterPlayerOnMatch(NetworkIdentity playerNetworkIdentity)
    {
        Debug.Log("Rpc_RegisterPlayerOnMatch");
        RegisterPlayerOnMatch(playerNetworkIdentity);
        ShowPlayers();
    }

    public void ShowPlayers()
    {
        if (playerOne != null)
        {
            Debug.Log("Player one: " + playerOne.GetComponent<NetworkConnection>().connectionId);
        }

        if (playerTwo != null)
        {
            Debug.Log("Player two: " + playerTwo.GetComponent<NetworkConnection>().connectionId);
        }
    }

    public void RemovePlayers() => playerOne = playerTwo = null;
}
