using UnityEngine;
using System.Collections;

public class BoardController : MonoBehaviour
{
    public static readonly int NoScoreValue = -1;

    private int[,] scoreArray = new int[3, 3];
    private int rowCount = 3;
    private int columnCount = 3;

    private void Start()
    {
        ResetScore();
    }

    public void ResetScore()
    {
        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < columnCount; j++)
            {
                RemoveScore(i, j);
            }
        }
    }
    public BoardController AddScore(CharacterController player, int rowPosition, int columnPosition) => SetScore(rowPosition, columnPosition, player.PlayerID);
    public BoardController RemoveScore(int rowPosition, int columnPosition) => SetScore(rowPosition, columnPosition, NoScoreValue);
    private BoardController SetScore(int rowPosition, int columnPosition, int value)
    {
        rowPosition = rowPosition >= rowCount ? rowCount - 1 : rowPosition;
        columnPosition = columnPosition >= columnCount ? rowCount - 1 : columnPosition;

        scoreArray[rowPosition, columnPosition] = value;

        return this;
    }
    public bool CheckHasWon(CharacterController player)
    {
        bool hasWon = false;
        hasWon = CheckHasWon_Horizontal(player) || CheckHasWon_Vertical(player) || CheckHasWon_Diagonal(player);

        return hasWon;
    }

    public bool CheckHasWon_Horizontal(CharacterController player)
    {
        return CheckHasWon_HorizontalVertical(player);
    }
    public bool CheckHasWon_Vertical(CharacterController player)
    {
        return CheckHasWon_HorizontalVertical(player, true);
    }
    private bool CheckHasWon_HorizontalVertical(CharacterController player, bool checkVertical = false)
    {
        bool hasWon = false;
        int lineScoreCount = 0;
        bool hasScore = false;

        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < columnCount; j++)
            {
                if (checkVertical)
                    hasScore = scoreArray[i, j] == player.PlayerID;
                else
                    hasScore = scoreArray[j, i] == player.PlayerID;

                if (hasScore)
                    lineScoreCount++;

                hasWon = lineScoreCount >= columnCount;

                if (hasWon)
                    break;
            }

            if (hasWon)
                break;

            lineScoreCount = 0;
            hasScore = false;
        }

        return hasWon;
    }
    public bool CheckHasWon_Diagonal(CharacterController player)
    {
        bool hasWon = CheckHasWon_Diagonal(player, false) || CheckHasWon_Diagonal(player, true);

        return hasWon;
    }
    private bool CheckHasWon_Diagonal(CharacterController player, bool reverse = false)
    {
        bool hasWon = false;
        int lineScoreCount = 0;
        bool hasScore = false;

        for (int i = 0; i < rowCount; i++)
        {
            if (!reverse)
                hasScore = scoreArray[i, i] == player.PlayerID;
            else
                hasScore = scoreArray[i, rowCount - 1 - i] == player.PlayerID;

            if (hasScore)
                lineScoreCount++;

            hasWon = lineScoreCount >= rowCount;

            if (hasWon)
                break;

            hasScore = false;
        }

        return hasWon;
    }

    public void DisplayGrid()
    {
        string text = "";
        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < columnCount; j++)
            {
                text += scoreArray[i, j].ToString() + "|";
            }
            text += "\n";
        }
        Debug.Log(text);
    }

    public void CheckPlayerHasWon(CharacterController player)
    {
        bool hasWon = CheckHasWon(player);

        Debug.Log("Player has won: " + hasWon);
        DisplayGrid();
    }
}
