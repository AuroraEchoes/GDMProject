using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private List<ControllableCharacter> controllingCharacters = new List<ControllableCharacter>();
    [SerializeField] private float jumpMaxHoldTime = 0.75f;
    [SerializeField] private float jumpMinHoldTime = 0.2f;
    [SerializeField] private float jumpMinHeightPercentage = 0.4f;
    private float jumpDownTime;
    private bool chargingJump;

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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpDownTime = Time.time;
            chargingJump = true;
        }
        float timeSinceJumpDown = Time.time - jumpDownTime;
        bool jumpBufferFull = timeSinceJumpDown >= jumpMaxHoldTime && Input.GetKey(KeyCode.Space);
        bool shouldJump = (!Input.GetKey(KeyCode.Space) && timeSinceJumpDown >= jumpMinHoldTime) || jumpBufferFull;
        if (chargingJump && shouldJump)
        {
            float jumpHeight = 1 + ((1.0f - jumpMinHeightPercentage) / (jumpMaxHoldTime - jumpMinHoldTime)) * (timeSinceJumpDown - jumpMaxHoldTime);
            chargingJump = false;
            foreach (ControllableCharacter character in controllingCharacters)
                character.Jump(jumpHeight);
        }
    }
}