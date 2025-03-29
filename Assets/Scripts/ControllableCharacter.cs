using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class ControllableCharacter : MonoBehaviour
{
    [SerializeField] private ControllableEntityParams parameters;
    private Vector2 moveDir;
    private bool accelerating = false;
    private float velocity;
    private bool jumping => jumpStartTime != 0.0f && Time.time <= jumpStartTime + parameters.jumpTime;
    private float jumpStartTime;
    private float jumpHeight;
    private CharacterController charCtrl;

    public void SetInputAxes(Vector2 input)
    {
        accelerating = !input.Equals(Vector2.zero);
        if (accelerating)
            moveDir = input.normalized;
    }

    // @param amount: Decimal percentage of maximum height
    public void Jump(float height)
    {
        if (charCtrl.isGrounded)
        {
            jumpStartTime = Time.time;
            jumpHeight = height;
        }
    }

    void Start()
    {
        charCtrl = GetComponent<CharacterController>();
    }

    void FixedUpdate()
    {
        Vector3 movement = Vector3.zero;
        Vector3 gravity = Vector3.down * parameters.gravityStrength;
        float velocityDelta = (accelerating ? parameters.acceleration : -parameters.deceleration) * Time.fixedDeltaTime;
        velocity = Mathf.Clamp(velocity + velocityDelta, 0, parameters.maxVelocity);
        Vector3 walk = moveDir.ToVector3XZ() * velocity;
        movement += gravity;
        movement += walk;
        if (jumping)
        {
            float completion = (Time.time - jumpStartTime) / parameters.jumpTime;
            Vector3 jump = JumpVelocity(completion) * jumpHeight;
            movement += jump;
        }
        charCtrl.Move(movement * Time.fixedDeltaTime);
    }

    private Vector3 JumpVelocity(float completion)
        => moveDir.ToVector3XZ() * TweenJump(completion, parameters.jumpBaseForwardVelocity)
        + Vector3.up * TweenJump(completion, parameters.jumpBaseUpwardVelocity);
    
    private float TweenJump(float completion, float maxValue) => maxValue * Mathf.Pow(1.0f - completion, 5.0f);
}