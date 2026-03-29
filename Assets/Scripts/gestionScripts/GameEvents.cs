using System;
using UnityEngine;

public static class GameEvents
{
    public static event Action<GameObject> OnLaunch;
    public static event Action OnHold;

    public static void OnLaunchEv(GameObject obj)
    {
        OnLaunch?.Invoke(obj);
    }

    public static void OnHoldEv()
    {
        OnHold?.Invoke();
    }
}