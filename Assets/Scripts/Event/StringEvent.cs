using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New String Game Event", menuName = "Game Events/String Game Event")]
public class StringEvent : ScriptableObject
{
    private List<StringEventListener> listeners = new List<StringEventListener>();

    public void Raise(string value)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised(value);
        }
    }

    public void RegisterListener(StringEventListener listener)
    {
        listeners.Add(listener);
    }

    public void UnRegisterListener(StringEventListener listener)
    {
        listeners.Remove(listener);
    }
}
