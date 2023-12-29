
using UnityEngine;

/// <summary>
/// We sadly cannot simply save the unity Vector2 variables.
/// Instead, we have to convert it to a serializable object the json converter can read.
/// We do this by saving the three axis separately.
/// </summary>
[System.Serializable]
public class SaveableVector2
{
    public float x;
    public float y;

    public SaveableVector2(Vector2 startValue)
    {
        x = startValue.x;
        y = startValue.y;
    }

    public void SetFromVector(Vector2 vector)
    {
        x = vector.x;
        y = vector.y;
    }

    public Vector2 GetVector2()
    {
        return new Vector2(x, y);
    }

    //this code allows us to directly assign Vector2 variables to a SaveableVector2 variable (and vice versa).
    //it will automatically be converted.
    public static implicit operator SaveableVector2(Vector2 origin) => new SaveableVector2(origin);
    public static implicit operator Vector2(SaveableVector2 origin) => origin.GetVector2();
}
