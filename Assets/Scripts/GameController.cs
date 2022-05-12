using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    [SerializeField] private BoardController boardController;
    [SerializeField] private CharacterController playerOne;
    [SerializeField] private CharacterController playerTwo;

    public override void Awake()
    {
        base.Awake();

        CharacterController.GeneratePlayerID(playerOne);
        CharacterController.GeneratePlayerID(playerTwo);
    }

    public static void ScorePoint(CharacterController player, int rowPosition, int columnPosition)
    {
        Instance.boardController.AddScore(player, rowPosition, columnPosition);
    }

    public static CharacterController GetCurrentPlayer()
    {
        return Instance.playerOne;
    }

    public static void DisplayGrid()
    {
        Instance.boardController.DisplayGrid();
    }
}
