using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Game Event", menuName = "Game Events/Player Game Event")]
public class PlayerEvent : ScriptableObject
{
    private List<PlayerEventListener> listeners = new List<PlayerEventListener>();

    public void Raise(Player value)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised(value);
        }
    }

    public void RegisterListener(PlayerEventListener listener)
    {
        listeners.Add(listener);
    }

    public void UnRegisterListener(PlayerEventListener listener)
    {
        listeners.Remove(listener);
    }
}
