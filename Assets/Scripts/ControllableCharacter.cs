using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ControllableCharacter : MonoBehaviour
{
    private Vector2 moveDir;
    [SerializeField] private float maxVelocity = 1.0f;
    [SerializeField] private float acceleration = 1.0f;
    [SerializeField] private float deceleration = 1.0f;
    private bool accelerating = false;
    private float accelStartTime;
    private float decelStartTime;
    private float velocity;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetInputAxes(Vector2 input)
    {
        accelerating = !input.Equals(Vector2.zero);
        if (accelerating)
            moveDir = input.normalized;
    }


    void FixedUpdate()
    {
        float velocityDelta = (accelerating ? acceleration : -deceleration) * Time.fixedDeltaTime;
        velocity = Mathf.Clamp(velocity + velocityDelta, 0, maxVelocity);
        Vector3 moveAmount = new Vector3(moveDir.x, 0, moveDir.y) * velocity;
        rb.MovePosition(transform.position + moveAmount);
    }
}
