using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public static readonly int NoScoreValue = -1;

    private int[,] scoreArray = new int[3, 3];
    private int rowCount = 3;
    private int columnCount = 3;
    private int playerID;

    public PlayerController()
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
    public PlayerController AddScore(int rowPosition, int columnPosition) => SetScore(rowPosition, columnPosition, playerID);
    public PlayerController RemoveScore(int rowPosition, int columnPosition) => SetScore(rowPosition, columnPosition, NoScoreValue);
    private PlayerController SetScore(int rowPosition, int columnPosition, int value)
    {
        rowPosition = rowPosition >= rowCount? rowCount - 1 : rowPosition;
        columnPosition = columnPosition >= columnCount? rowCount - 1 : columnPosition;

        scoreArray[rowPosition, columnPosition] = value;

        return this;
    }
    public bool CheckHasWon()
    {
        bool hasWon = false;
        hasWon = CheckHasWon_Horizontal() || CheckHasWon_Vertical() || CheckHasWon_Diagonal();

        return hasWon;
    }

    public bool CheckHasWon_Horizontal()
    {
        return CheckHasWon_HorizontalVertical();
    }
    public bool CheckHasWon_Vertical()
    {
        return CheckHasWon_HorizontalVertical(true);
    }
    private bool CheckHasWon_HorizontalVertical(bool checkVertical = false)
    {
        bool hasWon = false;
        int lineScoreCount = 0;
        bool hasScore = false;

        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < columnCount; j++)
            {
                if (checkVertical)
                    hasScore = scoreArray[i, j] == playerID;
                else
                    hasScore = scoreArray[j, i] == playerID;

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
    public bool CheckHasWon_Diagonal()
    {
        bool hasWon = CheckHasWon_Diagonal(false) || CheckHasWon_Diagonal(true);

        return hasWon;
    }
    private bool CheckHasWon_Diagonal(bool reverse = false)
    {
        bool hasWon = false;
        int lineScoreCount = 0;
        bool hasScore = false;

        for (int i = 0; i < rowCount; i++)
        {
            if (!reverse)
                hasScore = scoreArray[i, i] == playerID;
            else
                hasScore = scoreArray[i, rowCount - 1 - i] == playerID;

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

    public PlayerController SetPlayerID(int playerID)
    {
        this.playerID = playerID;
        return this;
    }
}
