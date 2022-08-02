using UnityEngine;
using System.Collections;

public class QuitMatchButton : MonoBehaviour
{
    public void QuitMatch()
    {
        bool isMultiplayerOnline = GameController.Instance.CurrentGameMode == GameController.GameMode.MULTIPLAYER_ONLINE;
        if (!isMultiplayerOnline)
        {
            Offline_QuitMatch();
            return;
        }

        Online_QuitMatch();
    }
    private void Online_QuitMatch()
    {
        BattleController_Network.Instance.Network_QuitMatch();
    }
    private void Offline_QuitMatch()
    {
        BattleController.Instance.QuitMatch();
    }
}
