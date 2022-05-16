using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MatchController_OnEndMatch_UpdateUI : MonoBehaviour, IMatchController_OnEndMatch
{
    [SerializeField] private TextMeshProUGUI descriptionText;
    public Text textTest;

    public void OnEndMatch()
    {
        int shownID = MatchController.ActivePlayer.PlayerID + 1;
        string text = "";

        switch (MatchController.CurrentMatchStatus)
        {
            case MatchController.MatchStatus.PLAYING:
                break;
            case MatchController.MatchStatus.WON:
                text = "Player " + shownID + " is the winner!!!";
                break;
            case MatchController.MatchStatus.DRAW:
                text = "The game is a draw!";
                break;
            case MatchController.MatchStatus.GIVE_UP:
                break;
            default:
                break;
        }

        descriptionText.text = text;
    }
}
