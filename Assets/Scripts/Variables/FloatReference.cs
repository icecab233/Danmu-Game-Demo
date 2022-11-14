using System;
using UnityEngine;

[Serializable]
public class FloatReference : MonoBehaviour
{
    public bool useConstant = true;
    public float constantValue;
    public FloatVariable variable;

    public float value
    {
        get { return useConstant ? constantValue : variable.value; }
    }
}
