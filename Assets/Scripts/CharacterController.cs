using UnityEngine;
using Mirror;

public class CharacterController : NetworkBehaviour
{
    public int PlayerID { get; private set; } = -1;
    private static int PlayerID_emptyValue = -1;
    [SyncVar] public string playerName;

    public static int CountPlayerID { get; private set; } = 0;

    private NetworkIdentity networkIdentity;
    public GameObject battleController_NetworkPrefab;
    public static CharacterController LocalPlayer { get; private set; }

    private void Awake()
    {
        networkIdentity = GetComponent<NetworkIdentity>();
    }

    public CharacterController SetPlayerID(int playerID)
    {
        this.PlayerID = playerID;
        return this;
    }

    public static void GeneratePlayerID(CharacterController player)
    {
        Debug.Log("GeneratePlayerID");
        int newID = GetNewPlayerID();
        player.SetPlayerID(newID);
    }
    public static void ResetPlayerID(CharacterController player)
    {
        SetCountPlayerID(PlayerID_emptyValue);
    }
    private static int GetNewPlayerID()
    {
        int newID = CountPlayerID + 1;
        SetCountPlayerID(newID);

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

    public void Network_SetPlayerName(string name)
    {
        if (isServer)
            SetPlayerName(name);
        else
            Cmd_SetPlayerName(name);
    }
    [Command]
    private void Cmd_SetPlayerName(string name)
    {
        SetPlayerName(name);
    }
    private void SetPlayerName(string name)
    {
        playerName = name;
    }

    #region Network

    public override void OnStartClient()
    {
        if (!isLocalPlayer)
            return;

        Network_SetPlayerName(PlayerPrefs.GetString(EnterNameScreen.PlayerPrefsNameKey));
        RegisterLocalPlayer(netIdentity);
        Network_CreateBattleControllerNetwork();
        BattleController_Network.Instance.Network_RegisterPlayerOnMatch(netIdentity);
        BattleController_Network.Instance.Network_ShouldStartMatch();
    }

    public void Network_CreateBattleControllerNetwork()
    {
        if (isServer)
            Cmd_CreateBattleControllerNetwork();
    }
    private void CreateBattleControllerNetwork()
    {
        if (!isServer)
            return;

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

    public void RegisterLocalPlayer(NetworkIdentity playerNetIdentity)
    {
        if (playerNetIdentity == null)
        {
            LocalPlayer = null;
            return;
        }
        LocalPlayer = playerNetIdentity.gameObject.GetComponent<CharacterController>();
    }
    public void RemoveLocalPlayer() => RegisterLocalPlayer(null);

    #endregion

    public override void OnStopLocalPlayer()
    {
        BattleController_Network.Instance.Network_DisconnectAllClients();
        BattleController.Instance.QuitMatch();
        NetworkManager_TicTacToe.ExitLobby();
        BattleController_Network.Instance.ResetPlayersIDs();
    }
}
