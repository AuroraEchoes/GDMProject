using UnityEngine;

[CreateAssetMenu(fileName = "ControllableEntityParams", menuName = "Scriptable Objects/ControllableEntityParams")]
public class ControllableEntityParams : ScriptableObject
{
    public Vector2 ForwardDirection = Vector2.right;
    public float MaxVelocity = 1.0f;
    public float Acceleration = 1.0f;
    public float RotationSpeed = 1.0f;
    public float MaxClimbHeight = 0.2f;
    public float MaxSlopeAngle = 45.0f;
    public float GravityStrength = 5.0f;
}
