using UnityEngine;
using UnityEngine.UI;

public class LobbyController : SingletonDestroyable<LobbyController>
{
    [Header("Screens")]
    [SerializeField] private GameObject lobbyScreenObject;
    [SerializeField] private LobbyScreen enterNameScreen; 
    [SerializeField] private LobbyScreen connectionScreen;

    

    public static void ShowEnterNameScreen()
    {
        Instance.enterNameScreen.gameObject.SetActive(true);
        Instance.connectionScreen.gameObject.SetActive(false);
    }
    public static void ShowConnectionScreen()
    {
        Instance.connectionScreen.gameObject.SetActive(true);
        Instance.enterNameScreen.gameObject.SetActive(false);
    }
    public static void ShowLobbyScreen(bool show = true)
    {
        Instance.lobbyScreenObject.SetActive(show);
    }
    public static void HideLobbyScreen() => ShowLobbyScreen(false);
}
