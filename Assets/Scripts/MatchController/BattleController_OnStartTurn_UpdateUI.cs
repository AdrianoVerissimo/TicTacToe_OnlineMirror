using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleController_OnStartTurn_UpdateUI : MonoBehaviour, IMatchController_OnStartTurn
{
    [SerializeField] private Text descriptionText;

    public void OnStartTurn()
    {
        int shownID = BattleController.ActivePlayer.PlayerID + 1;
        string text = "Player " + shownID.ToString() + " turn";

        descriptionText.text = text;
    }
}
