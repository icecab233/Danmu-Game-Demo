using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DanmuGame.events;

public class EventDebugger : MonoBehaviour
{
    [Header("Void Event")]
    public VoidEvent voidEvent;
    [Header("Int Event")]
    public int intEventValue;
    public IntEvent intEvent;
    [Header("String Event")]
    public string stringEventValue;
    public StringEvent stringEvent;
    [Header("IntString Event")]
    public int IntValue;
    public string StringValue;
    public IntStringEvent intStringEvent;
}
