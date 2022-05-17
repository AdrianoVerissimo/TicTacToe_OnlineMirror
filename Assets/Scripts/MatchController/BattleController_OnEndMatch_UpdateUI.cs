using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleController_OnEndMatch_UpdateUI : MonoBehaviour, IMatchController_OnEndMatch
{
    [SerializeField] private TextMeshProUGUI descriptionText;
    public Text textTest;

    public void OnEndMatch()
    {
        int shownID = BattleController.ActivePlayer.PlayerID + 1;
        string text = "";

        switch (BattleController.CurrentMatchStatus)
        {
            case BattleController.MatchStatus.PLAYING:
                break;
            case BattleController.MatchStatus.WON:
                text = "Player " + shownID + " is the winner!!!";
                break;
            case BattleController.MatchStatus.DRAW:
                text = "The game is a draw!";
                break;
            case BattleController.MatchStatus.GIVE_UP:
                break;
            default:
                break;
        }

        descriptionText.text = text;
    }
}
