using UnityEngine;
using TMPro;

public class MatchController_OnStartTurn_UpdateUI : MonoBehaviour, IMatchController_OnStartTurn
{
    [SerializeField] private TextMeshProUGUI descriptionText;

    public void OnStartTurn()
    {
        int shownID = MatchController.ActivePlayer.PlayerID + 1;
        string text = "Player " + shownID.ToString() + " turn";

        descriptionText.text = text;
    }
}
