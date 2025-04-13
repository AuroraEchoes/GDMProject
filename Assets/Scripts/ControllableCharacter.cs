using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class ControllableCharacter : MonoBehaviour
{
    public MovementController Controller;
    private Rigidbody rb;
    private Vector2 moveDir;
    private Vector2 nonZeroMoveDir;
    private Vector2 prevNonZeroMoveDir;
    private bool accelerating = false;
    private float velocity;
    private float rotationCompletion = 0.0f;
    private float rotationTime => 1.0f / Controller.MovementParams.RotationSpeed;

    public void SetInputAxes(Vector2 input)
    {
        Vector2 rotatedInput = input.Rotate(Controller.ForwardDirection.Angle()).normalized;
        accelerating = !rotatedInput.Equals(Vector2.zero);
        if (accelerating && !moveDir.Equals(rotatedInput))
            // Move dir has changed; we should rotate
            if (!rotatedInput.Equals(Vector2.zero))
            {
                prevNonZeroMoveDir = nonZeroMoveDir.Equals(rotatedInput) ? prevNonZeroMoveDir : nonZeroMoveDir;
                nonZeroMoveDir = rotatedInput;
                rotationCompletion = 0.0f;
            }
            moveDir = rotatedInput;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector2 rotationVec = nonZeroMoveDir;

        if (rotationCompletion < 1.0f)
        {
            float deltaCompletion = Time.deltaTime / rotationTime;
            rotationCompletion += deltaCompletion;
            float totalRotation = Vector2.SignedAngle(prevNonZeroMoveDir, nonZeroMoveDir);
            float thisRotation = Mathf.Lerp(0, totalRotation, rotationCompletion);
            rotationVec = prevNonZeroMoveDir.Rotate(thisRotation);
        }
        rb.MoveRotation(Quaternion.LookRotation(rotationVec.Rotate(-90).ToVector3XZ()));
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