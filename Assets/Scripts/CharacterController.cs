using UnityEngine;
using Mirror;

public class CharacterController : NetworkBehaviour
{
    public int PlayerID { get; private set; }

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
        int newID = GetNewPlayerID();
        player.SetPlayerID(newID);
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
}
