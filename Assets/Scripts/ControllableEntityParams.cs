using UnityEngine;

[CreateAssetMenu(fileName = "ControllableEntityParams", menuName = "Scriptable Objects/ControllableEntityParams")]
public class ControllableEntityParams : ScriptableObject
{
    public float MaxVelocity = 1.0f;
    public float Acceleration = 1.0f;
    public float Deceleration = 1.0f;
    public float JumpForwardVelocity = 1.0f;
    public float JumpUpwardVelocity = 2.0f;
    public float JumpBaseLength = 1.0f;
    public float JumpHoldWindowLength = 0.3f;
    public float gravityStrength = 9.81f;   
}
