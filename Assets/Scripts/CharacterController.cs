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

        GameObject battleController_NetworkInstance = Instantiate(battleController_NetworkPrefab);
        if (BattleController_Network.Instance == null)
        {
            NetworkServer.Spawn(battleController_NetworkInstance);
        }
    }

    

    #endregion
}
