using System;
using UnityEngine;

public static class Utils
{
    public static Vector3 ToVector3XZ(this Vector2 vector)
    {
        return new Vector3(vector.x, 0, vector.y);
    }

    public static Vector2 Rotate(this Vector2 vector, float degrees)
    {
        return new Vector2
        (
            vector.x * Mathf.Cos(degrees.ToRadians()) - vector.y * Mathf.Sin(degrees.ToRadians()),
            vector.x * Mathf.Sin(degrees.ToRadians()) + vector.y * Mathf.Cos(degrees.ToRadians())
        );
    }

    public static float Angle(this Vector2 vector)
    {
        return Vector2.SignedAngle(Vector2.up, vector);
    }

    public static float ToRadians(this float degrees)
    {
        return degrees * (Mathf.PI / 180.0f);
    }

    public static Vector2 ToVector2(this float degrees)
    {
        return new Vector2(Mathf.Sin(degrees.ToRadians()), Mathf.Cos(degrees.ToRadians()));
    }
}