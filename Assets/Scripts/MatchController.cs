using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchController : SingletonDestroyable<MatchController>
{
    [SerializeField] private BoardController boardController;
    [SerializeField] private CharacterController playerOne;
    [SerializeField] private CharacterController playerTwo;

    public static CharacterController ActivePlayer { get; private set; }

    public override void Awake()
    {
        base.Awake();

        CharacterController.GeneratePlayerID(playerOne);
        CharacterController.GeneratePlayerID(playerTwo);

        SetActivePlayer(playerOne);
    }

    public static void ScorePoint(CharacterController player, int rowPosition, int columnPosition)
    {
        Instance.boardController.AddScore(player, rowPosition, columnPosition);
    }

    public static void DisplayGrid()
    {
        Instance.boardController.DisplayGrid();
    }

    public static void SetActivePlayer(CharacterController player)
    {
        ActivePlayer = player;
    }

    public static void BeginTurn()
    {

    }
    public static void EndTurn()
    {
        bool hasWinner = HasWinner();
        if (hasWinner)
        {
            EndMatch();
            return;
        }

        ChangeTurnPlayer();
        BeginTurn();
    }
    public static void ChangeTurnPlayer()
    {
        bool playerOneActive = ActivePlayer.PlayerID == Instance.playerOne.PlayerID;
        if (playerOneActive)
            SetActivePlayer(Instance.playerTwo);
        else
            SetActivePlayer(Instance.playerOne);
    }
    public static bool HasWinner()
    {
        return false;
    }
    public static void StartMatch()
    {
        BeginTurn();
    }
    public static void RestartMatch()
    {

    }
    public static void EndMatch()
    { 

    }
}