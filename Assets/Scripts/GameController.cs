using UnityEngine;
using System.Collections;

public class GameController : Singleton<GameController>
{
    public enum GameMode
    {
        SINGLE_PLAYER, MULTIPLAYER, MULTIPLAYER_ONLINE
    }
    public GameMode CurrentGameMode { get; private set; }

    public override void Awake()
    {
        base.Awake();
        SetCurrentGameMode(GameMode.MULTIPLAYER_ONLINE);
    }

    public void SetCurrentGameMode(GameMode gameMode)
    {
        CurrentGameMode = gameMode;
    }
}
