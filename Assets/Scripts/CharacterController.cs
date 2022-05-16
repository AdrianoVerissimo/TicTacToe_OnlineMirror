using UnityEngine;
using Mirror;

public class CharacterController : NetworkBehaviour
{
    public int PlayerID { get; private set; }

    public static int CountPlayerID { get; private set; } = 0;

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
}
