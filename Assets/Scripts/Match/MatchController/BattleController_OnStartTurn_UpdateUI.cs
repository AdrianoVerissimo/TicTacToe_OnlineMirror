using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleController_OnStartTurn_UpdateUI : MonoBehaviour, IMatchController_OnStartTurn
{
    [SerializeField] private Text descriptionText;

    public void OnStartTurn()
    {
        bool isLocalPlayerTurn = BattleController.ActivePlayer.netId == CharacterController.LocalPlayer.netId;
        string text = "";
        if (isLocalPlayerTurn)
            text = "Your turn.";
        else
            text =  "Waiting for opponent's move...";

        descriptionText.text = text;
    }
}
