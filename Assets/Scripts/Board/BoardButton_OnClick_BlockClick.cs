using UnityEngine;
using System.Collections;

public class BoardButton_OnClick_BlockClick : MonoBehaviour, IBoardButton_OnClick
{
    [SerializeField] private BoardButton boardButton;

    public void BoardButton_OnClick()
    {
        bool isOnline = GameController.Instance.CurrentGameMode == GameController.GameMode.MULTIPLAYER_ONLINE;
        if (isOnline)
        {
            bool isLocalPlayerTurn = BattleController_Network.IsLocalPlayerTurn();
            if (!isLocalPlayerTurn)
                return;
        }

        //boardButton.DisableInteraction();
    }
}
