using UnityEngine;
using System.Collections;

public class BattleController_OnEndMatch_ShowEndMatchButtons : MonoBehaviour, IMatchController_OnEndMatch
{
    public void OnEndMatch()
    {
        MatchButtonsController.ShowEndMatchButtons();
    }
}
