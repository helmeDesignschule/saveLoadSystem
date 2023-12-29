using UnityEngine;

/// <summary>
/// We sadly cannot simply save the unity Vector3 variables.
/// Instead, we have to convert it to a serializable object the json converter can read.
/// We do this by saving the three axis separately.
/// </summary>
[System.Serializable]
public struct SaveableVector3
{
    public float x;
    public float y;
    public float z;

    public SaveableVector3(Vector3 startValue)
    {
        x = startValue.x;
        y = startValue.y;
        z = startValue.z;
    }
    
    public void SetFromVector(Vector3 vector)
    {
        x = vector.x;
        y = vector.y;
        z = vector.z;
    }

    public Vector3 GetVector3()
    {
        return new Vector3(x, y, z);
    }

    //this code allows us to directly assign vector3 variables to a SaveableVector3 variable (and vice versa).
    //it will automatically be converted.
    public static implicit operator SaveableVector3(Vector3 origin) => new SaveableVector3(origin);
    public static implicit operator Vector3(SaveableVector3 origin) => origin.GetVector3();
}
