﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class BoardButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Text buttonText;

    private IBoardButton_OnClick[] clickEvents;
    private string starterButtonText;

    private void Awake()
    {
        InitButtonText();
        LoadClickEvents();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        RunClickEvents();
    }

    #region Events

    private void LoadClickEvents()
    {
        clickEvents = GetComponents<IBoardButton_OnClick>();
    }
    private void RunClickEvents()
    {
        if (clickEvents.Length == 0)
            return;

        foreach (var e in clickEvents)
        {
            e.BoardButton_OnClick();
        }
    }

    #endregion

    #region Text

    private void InitButtonText()
    {
        starterButtonText = buttonText.text;
    }
    public void SetText(string text)
    {
        buttonText.text = text;
    }
    public void ResetText()
    {
        SetText(starterButtonText);
    }

    #endregion
}
