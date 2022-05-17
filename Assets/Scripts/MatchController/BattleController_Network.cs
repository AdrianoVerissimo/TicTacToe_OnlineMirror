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
            RegisterPlayerOnMatch(playerNetworkIdentity);
        else
            Cmd_RegisterPlayerOnMatch(playerNetworkIdentity);
    }
    private void RegisterPlayerOnMatch(NetworkIdentity playerNetworkIdentity)
    {
        CharacterController player = playerNetworkIdentity.GetComponent<CharacterController>();

        if (playerOne == null)
            playerOne = player;
        else if (playerTwo == null)
            playerTwo = player;
    }

    [Command(channel = 0, requiresAuthority = false)]
    private void Cmd_RegisterPlayerOnMatch(NetworkIdentity playerNetworkIdentity) => RegisterPlayerOnMatch(playerNetworkIdentity);

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

    public bool CheckAllPlayersConnected()
    {
        bool allPlayersConnected = playerOne != null && playerTwo != null;
        return allPlayersConnected;
    }
    public void Network_ShouldStartMatch()
    {
        if (isServer)
            ShouldStartMatch();
        else
            Cmd_ShouldStartMatch();
    }
    private void ShouldStartMatch()
    {
        bool allPlayersConnected = CheckAllPlayersConnected();
        bool canStartMatch = allPlayersConnected;

        Debug.Log("can start match: " + canStartMatch);

        if (!canStartMatch)
            return;
    }
    [Command(channel = 0, requiresAuthority = false)]
    private void Cmd_ShouldStartMatch()
    {
        ShouldStartMatch();
    }

}
