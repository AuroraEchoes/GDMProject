using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class ControllableCharacter : MonoBehaviour
{
    private Vector2 moveDir;
    [SerializeField] private float maxVelocity = 1.0f;
    [SerializeField] private float acceleration = 1.0f;
    [SerializeField] private float deceleration = 1.0f;
    [SerializeField] private float jumpBaseForwardVelocity = 1.0f;
    [SerializeField] private float jumpBaseUpwardVelocity = 2.0f;
    [SerializeField] private float jumpTime = 1.0f;
    [SerializeField] private float gravityStrength = 9.81f;
    private bool accelerating = false;
    private float velocity;
    private bool jumping => jumpStartTime != 0.0f && Time.time <= jumpStartTime + jumpTime;
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
            Debug.Log("Jump with height " + jumpHeight);
        }
    }

    void Start()
    {
        charCtrl = GetComponent<CharacterController>();
    }

    void FixedUpdate()
    {
        Vector3 movement = Vector3.zero;
        Vector3 gravity = Vector3.down * gravityStrength;
        float velocityDelta = (accelerating ? acceleration : -deceleration) * Time.fixedDeltaTime;
        velocity = Mathf.Clamp(velocity + velocityDelta, 0, maxVelocity);
        Vector3 walk = moveDir.ToVector3XZ() * velocity;
        movement += gravity;
        movement += walk;
        if (jumping)
        {
            float completion = (Time.time - jumpStartTime) / jumpTime;
            Vector3 jump = JumpVelocity(completion) * jumpHeight;
            movement += jump;
        }
        charCtrl.Move(movement * Time.fixedDeltaTime);
    }

    private Vector3 JumpVelocity(float completion)
        => moveDir.ToVector3XZ() * TweenJump(completion, jumpBaseForwardVelocity)
        + Vector3.up * TweenJump(completion, jumpBaseUpwardVelocity);
    
    private float TweenJump(float completion, float maxValue) => maxValue * Mathf.Pow(1.0f - completion, 5.0f);
}