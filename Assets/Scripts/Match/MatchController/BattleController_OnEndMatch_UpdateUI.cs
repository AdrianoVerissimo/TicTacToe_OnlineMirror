using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleController_OnEndMatch_UpdateUI : MonoBehaviour, IMatchController_OnEndMatch
{
    [SerializeField] private Text descriptionText;
    public Text textTest;

    public void OnEndMatch()
    {
        uint activePlayerNetId = BattleController.ActivePlayer.netId;
        uint localPlayerNetId = CharacterController.LocalPlayer.netId;

        bool localPlayerWon = activePlayerNetId == localPlayerNetId;
        string text = "";
        string winText = "";

        if (localPlayerWon)
            winText = "You win!!!";
        else
            winText = "You lose...";

        switch (BattleController.CurrentMatchStatus)
        {
            case BattleController.MatchStatus.PLAYING:
                break;
            case BattleController.MatchStatus.WON:
                text = winText;
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
