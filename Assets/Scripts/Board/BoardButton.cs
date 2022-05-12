using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class BoardButton : MonoBehaviour, IPointerClickHandler
{
    private IBoardButton_OnClick[] clickEvents;

    private void Awake()
    {
        LoadClickEvents();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        RunClickEvents();
    }

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
}
