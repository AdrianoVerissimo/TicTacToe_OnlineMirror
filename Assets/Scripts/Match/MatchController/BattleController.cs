using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class BattleController : SingletonDestroyable<BattleController>
{
    public enum MatchStatus
    {
        PLAYING, WON, DRAW, GIVE_UP
    }
    public static MatchStatus CurrentMatchStatus;
    public static CharacterController ActivePlayer { get; private set; }

    public GameObject gameplayScreenObject;
    public BoardController BoardController { get { return boardController; } }
    [SerializeField] private BoardController boardController;
    [SerializeField] public CharacterController playerOne;
    [SerializeField] public CharacterController playerTwo;

    [SerializeField] private Button restartMatchButton;

    private IMatchController_OnStartMatch[] OnStartMatchEvents;
    private IMatchController_OnEndMatch[] OnEndMatchEvents;
    private IMatchController_OnStartTurn[] OnStartTurnEvents;
    private IMatchController_OnEndTurn[] OnEndTurnEvents;

    private bool allPlayersConnected = false;

    public override void Awake()
    {
        base.Awake();

        LoadAllEvents();

        if (allPlayersConnected)
            StartMatch();
    }

    private void Start()
    {
        DisableGameplay();
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

    public static void SetCurrentMatchStatus(MatchStatus matchStatus)
    {
        CurrentMatchStatus = matchStatus;
    }

    public static void StartMatch()
    {
        GeneratePlayersIDs();
        ResetMatchData();
        SetupMatch();
        Instance.EnableGameplay();
        Instance.RunEvents_StartMatch();
        StartTurn();
    }
    public static void SetupMatch()
    {
        SetActivePlayer(Instance.playerOne);
        CurrentMatchStatus = MatchStatus.PLAYING;
    }
    public static void ResetMatchData()
    {
        Instance.boardController.ResetBoard();
        SetActivePlayer(Instance.playerOne);
        Instance.boardController.EnableAllButtons();
    }
    public static void RestartMatch()
    {
        StartMatch();
    }

    public static void EndMatch()
    {
        Instance.boardController.DisableAllButtons();
        Instance.RunEvents_EndMatch();
    }

    public static MatchStatus GetUpdatedMatchStatus()
    {
        bool hasWinner = Instance.boardController.HasPlayerWon(ActivePlayer);
        bool isDraw = !hasWinner && Instance.boardController.FreeSpacesCount == 0;
        bool hasEnded = hasWinner || isDraw;
        MatchStatus matchStatus = MatchStatus.PLAYING;

        if (hasWinner)
            matchStatus = MatchStatus.WON;
        else if (isDraw)
            matchStatus = MatchStatus.DRAW;

        return matchStatus;
    }

    public static void StartTurn()
    {
        Instance.RunEvents_StartTurn();
    }
    public static void EndTurn()
    {
        Instance.boardController.RemoveFreeSpacesCount(1);
        CurrentMatchStatus = GetUpdatedMatchStatus();
        bool hasEndedMatch = CurrentMatchStatus == MatchStatus.WON || CurrentMatchStatus == MatchStatus.DRAW;

        if (hasEndedMatch)
        {
            EndMatch();
            return;
        }

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

    public void EnableRestartMatchButton(bool enable = true) => restartMatchButton.gameObject.SetActive(enable);
    public void DisableRestartMatchButton() => EnableRestartMatchButton(false);

    public void EnableGameplay(bool enable = true)
    {
        boardController.EnableAllButtons(enable);
        EnableRestartMatchButton(enable);
    }
    public void DisableGameplay() => EnableGameplay(false);

    public void QuitMatch()
    {
        GameController.ShowLobbyScreen();
        LobbyController.ShowEnterNameScreen();
    }

    #endregion

    #region Players

    public static void GeneratePlayersIDs()
    {
        CharacterController.GeneratePlayerID(Instance.playerOne);
        CharacterController.GeneratePlayerID(Instance.playerTwo);
    }

    #endregion

    public static void ShowGameplayScreen(bool show = true)
    {
        Instance.gameplayScreenObject.SetActive(show);
    }
    public static void HideGameplayScreen() => ShowGameplayScreen(false);

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

    public void RunEvents_StartMatch()
    {
        foreach (var e in OnStartMatchEvents)
            e.OnStartMatch();
    }
    public void RunEvents_EndMatch()
    {
        foreach (var e in OnEndMatchEvents)
            e.OnEndMatch();
    }
    public void RunEvents_StartTurn()
    {
        foreach (var e in OnStartTurnEvents)
            e.OnStartTurn();
    }
    public void RunEvents_EndTurn()
    {
        foreach (var e in OnEndTurnEvents)
            e.OnEndTurn();
    }

    #endregion
}