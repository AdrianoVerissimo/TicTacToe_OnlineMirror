using UnityEngine;
using Mirror;
using System;

public class BattleController_Network : NetworkBehaviour
{
    private BattleController battleController;
    private NetworkManager networkManager;

    [SyncVar] public NetworkIdentity playerOneNetworkIdentity;
    [SyncVar] public NetworkIdentity playerTwoNetworkIdentity;
    [SyncVar] public BattleController.MatchStatus CurrentMatchStatus;

    public static BattleController_Network Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        battleController = GetComponent<BattleController>();
        networkManager = NetworkManager.singleton;
    }

    public void Network_RegisterPlayerOnMatch(NetworkIdentity playerNetworkIdentity)
    {
        if (isServer)
            RegisterPlayerOnMatch(playerNetworkIdentity);
        else
            Cmd_RegisterPlayerOnMatch(playerNetworkIdentity);
    }
    private void RegisterPlayerOnMatch(NetworkIdentity playerNetworkIdentity)
    {
        if (playerOneNetworkIdentity == null)
            playerOneNetworkIdentity = playerNetworkIdentity;
        else if (playerTwoNetworkIdentity == null)
            playerTwoNetworkIdentity = playerNetworkIdentity;
    }

    [Command(channel = 0, requiresAuthority = false)]
    private void Cmd_RegisterPlayerOnMatch(NetworkIdentity playerNetworkIdentity) => RegisterPlayerOnMatch(playerNetworkIdentity);

    public void ShowPlayers()
    {
        if (playerOneNetworkIdentity != null)
        {
            Debug.Log("Player one: " + playerOneNetworkIdentity.GetComponent<NetworkConnection>().connectionId);
        }

        if (playerTwoNetworkIdentity != null)
        {
            Debug.Log("Player two: " + playerTwoNetworkIdentity.GetComponent<NetworkConnection>().connectionId);
        }
    }

    public void RemovePlayers() => playerOneNetworkIdentity = playerTwoNetworkIdentity = null;

    public bool CheckAllPlayersConnected()
    {
        bool allPlayersConnected = playerOneNetworkIdentity != null && playerTwoNetworkIdentity != null;
        return allPlayersConnected;
    }
    public void Network_ShouldStartMatch()
    {
        if (isServer)
            ShouldStartMatch();
        else
            Cmd_ShouldStartMatch();
    }
    private void ShouldStartMatch()
    {
        if (!isServer)
        {
            Debug.Log("Error: can only ask to start a match by the server.");
            return;
        }

        bool allPlayersConnected = CheckAllPlayersConnected();
        bool canStartMatch = allPlayersConnected;

        if (!canStartMatch)
            return;

        StartMatch();
    }
    [Command(channel = 0, requiresAuthority = false)]
    private void Cmd_ShouldStartMatch()
    {
        ShouldStartMatch();
    }

    private void StartMatch()
    {
        CharacterController playerOne = playerOneNetworkIdentity.gameObject.GetComponent<CharacterController>();
        CharacterController playerTwo = playerTwoNetworkIdentity.gameObject.GetComponent<CharacterController>();

        CharacterController.GeneratePlayerID(playerOne);
        CharacterController.GeneratePlayerID(playerTwo);
        BattleController.SetupMatch();

        Rpc_StartMatch(playerOne.PlayerID, playerTwo.PlayerID, playerOneNetworkIdentity, playerTwoNetworkIdentity);
    }

    [ClientRpc]
    private void Rpc_StartMatch(int playerOneID, int playerTwoID, NetworkIdentity playerOneNetIdentity, NetworkIdentity playerTwoNetIdentity)
    {
        GameController.ShowGameplayScreen();

        BattleController.Instance.playerOne = playerOneNetIdentity.gameObject.GetComponent<CharacterController>();
        BattleController.Instance.playerTwo = playerTwoNetIdentity.gameObject.GetComponent<CharacterController>();

        BattleController.Instance.playerOne.SetPlayerID(playerOneID);
        BattleController.Instance.playerTwo.SetPlayerID(playerTwoID);

        BattleController.SetActivePlayer(BattleController.Instance.playerOne);
        BattleController.Instance.EnableGameplay();
        BattleController.Instance.RunEvents_StartMatch();

        ShouldEnableButtonsForLocalPlayer();

        BattleController.StartTurn();
    }

    public void Network_RestartMatch()
    {
        if (isServer)
        {
            Rpc_RestartMatch();
        }
        else
        {
            Cmd_RestartMatch();
        }
    }

    [ClientRpc]
    public void Rpc_RestartMatch()
    {
        BattleController.RestartMatch();
    }
    [Command(channel = 0, requiresAuthority = false)]
    public void Cmd_RestartMatch()
    {
        Rpc_RestartMatch();
    }

    public void Network_EndTurn()
    {
        if (isServer)
            EndTurn();
        else
            Cmd_EndTurn();
    }

    private void EndTurn()
    {
        int removeFreeSpacesNumber = 1;
        BattleController.Instance.BoardController.RemoveFreeSpacesCount(removeFreeSpacesNumber);
        int freeSpacesNumber = BattleController.Instance.BoardController.FreeSpacesCount;

        CurrentMatchStatus = BattleController.GetUpdatedMatchStatus();
        bool hasEndedMatch = CurrentMatchStatus == BattleController.MatchStatus.WON || CurrentMatchStatus == BattleController.MatchStatus.DRAW;
        if (hasEndedMatch)
        {
            EndMatch(CurrentMatchStatus);
            return;
        }

        BattleController.ChangeTurnPlayer();
        CharacterController activePlayer = BattleController.ActivePlayer;
        
        Rpc_EndTurn(freeSpacesNumber, CurrentMatchStatus, activePlayer.netIdentity);
    }
    [Command(channel = 0, requiresAuthority = false)]
    private void Cmd_EndTurn() => EndTurn();

    [ClientRpc]
    private void Rpc_EndTurn(int freeSpacesNumber, BattleController.MatchStatus matchStatus, NetworkIdentity activePlayerNetworkIdentity)
    {
        CharacterController activePlayer = activePlayerNetworkIdentity.gameObject.GetComponent<CharacterController>();

        BattleController.Instance.BoardController.SetFreeSpacesCount(freeSpacesNumber);
        BattleController.SetActivePlayer(activePlayer);
        BattleController.SetCurrentMatchStatus(matchStatus);

        BattleController.Instance.RunEvents_EndTurn();

        ShouldEnableButtonsForLocalPlayer();

        BattleController.StartTurn();
    }

    public void Network_TryScorePoint(int positionX, int positionY)
    {
        if (isServer)
            TryScorePoint(positionX, positionY);
        else
            Cmd_TryScorePoint(positionX, positionY);
    }
    [Command]
    private void Cmd_TryScorePoint(int positionX, int positionY)
    {
        TryScorePoint(positionX, positionY);
    }
    private void TryScorePoint(int positionX, int positionY)
    {
        Network_ScorePoint(positionX, positionY);
    }
    public void Network_ScorePoint(int positionX, int positionY)
    {
        if (isServer)
            ScorePoint(positionX, positionY);
        else
            Cmd_ScorePoint(positionX, positionY);
    }
    private void ScorePoint(int positionX, int positionY)
    {
        bool canScore = true;
        if (!canScore)
            return;

        CharacterController activePlayer = BattleController.ActivePlayer;
        BoardController boardController = BattleController.Instance.BoardController;

        BattleController.ScorePoint(activePlayer, positionX, positionY);
        Rpc_UpdateClickedButtonUI(activePlayer.netIdentity, positionX, positionY);
    }

    [Command(channel = 0, requiresAuthority = false)]
    private void Cmd_ScorePoint(int positionX, int positionY) => ScorePoint(positionX, positionY);

    public static bool IsLocalPlayerTurn()
    {
        return CharacterController.LocalPlayer.PlayerID == BattleController.ActivePlayer.PlayerID;
    }

    [ClientRpc]
    private void Rpc_UpdateClickedButtonUI(NetworkIdentity activePlayerNetIdentity, int positionX, int positionY)
    {
        CharacterController activePlayer = activePlayerNetIdentity.gameObject.GetComponent<CharacterController>();
        BoardController boardController = BattleController.Instance.BoardController;
        BoardButton clickedButton = boardController.GetButtonByCoordinates(positionX, positionY);
        BoardButton_OnClick_RegisterScore clickedButton_RegisterScore = clickedButton.gameObject.GetComponent<BoardButton_OnClick_RegisterScore>();

        clickedButton_RegisterScore.UpdateUI(activePlayer);
        clickedButton.DisableInteraction();
    }

    private void EndMatch(BattleController.MatchStatus matchStatus) => Rpc_EndMatch(matchStatus);

    [ClientRpc]
    private void Rpc_EndMatch(BattleController.MatchStatus matchStatus)
    {
        BattleController.CurrentMatchStatus = matchStatus;
        BattleController.EndMatch();
    }

    public void ShouldEnableButtonsForLocalPlayer()
    {
        bool isLocalPlayerTurn = IsLocalPlayerTurn();
        if (isLocalPlayerTurn)
            BattleController.Instance.BoardController.EnableAvailableButtons();
        else
            BattleController.Instance.BoardController.DisableAvailableButtons();
    }
}
