using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private PlayerController playerOne;
    private PlayerController playerTwo;

    private int countPlayerID = 0;

    private void Start()
    {
        playerOne = new PlayerController();
        playerOne.SetPlayerID(GeneratePlayerID());

        playerTwo = new PlayerController();
        playerTwo.SetPlayerID(GeneratePlayerID());

        playerOne
            .AddScore(0, 0)
            .AddScore(1, 0)
            .AddScore(2, 0);

        CheckPlayerHasWon(playerOne);
        playerOne.ResetScore();

        playerOne
            .AddScore(0, 0)
            .AddScore(1, 0)
            .AddScore(0, 2);

        CheckPlayerHasWon(playerOne);
        playerOne.ResetScore();

        playerTwo
            .AddScore(0, 0)
            .AddScore(1, 1)
            .AddScore(2, 2);

        CheckPlayerHasWon(playerTwo);
        playerTwo.ResetScore();

        playerTwo
            .AddScore(0, 0)
            .AddScore(1, 0)
            .AddScore(2, 2);

        CheckPlayerHasWon(playerTwo);
        playerTwo.ResetScore();
    }

    public void CheckPlayerHasWon(PlayerController player)
    {
        bool hasWon = player.CheckHasWon();

        Debug.Log("Player has won: " + hasWon);
        player.DisplayGrid();
    }

    public GameController CreatePlayerID(PlayerController player)
    {
        int newID = GeneratePlayerID();
        player.SetPlayerID(newID);

        return this;
    }

    #region PlayerID
    public int GeneratePlayerID()
    {
        int newID = countPlayerID;
        SetCountPlayerID(countPlayerID + 1);

        return newID;
    }
    public GameController ResetCountPlayerID()
    {
        SetCountPlayerID(0);
        return this;
    }
    public GameController SetCountPlayerID(int value)
    {
        countPlayerID = value;
        return this;
    }
    #endregion
}
