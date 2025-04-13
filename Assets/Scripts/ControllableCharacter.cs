using System;
using UnityEngine;

public class ControllableCharacter : MonoBehaviour
{
    [SerializeField] private ControllableEntityParams parameters;
    private Vector2 moveDir;
    private bool accelerating = false;
    private float velocity;

    private CharacterController charCtrl;

    public void SetInputAxes(Vector2 input)
    {
        accelerating = !input.Equals(Vector2.zero);
        if (accelerating)
            moveDir = input.normalized;
    }

    void Start()
    {
        charCtrl = GetComponent<CharacterController>();
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
        Vector3 gravity = Vector3.down * parameters.gravityStrength;
        float velocityDelta = (accelerating ? parameters.Acceleration : -parameters.Deceleration) * Time.fixedDeltaTime;
        velocity = Mathf.Clamp(velocity + velocityDelta, 0, parameters.MaxVelocity);
        Vector3 walk = moveDir.ToVector3XZ() * velocity;
        movement += gravity;
        movement += walk;
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
}