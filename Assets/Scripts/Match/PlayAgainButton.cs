using UnityEngine;
using System.Collections;

public class PlayAgainButton : MonoBehaviour
{
    public void PlayAgain()
    {
        bool isMultiplayerOnline = GameController.Instance.CurrentGameMode == GameController.GameMode.MULTIPLAYER_ONLINE;
        if (!isMultiplayerOnline)
        {
            BattleController.RestartMatch();
            return;
        }

        BattleController_Network.Instance.Network_RestartMatch();

    }
}
