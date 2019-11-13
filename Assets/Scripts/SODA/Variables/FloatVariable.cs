using UnityEngine;

[CreateAssetMenu]
public class FloatVariable : SODAVariable
{
    public float value;

    public void SetValue(float v)
    {
        value = v;
    }

    public void SetValue(FloatVariable v)
    {
        value = v.value;
    }

    public void ModValue(float amount)
    {
        value += amount;
    }

    public void ModValue(FloatVariable amount)
    {
        value += amount.value;
    }

    public static implicit operator float(FloatVariable reference)
    {
        return reference.value;
    }

    public override string ToString() {
        return "" + value;
    }

}