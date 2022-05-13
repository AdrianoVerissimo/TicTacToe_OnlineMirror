using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchController : SingletonDestroyable<MatchController>
{
    public enum MatchStatus
    {
        PLAYING, WON, DRAW, GIVE_UP
    }
    public static MatchStatus CurrentMatchStatus;

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

        StartMatch();
    }

    #region Score

    public static void ScorePoint(CharacterController player, int rowPosition, int columnPosition)
    {
        Instance.boardController.AddScore(player, rowPosition, columnPosition);
    }
    
    #endregion

    #region Player

    public static void SetActivePlayer(CharacterController player)
    {
        ActivePlayer = player;
    }

    #endregion

    #region Gameplay

    public static void BeginTurn()
    {
        Debug.Log("Player " + ActivePlayer.PlayerID + " turn.");
    }
    public static void EndTurn()
    {
        Instance.boardController.RemoveFreeSpacesCount(1);
        Debug.Log("FreeSpacesCount: " + Instance.boardController.FreeSpacesCount);
        bool matchEnded = CheckMatchEnded();
        if (matchEnded)
            return;

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
    public static void StartMatch()
    {
        CurrentMatchStatus = MatchStatus.PLAYING;
        BeginTurn();
    }
    public static void RestartMatch()
    {

    }
    public static void EndMatch()
    {
        string endText = "";
        switch (CurrentMatchStatus)
        {
            case MatchStatus.WON:
                endText = "Player " + ActivePlayer.PlayerID + " is the winner!!!";
                break;
            case MatchStatus.DRAW:
                endText = "Draw game!";
                break;
            case MatchStatus.GIVE_UP:
                break;
            default:
                break;
        }

        Debug.Log(endText);
    }

    public static bool CheckMatchEnded()
    {
        bool hasWinner = Instance.boardController.HasPlayerWon(ActivePlayer);
        bool isDraw = !hasWinner && Instance.boardController.FreeSpacesCount == 0;

        if (hasWinner || isDraw)
        {
            if (hasWinner)
                CurrentMatchStatus = MatchStatus.WON;
            else if (isDraw)
                CurrentMatchStatus = MatchStatus.DRAW;

            EndMatch();
        }

        return hasWinner;
    }

    #endregion
}