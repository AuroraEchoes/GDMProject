using UnityEngine;

[CreateAssetMenu(fileName = "ControllableEntityParams", menuName = "Scriptable Objects/ControllableEntityParams")]
public class ControllableEntityParams : ScriptableObject
{
    public float maxVelocity = 1.0f;
    public float acceleration = 1.0f;
    public float deceleration = 1.0f;
    public float jumpBaseForwardVelocity = 1.0f;
    public float jumpBaseUpwardVelocity = 2.0f;
    public float jumpTime = 1.0f;
    public float gravityStrength = 9.81f;   
}
