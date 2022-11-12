using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Void Game Event", menuName = "Game Events/Void Game Event")]
public class VoidEvent : ScriptableObject
{
    private List<VoidEventListener> listeners = new List<VoidEventListener>();

    public void Raise()
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised();
        }
    }

    public void RegisterListener(VoidEventListener listener)
    {
        listeners.Add(listener);
    }

    public void UnRegisterListener(VoidEventListener listener)
    {
        listeners.Remove(listener);
    }
}
