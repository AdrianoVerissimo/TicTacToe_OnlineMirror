using UnityEngine;
using System.Collections;

public class BattleController_OnStartMatch_ShowMatchButtons : MonoBehaviour, IMatchController_OnStartMatch
{
    public void OnStartMatch()
    {
        MatchButtonsController.ShowMatchButtons();
    }
}
