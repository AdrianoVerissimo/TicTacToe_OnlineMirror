using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private PlayerController playerOne;
    private PlayerController playerTwo;

    private void Start()
    {
        playerOne = new PlayerController();
        playerOne
            .SetScore(0, 0)
            .SetScore(0, 1)
            .SetScore(1, 0);

        CheckPlayerHasWon(playerOne);

        playerOne.ResetScore();
        playerOne
            .SetScore(0, 0)
            .SetScore(0, 1)
            .SetScore(0, 2);

        CheckPlayerHasWon(playerOne);

        playerOne.ResetScore();
        playerOne
            .SetScore(0, 0)
            .SetScore(1, 0)
            .SetScore(1, 1)
            .SetScore(2, 0);

        CheckPlayerHasWon(playerOne);
        playerOne.ResetScore();

        playerOne
            .SetScore(0, 0)
            .SetScore(1, 1)
            .SetScore(1, 2)
            .SetScore(2, 2);

        CheckPlayerHasWon(playerOne);
        playerOne.ResetScore();

        playerOne
            .SetScore(0, 2)
            .SetScore(1, 1)
            .SetScore(2, 0)
            .SetScore(2, 2);

        CheckPlayerHasWon(playerOne);
        playerOne.ResetScore();

        playerOne
            .SetScore(0, 2)
            .SetScore(1, 1)
            .SetScore(2, 2);

        CheckPlayerHasWon(playerOne);
        playerOne.ResetScore();
    }

    public void CheckPlayerHasWon(PlayerController player)
    {
        bool hasWon = player.CheckHasWon();

        Debug.Log("Player has won: " + hasWon);
        player.DisplayGrid();
    }
}
