using UnityEngine;
using System.Collections;

public class LobbyController : MonoBehaviour
{
    [SerializeField] private LobbyScreen enterNameScreen; 
    [SerializeField] private LobbyScreen connectionScreen; 

    public void ShowEnterNameScreen()
    {
        enterNameScreen.gameObject.SetActive(true);
        connectionScreen.gameObject.SetActive(false);
    }
    public void ShowConnectionScreen()
    {
        connectionScreen.gameObject.SetActive(true);
        enterNameScreen.gameObject.SetActive(false);
    }
}
