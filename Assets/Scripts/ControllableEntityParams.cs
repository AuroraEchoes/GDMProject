using UnityEngine;

[CreateAssetMenu(fileName = "ControllableEntityParams", menuName = "Scriptable Objects/ControllableEntityParams")]
public class ControllableEntityParams : ScriptableObject
{
    public Vector2 ForwardDirection = Vector2.right;
    public float MaxVelocity = 1.0f;
    public float Acceleration = 1.0f;
    public float Deceleration = 1.0f;
}
