using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private bool[,] scoreArray = new bool[3, 3];
    private int rowCount = 3;
    private int columnCount = 3;

    public void ResetScore()
    {
        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < columnCount; j++)
            {
                SetScore(i, j, false);
            }
        }
    }
    public PlayerController SetScore(int rowPosition, int columnPosition, bool valueToSet = true)
    {
        rowPosition = rowPosition >= rowCount? rowCount - 1 : rowPosition;
        columnPosition = columnPosition >= columnCount? rowCount - 1 : columnPosition;

        scoreArray[rowPosition, columnPosition] = valueToSet;

        return this;
    }
    public bool CheckHasWon()
    {
        bool hasWon = false;
        int lineScoreCount = 0;
        bool hasScore = false;

        //check row
        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < columnCount; j++)
            {
                hasScore = scoreArray[i, j] == true;
                if (hasScore)
                    lineScoreCount++;

                if (lineScoreCount >= columnCount)
                {
                    hasWon = true;
                    break;
                }
            }

            lineScoreCount = 0;
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
}
