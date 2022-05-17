using UnityEngine;
using Mirror;

public class CharacterController : NetworkBehaviour
{
    public int PlayerID { get; private set; }

    public static int CountPlayerID { get; private set; } = 0;

    private NetworkIdentity networkIdentity;
    public GameObject battleController_NetworkPrefab;

    private void Awake()
    {
        networkIdentity = GetComponent<NetworkIdentity>();
    }

    public CharacterController SetPlayerID(int playerID)
    {
        this.PlayerID = playerID;
        return this;
    }

    public static void GeneratePlayerID(NetworkIdentity player)
    {
        CharacterController character = player.gameObject.GetComponent<CharacterController>();
        int newID = GetNewPlayerID();
        character.SetPlayerID(newID);
    }
    private static int GetNewPlayerID()
    {
        int newID = CountPlayerID;
        SetCountPlayerID(CountPlayerID + 1);

        return newID;
    }
    public static void ResetCountPlayerID()
    {
        SetCountPlayerID(0);
    }
    public static void SetCountPlayerID(int value)
    {
        CountPlayerID = value;
    }

    #region Network

    public override void OnStartClient()
    {
        if (!isLocalPlayer)
            return;

        Network_CreateBattleControllerNetwork();

        Network_RegisterPlayerOnMatch(netIdentity);
    }

    public void Network_CreateBattleControllerNetwork()
    {
        if (isServer)
        {
            Cmd_CreateBattleControllerNetwork();
        }
    }
    private void CreateBattleControllerNetwork()
    {
        if (BattleController_Network.Instance == null)
        {
            GameObject battleController_NetworkInstance = Instantiate(battleController_NetworkPrefab);
            NetworkServer.Spawn(battleController_NetworkInstance);
        }
    }

    [Command]
    private void Cmd_CreateBattleControllerNetwork()
    {
        CreateBattleControllerNetwork();
    }



    public void Network_RegisterPlayerOnMatch(NetworkIdentity playerNetworkIdentity)
    {
        if (isServer)
            RegisterPlayerOnMatch(playerNetworkIdentity);
        else
            Cmd_RegisterPlayerOnMatch(playerNetworkIdentity);
    }
    public void RegisterPlayerOnMatch(NetworkIdentity playerNetworkIdentity)
    {
        CharacterController player = playerNetworkIdentity.GetComponent<CharacterController>();

        if (BattleController_Network.Instance.playerOne == null)
            BattleController_Network.Instance.playerOne = player;
        else if (BattleController_Network.Instance.playerTwo == null)
            BattleController_Network.Instance.playerTwo = player;
    }

    [Command(channel = 0, requiresAuthority = false)]
    private void Cmd_RegisterPlayerOnMatch(NetworkIdentity playerNetworkIdentity)
    {
        Debug.Log("Cmd_RegisterPlayerOnMatch");
        RegisterPlayerOnMatch(playerNetworkIdentity);
    }

    #endregion
}
