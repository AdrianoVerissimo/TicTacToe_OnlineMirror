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

    private IMatchController_OnStartMatch[] OnStartMatchEvents;
    private IMatchController_OnEndMatch[] OnEndMatchEvents;
    private IMatchController_OnStartTurn[] OnStartTurnEvents;
    private IMatchController_OnEndTurn[] OnEndTurnEvents;

    public override void Awake()
    {
        base.Awake();

        LoadAllEvents();

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

    public static void StartTurn()
    {
        Instance.RunEvents_StartTurn();
    }
    public static void EndTurn()
    {
        Instance.boardController.RemoveFreeSpacesCount(1);
        bool matchEnded = CheckMatchEnded();
        if (matchEnded)
            return;

        ChangeTurnPlayer();
        Instance.RunEvents_EndTurn();
        StartTurn();
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
        Instance.RunEvents_StartMatch();
        StartTurn();
    }
    public static void RestartMatch()
    {
        Instance.boardController.ResetBoard();
        SetActivePlayer(Instance.playerOne);
        StartMatch();
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

        Instance.RunEvents_EndMatch();
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

    #region Events

    private void LoadAllEvents()
    {
        LoadEvents_StartMatch();
        LoadEvents_EndMatch();
        LoadEvents_StartTurn();
        LoadEvents_EndTurn();
    }
    private void LoadEvents_StartMatch() => OnStartMatchEvents = GetComponents<IMatchController_OnStartMatch>();
    private void LoadEvents_EndMatch() => OnEndMatchEvents = GetComponents<IMatchController_OnEndMatch>();
    private void LoadEvents_StartTurn() => OnStartTurnEvents = GetComponents<IMatchController_OnStartTurn>();
    private void LoadEvents_EndTurn() => OnEndTurnEvents = GetComponents<IMatchController_OnEndTurn>();

    private void RunEvents_StartMatch()
    {
        foreach (var e in OnStartMatchEvents)
            e.OnStartMatch();
    }
    private void RunEvents_EndMatch()
    {
        foreach (var e in OnEndMatchEvents)
            e.OnEndMatch();
    }
    private void RunEvents_StartTurn()
    {
        foreach (var e in OnStartTurnEvents)
            e.OnStartTurn();
    }
    private void RunEvents_EndTurn()
    {
        foreach (var e in OnEndTurnEvents)
            e.OnEndTurn();
    }

    #endregion
}