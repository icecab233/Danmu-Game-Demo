using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Int Game Event", menuName = "Game Events/Int Game Event")]
public class IntEvent : ScriptableObject
{
    private List<IntEventListener> listeners = new List<IntEventListener>();

    public void Raise(int value)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised(value);
        }
    }

    public void RegisterListener(IntEventListener listener)
    {
        listeners.Add(listener);
    }

    public void UnRegisterListener(IntEventListener listener)
    {
        listeners.Remove(listener);
    }
}
