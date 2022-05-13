using UnityEngine;
using System.Collections;

public class BoardButton_OnClick_BlockClick : MonoBehaviour, IBoardButton_OnClick
{
    [SerializeField] private BoardButton boardButton;

    public void BoardButton_OnClick()
    {
        boardButton.DisableInteraction();
    }
}
