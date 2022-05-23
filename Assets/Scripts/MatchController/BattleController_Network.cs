using UnityEngine;
using Mirror;

public class BattleController_Network : NetworkBehaviour
{
    private BattleController battleController;
    private NetworkManager networkManager;

    [SyncVar] public CharacterController playerOne;
    [SyncVar] public CharacterController playerTwo;

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
        CharacterController player = playerNetworkIdentity.GetComponent<CharacterController>();

        if (playerOne == null)
            playerOne = player;
        else if (playerTwo == null)
            playerTwo = player;
    }

    [Command(channel = 0, requiresAuthority = false)]
    private void Cmd_RegisterPlayerOnMatch(NetworkIdentity playerNetworkIdentity) => RegisterPlayerOnMatch(playerNetworkIdentity);

    public void ShowPlayers()
    {
        if (playerOne != null)
        {
            Debug.Log("Player one: " + playerOne.GetComponent<NetworkConnection>().connectionId);
        }

        if (playerTwo != null)
        {
            Debug.Log("Player two: " + playerTwo.GetComponent<NetworkConnection>().connectionId);
        }
    }

    public void RemovePlayers() => playerOne = playerTwo = null;

    public bool CheckAllPlayersConnected()
    {
        bool allPlayersConnected = playerOne != null && playerTwo != null;
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
        CharacterController.GeneratePlayerID(playerOne);
        CharacterController.GeneratePlayerID(playerTwo);
        BattleController.SetupMatch();

        Rpc_StartMatch(playerOne.netIdentity, playerTwo.netIdentity);
    }

    [ClientRpc]
    private void Rpc_StartMatch(NetworkIdentity playerOne, NetworkIdentity playerTwo)
    {
        BattleController.Instance.playerOne = playerOne.gameObject.GetComponent<CharacterController>();
        BattleController.Instance.playerTwo = playerTwo.gameObject.GetComponent<CharacterController>();
        BattleController.SetActivePlayer(BattleController.Instance.playerOne);
        BattleController.Instance.EnableGameplay();
        BattleController.Instance.RunEvents_StartMatch();
        BattleController.StartTurn();
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

        BattleController.MatchStatus matchStatus = BattleController.GetUpdatedMatchStatus();
        bool hasEndedMatch = matchStatus == BattleController.MatchStatus.WON || matchStatus == BattleController.MatchStatus.DRAW;
        if (hasEndedMatch)
        {

            return;
        }

        BattleController.ChangeTurnPlayer();
        CharacterController activePlayer = BattleController.ActivePlayer;
        
        Rpc_EndTurn(removeFreeSpacesNumber, matchStatus, activePlayer.netIdentity);
    }
    [Command(channel = 0, requiresAuthority = false)]
    private void Cmd_EndTurn() => EndTurn();

    [ClientRpc]
    private void Rpc_EndTurn(int removeFreeSpacesNumber, BattleController.MatchStatus matchStatus, NetworkIdentity activePlayerNetworkIdentity)
    {
        CharacterController activePlayer = activePlayerNetworkIdentity.gameObject.GetComponent<CharacterController>();

        BattleController.Instance.BoardController.RemoveFreeSpacesCount(removeFreeSpacesNumber);
        BattleController.SetActivePlayer(activePlayer);
        BattleController.Instance.testActivePlayer = activePlayer;
        BattleController.SetCurrentMatchStatus(matchStatus);
        BattleController.Instance.RunEvents_EndTurn();
        BattleController.StartTurn();
    }

    public void Network_ScorePoint(int positionX, int positionY)
    {
        if (isServer)
            TryScorePoint(positionX, positionY);
        else
            Cmd_TryScorePoint(positionX, positionY);
    }
    private void TryScorePoint(int positionX, int positionY)
    {
        bool canScore = true;
        if (!canScore)
            return;

        CharacterController activePlayer = BattleController.ActivePlayer;
        Rpc_ScorePoint(activePlayer.netIdentity, positionX, positionY);
    }

    [Command(channel = 0, requiresAuthority = false)]
    private void Cmd_TryScorePoint(int positionX, int positionY)
    {
        TryScorePoint(positionX, positionY);
    }

    [ClientRpc]
    private void Rpc_ScorePoint(NetworkIdentity playerNet, int positionX, int positionY)
    {
        BoardController boardController = BattleController.Instance.BoardController;
        boardController.Debug_DisplayGrid();
        CharacterController player = playerNet.GetComponent<CharacterController>();
        BattleController.ScorePoint(player, positionX, positionY);
        boardController.Debug_DisplayGrid();

        BoardButton clickedButton = boardController.GetButtonByCoordinates(positionX, positionY);
        clickedButton.GetComponent<BoardButton_OnClick_RegisterScore>().UpdateUI(player);
    }
}
