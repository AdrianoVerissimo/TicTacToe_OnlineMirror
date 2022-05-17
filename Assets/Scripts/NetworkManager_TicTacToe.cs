using UnityEngine;
using Mirror;

public class NetworkManager_TicTacToe : NetworkManager
{
    private BattleController_Network matchController_Network;

    public override void Start()
    {
        base.Start();

        matchController_Network = GetComponent<BattleController_Network>();
    }
}
