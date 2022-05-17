using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BoardButton_OnClick_RegisterScore : MonoBehaviour, IBoardButton_OnClick
{
    [SerializeField] private Vector2 boardPosition = new Vector2(0, 0);
    [SerializeField] private Text buttonText;

    public void BoardButton_OnClick()
    {
        CharacterController player = BattleController.ActivePlayer;
        BattleController.ScorePoint(player, (int)boardPosition.x, (int)boardPosition.y);

        buttonText.text = player.PlayerID.ToString();

        BattleController.EndTurn();
    }
}
