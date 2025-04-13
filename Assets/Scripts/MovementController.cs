using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private List<ControllableCharacter> controllingCharacters = new List<ControllableCharacter>();

    public ControllableEntityParams MovementParams;
    public bool OverrideForwardDirection = false;
    public Vector2 ForwardDirectionOverride = Vector2.zero;
    public float MaxVelocity => MovementParams.MaxVelocity;
    public float Acceleration => MovementParams.Acceleration;
    public float Deceleration => MovementParams.Deceleration;
    public Vector2 ForwardDirection => OverrideForwardDirection ? ForwardDirectionOverride : MovementParams.ForwardDirection;

    void Start()
    {
        foreach (ControllableCharacter character in controllingCharacters)
            character.SetController(this);
    }


    void Update()
    {
        Vector2 inputMovement = Vector2.zero;
        if (Input.GetKey(KeyCode.W))
            inputMovement.y += 1;
        if (Input.GetKey(KeyCode.S))
            inputMovement.y -= 1;
        if (Input.GetKey(KeyCode.A))
            inputMovement.x -= 1;
        if (Input.GetKey(KeyCode.D))
            inputMovement.x += 1;
        foreach (ControllableCharacter character in controllingCharacters)
            character.SetInputAxes(inputMovement);
    }
}