using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BoardButton_OnClick_RegisterScore : MonoBehaviour, IBoardButton_OnClick
{
    [SerializeField] private Vector2 boardPosition = new Vector2(0, 0);
    [SerializeField] private Text buttonText;

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
        BattleController.ScorePoint(player, (int)boardPosition.x, (int)boardPosition.y);

        buttonText.text = player.PlayerID.ToString();

        BattleController.EndTurn();
    }

    public void Network_RegisterScore()
    {
        BattleController_Network.Instance.Network_ScorePoint((int)boardPosition.x, (int)boardPosition.y);
    }

    /**
     * botão clicado
     * mandar informação de que foi clicado para BattleController_Network
     * BattleController_Network receber o botão clicado e registrar score pelo lado servidor
     * BattleController_Network atualizar score e visualização do botão para todos os clientes
     * */
}
