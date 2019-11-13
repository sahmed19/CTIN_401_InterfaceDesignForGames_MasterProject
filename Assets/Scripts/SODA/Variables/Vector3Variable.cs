using UnityEngine;

[CreateAssetMenu]
public class Vector3Variable : SODAVariable
{

    public Vector3 value;

    public float x { get { return value.x; } }
    public float y { get { return value.y; } }
    public float z { get { return value.z; } }

    public float sqrMagnitude {
        get { return Mathf.Pow(this.x, 2) + Mathf.Pow(this.y,2) + Mathf.Pow(this.z, 2); }
    }

    public void SetValue(Vector3 v)
    {
        value = v;
    }

    public void SetValue(Vector3Variable v)
    {
        value = v.value;
    }

    public void ModValue(Vector3 amount)
    {
        value += amount;
    }

    public void ModValue(Vector3Variable amount)
    {
        value += amount.value;
    }

    public override string ToString() {
        return "(" + value.x + "," + value.y + "," + value.z + ")";
    }

}