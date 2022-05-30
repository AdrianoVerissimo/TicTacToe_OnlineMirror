using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleController_OnStartTurn_UpdateUI : MonoBehaviour, IMatchController_OnStartTurn
{
    [SerializeField] private Text descriptionText;

    public void OnStartTurn()
    {
        string playerName = BattleController.ActivePlayer.playerName;
        string text = playerName + " turn";

        descriptionText.text = text;
    }
}
