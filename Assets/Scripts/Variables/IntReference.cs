using System;
using UnityEngine;

[Serializable]
public class IntReference : MonoBehaviour
{
    public bool useConstant = true;
    public int constantValue;
    public IntVariable variable;

    public int value
    {
        get { return useConstant ? constantValue : variable.value; }
    }
}
