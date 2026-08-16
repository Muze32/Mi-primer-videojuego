using System;
using UnityEngine;

public static class GameEvents
{
    public static event Action<GameObject> OnLaunch;
    public static event Action<GameObject> OnHold;
    public static event Action OnNextLevel;
    public static event Action OnGameOver;
    public static event Action OnNextTurn;
    public static event Action OnPause;

    public static void OnLaunchEv(GameObject obj)
    {
        OnLaunch?.Invoke(obj);
    }

    public static void OnHoldEv(GameObject obj)
    {
        OnHold?.Invoke(obj);
    }

    public static void OnNextTurnEv()
    {
        OnNextTurn?.Invoke();
    }
    
    public static void OnNextLevelEv()
    {
        OnNextLevel?.Invoke();
    }

    public static void OnGameOverEv()
    {
        OnGameOver?.Invoke();
    }

    public static void OnPauseEv()
    {
        OnPause?.Invoke();
    }
}