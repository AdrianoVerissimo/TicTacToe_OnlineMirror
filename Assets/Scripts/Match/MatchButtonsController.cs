using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MatchButtonsController : Singleton<MatchButtonsController>
{
    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button QuitMatchButton;

    public static void ShowMatchButtons()
    {
        Instance.playAgainButton.gameObject.SetActive(false);
        Instance.QuitMatchButton.gameObject.SetActive(true);
    }
    public static void ShowEndMatchButtons()
    {
        Instance.playAgainButton.gameObject.SetActive(true);
        Instance.QuitMatchButton.gameObject.SetActive(true);
    }
}
