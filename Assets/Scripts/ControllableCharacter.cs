using System;
using UnityEngine;

public class ControllableCharacter : MonoBehaviour
{
    [SerializeField] private ControllableEntityParams parameters;
    private Vector2 moveDir;
    private bool accelerating = false;
    private float velocity;
    private bool jumping;
    private bool jumpHoldWindowActive;
    private float jumpStartTime;
    private float jumpHoldWindowEndTime;

    private CharacterController charCtrl;

    public void SetInputAxes(Vector2 input)
    {
        accelerating = !input.Equals(Vector2.zero);
        if (accelerating)
            moveDir = input.normalized;
    }

    public void JumpWindowStart()
    {
        bool canJump = !jumping && charCtrl.isGrounded;
        if (canJump)
        {
            jumpHoldWindowActive = true;
            jumping = true;
            jumpStartTime = Time.time;
        }
    }

    public void JumpWindowEnd()
    {
        if (jumping && jumpHoldWindowActive)
        {
            jumpHoldWindowActive = false;
            jumpHoldWindowEndTime = Time.time;
        }
    }

    void Start()
    {
        charCtrl = GetComponent<CharacterController>();
    }

    void Update()
    {
        // End hold window phase of jump
        if (jumpHoldWindowActive)
        {
            float windowExpiry = jumpStartTime + parameters.JumpHoldWindowLength;
            if (Time.time > windowExpiry)
                JumpWindowEnd();
        }
        // End jump
        if (jumping && !jumpHoldWindowActive)
        {
            float jumpCompletion = (Time.time - jumpHoldWindowEndTime) / parameters.JumpBaseLength;
            jumping = jumpCompletion <= 1.0f;
        }
    }

    void FixedUpdate()
    {
        Vector3 movement = Vector3.zero;
        Vector3 gravity = Vector3.down * parameters.gravityStrength;
        float velocityDelta = (accelerating ? parameters.Acceleration : -parameters.Deceleration) * Time.fixedDeltaTime;
        velocity = Mathf.Clamp(velocity + velocityDelta, 0, parameters.MaxVelocity);
        Vector3 walk = moveDir.ToVector3XZ() * velocity;
        movement += gravity;
        movement += walk;
        if (jumping)
        {
            float jumpCompletion = (Time.time - jumpHoldWindowEndTime) / parameters.JumpBaseLength;
            Vector3 jump = JumpVelocity(jumpHoldWindowActive ? 0.0f : jumpCompletion);
            movement += jump;
        }
        charCtrl.Move(movement * Time.fixedDeltaTime);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rb = hit.collider.attachedRigidbody;
        if (rb is null || rb.isKinematic)
            return;
        // Move smoothly, instead of rotating
        rb.MovePosition(rb.transform.position + (hit.moveDirection.normalized * Time.fixedDeltaTime));
    }

    private Vector3 JumpVelocity(float completion)
        => moveDir.ToVector3XZ() * TweenJump(completion, parameters.JumpForwardVelocity)
        + Vector3.up * TweenJump(completion, parameters.JumpUpwardVelocity);
    
    private float TweenJump(float completion, float maxValue) => maxValue * Mathf.Pow(1.0f - completion, 5.0f);
}