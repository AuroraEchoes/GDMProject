using System;
using UnityEngine;

public class ControllableCharacter : MonoBehaviour
{
    public MovementController Controller;
    private Rigidbody rb;
    private Vector2 moveDir;
    private bool accelerating = false;
    private float velocity;

    public void SetInputAxes(Vector2 input)
    {
        accelerating = !input.Equals(Vector2.zero);
        if (accelerating)
            moveDir = input.Rotate(Controller.ForwardDirection.Angle()).normalized;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Rotate cat to look in moving direction (VERY CRUDE)
        if (!moveDir.Equals(Vector2.zero))
            transform.rotation = Quaternion.LookRotation(moveDir.Rotate(-90).ToVector3XZ());
    }

    void FixedUpdate()
    {
        Vector3 movement = Vector3.zero;
        float velocityDelta = (accelerating ? Controller.Acceleration : -Controller.Deceleration) * Time.fixedDeltaTime;
        velocity = Mathf.Clamp(velocity + velocityDelta, 0, Controller.MaxVelocity);
        Vector3 walk = moveDir.ToVector3XZ() * velocity;
        movement += walk;
        rb.MovePosition(transform.position + (movement * Time.fixedDeltaTime));
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody otherRb = hit.collider.attachedRigidbody;
        if (otherRb is null || otherRb.isKinematic) return;
        // Move smoothly, instead of rotating affected objects
        otherRb.MovePosition(otherRb.transform.position + (hit.moveDirection.normalized * Time.fixedDeltaTime));
    }
}