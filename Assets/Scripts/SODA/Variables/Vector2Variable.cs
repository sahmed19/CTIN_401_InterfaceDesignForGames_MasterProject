using UnityEngine;

[CreateAssetMenu]
public class Vector2Variable : SODAVariable
{

    public Vector2 value;

    public float x { get { return value.x; } }
    public float y { get { return value.y; } }

    public float sqrMagnitude {
        get { return Mathf.Pow(x, 2) + Mathf.Pow(y,2); }
    }

    public void SetValue(Vector2 v)
    {
        value = v;
    }

    public void SetValue(Vector2Variable v)
    {
        value = v.value;
    }

    public void ModValue(Vector2 amount)
    {
        value += amount;
    }

    public void ModValue(Vector2Variable amount)
    {
        value += amount.value;
    }

    public override string ToString() {
        return "(" + value.x + "," + value.y + ")";
    }

}