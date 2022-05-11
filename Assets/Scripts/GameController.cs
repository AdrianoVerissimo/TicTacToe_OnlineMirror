using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] BoardController boardController;
    [SerializeField] private CharacterController playerOne;
    [SerializeField] private CharacterController playerTwo;

    private void Start()
    {
        CharacterController.GeneratePlayerID(playerOne);
        CharacterController.GeneratePlayerID(playerTwo);

        boardController
            .AddScore(playerOne, 0, 0)
            .AddScore(playerTwo, 1, 0)
            .AddScore(playerOne, 0, 1)
            .AddScore(playerTwo, 1, 1)
            .AddScore(playerOne, 0, 2);

        boardController.DisplayPlayerHasWon(playerOne);
        boardController.DisplayPlayerHasWon(playerTwo);
    }
}
