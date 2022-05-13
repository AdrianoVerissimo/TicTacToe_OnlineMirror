using UnityEngine;
using System.Collections;

public interface IMatchController_OnStartMatch
{
    void OnStartMatch();
}
public interface IMatchController_OnEndMatch
{
    void OnEndMatch();
}
public interface IMatchController_OnStartTurn
{
    void OnStartTurn();
}
public interface IMatchController_OnEndTurn
{
    void OnEndTurn();
}