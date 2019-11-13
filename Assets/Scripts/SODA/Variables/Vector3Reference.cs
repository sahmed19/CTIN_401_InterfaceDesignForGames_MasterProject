using System;
using UnityEngine;

[Serializable]
public class Vector3Reference
{
    public bool useConstant = true;
    public Vector3 constantValue;
    public Vector3Variable variable;

    public Vector3Reference()
    { }

    public Vector3Reference(Vector3 value)
    {
        useConstant = true;
        constantValue = value;
    }

    public Vector3 Value
    {
        get { return useConstant ? constantValue : variable.value; }
    }

    public static implicit operator Vector3(Vector3Reference reference)
    {
        return reference.Value;
    }

    public float x { get { return Value.x; } }
    public float y { get { return Value.y; } }
    public float z { get { return Value.z; } }
    public float sqrMagnitude {
        get { return Value.sqrMagnitude; }
    }

}