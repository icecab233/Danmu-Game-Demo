using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EventDebugger)), CanEditMultipleObjects]
public class CustomEventDebugInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EventDebugger eventDebugger = (EventDebugger)target;
        if (GUILayout.Button("Call Void Event"))
        {
            eventDebugger.voidEvent.Raise();
        }
        if (GUILayout.Button("Call Int Event"))
        {
            eventDebugger.intEvent.Raise(eventDebugger.intEventValue);
        }
        if (GUILayout.Button("Call String Event"))
        {
            eventDebugger.stringEvent.Raise(eventDebugger.stringEventValue);
        }
    }
}