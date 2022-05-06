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

        bool hasWon = playerOne.CheckHasWon();

        Debug.Log("Player One has won: " + hasWon);
        playerOne.DisplayGrid();

        playerOne.ResetScore();
        playerOne
            .SetScore(0, 0)
            .SetScore(0, 1)
            .SetScore(0, 2);

        hasWon = playerOne.CheckHasWon();

        Debug.Log("Player One has won: " + hasWon);
        playerOne.DisplayGrid();
    }
}
