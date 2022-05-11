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
        Debug.Log("Start");
        CharacterController.CreatePlayerID(playerOne);
        CharacterController.CreatePlayerID(playerTwo);

        boardController
            .AddScore(playerOne, 0, 0)
            .AddScore(playerTwo, 1, 0)
            .AddScore(playerOne, 0, 1)
            .AddScore(playerTwo, 1, 1)
            .AddScore(playerOne, 0, 2);

        boardController.CheckPlayerHasWon(playerOne);
        boardController.CheckPlayerHasWon(playerTwo);
    }
}
