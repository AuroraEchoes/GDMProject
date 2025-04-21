using System.Runtime.InteropServices;
using UnityEngine;

public class NewCharacterController : MonoBehaviour
{
    // Upgraded character controller made with heavy reference to the techniques
    // described in this excellent YouTube video https://www.youtube.com/watch?v=qdskE8PJy6Q

    private Rigidbody rb;
    private Vector2 inputMovement;

    public float TargetFloatHeight = 1.0f;
    public float FloatHeightCorrectionStrength = 1.0f;
    public float MaxVelocity = 1.0f;
    public float Acceleration = 0.8f;
    public float TurnSpeed = 1.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();        
    }

    void Update()
    {
        inputMovement = Vector2.zero;
        if (Input.GetKey(KeyCode.W))
            inputMovement.y += 1;
        if (Input.GetKey(KeyCode.S))
            inputMovement.y -= 1;
        if (Input.GetKey(KeyCode.A))
            inputMovement.x -= 1;
        if (Input.GetKey(KeyCode.D))
            inputMovement.x += 1;
    }

    void FixedUpdate()
    {
        // X-Z plane movement
        Vector3 targetMoveDir = inputMovement.normalized.ToVector3XZ();
        Vector3 currMoveDir = rb.linearVelocity.normalized;
        Vector3 actualMoveDir = Vector3.MoveTowards(currMoveDir, targetMoveDir, TurnSpeed);
        Vector3 currVelocity = rb.linearVelocity;
        Vector3 targetVelocity = actualMoveDir * MaxVelocity;
        Vector3 actualVelocity = Vector3.MoveTowards(currVelocity, targetVelocity, Acceleration);
        Vector3 deltaVelocity = actualVelocity - currVelocity;
        Vector3 timescaledDeltaVelocity = deltaVelocity;
        rb.AddForce(timescaledDeltaVelocity);

        // Floating
        RaycastHit downwardsCast;
        bool raycastHit = Physics.Raycast(rb.position, Vector3.down, out downwardsCast, TargetFloatHeight * 10.0f);
        if (raycastHit)
        {
            float heightOffset = downwardsCast.distance - TargetFloatHeight;
            float floatForce = -heightOffset * FloatHeightCorrectionStrength;
            rb.AddForce(Vector3.up * floatForce);
        }
        // We’re pretty high up; add the maximum downwards value
        else
        {
            rb.AddForce(Vector3.down * TargetFloatHeight * FloatHeightCorrectionStrength * 2.0f);
        }
    }
}
