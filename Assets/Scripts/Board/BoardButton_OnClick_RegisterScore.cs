using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BoardButton_OnClick_RegisterScore : MonoBehaviour, IBoardButton_OnClick
{
    [SerializeField] private Text buttonText;

    private BoardButton boardButton;

    private void Awake()
    {
        boardButton = GetComponent<BoardButton>();
    }

    public void BoardButton_OnClick()
    {
        bool isOnline = GameController.Instance.CurrentGameMode == GameController.GameMode.MULTIPLAYER_ONLINE;
        if (isOnline)
        {
            Network_RegisterScore();
            return;
        }

        RegisterScore();
    }

    public void RegisterScore()
    {
        CharacterController player = BattleController.ActivePlayer;
        BattleController.ScorePoint(player, (int)boardButton.BoardPosition.x, (int)boardButton.BoardPosition.y);

        UpdateUI(player);

        BattleController.EndTurn();
    }

    public void Network_RegisterScore()
    {
        bool isLocalPlayerTurn = BattleController_Network.IsLocalPlayerTurn();
        if (!isLocalPlayerTurn)
            return;

        BattleController_Network.Instance.Network_ScorePoint((int)boardButton.BoardPosition.x, (int)boardButton.BoardPosition.y);
        BattleController_Network.Instance.Network_EndTurn();
    }
    public void UpdateUI(CharacterController player)
    {
        buttonText.text = player.PlayerID.ToString();
    }
}
