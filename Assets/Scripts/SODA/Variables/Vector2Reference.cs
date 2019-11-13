using System;
using UnityEngine;

[Serializable]
public class Vector2Reference
{
    public bool useConstant = true;
    public Vector2 constantValue;
    public Vector2Variable variable;

    public Vector2Reference()
    { }

    public Vector2Reference(Vector2 value)
    {
        useConstant = true;
        constantValue = value;
    }

    public Vector2 Value
    {
        get { return useConstant ? constantValue : variable.value; }
    }

    public static implicit operator Vector2(Vector2Reference reference)
    {
        return reference.Value;
    }

    public float x { get { return Value.x; } }
    public float y { get { return Value.y; } }
    public float sqrMagnitude {
        get { return Value.sqrMagnitude; }
    }

}