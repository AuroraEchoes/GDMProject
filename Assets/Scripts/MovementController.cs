using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private List<ControllableCharacter> controllingCharacters = new List<ControllableCharacter>();
    [SerializeField] private float jumpHoldBufferTime = 0.75f;
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
        bool jumpBufferFull = timeSinceJumpDown >= jumpHoldBufferTime && Input.GetKey(KeyCode.Space);
        bool shouldJump = (!Input.GetKey(KeyCode.Space) && timeSinceJumpDown >= jumpMinHoldTime) || jumpBufferFull;
        if (chargingJump && shouldJump)
        {
            chargingJump = false;
            foreach (ControllableCharacter character in controllingCharacters)
                character.Jump(Mathf.Max(jumpMinHeightPercentage, timeSinceJumpDown / jumpHoldBufferTime));
        }
    }
}