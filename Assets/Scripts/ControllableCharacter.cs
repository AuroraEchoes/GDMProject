using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Scripting.APIUpdating;

public class ControllableCharacter : MonoBehaviour
{
    private MovementController ctrl;
    private Rigidbody rb;
    private Vector2 moveDir;
    private Vector2 nonZeroMoveDir;
    private Vector2 prevNonZeroMoveDir;
    private bool accelerating = false;
    private float velocity;
    private float rotationCompletion = 0.0f;
    private float rotationTime => 1.0f / ctrl.MovementParams.RotationSpeed;

    public void SetInputAxes(Vector2 input)
    {
        Vector2 rotatedInput = input.Rotate(ctrl.ForwardDirection.Angle()).normalized;
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

    public void SetController(MovementController ctrl) { this.ctrl = ctrl; }

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
        if (!rotationVec.Equals(Vector2.zero))
            rb.MoveRotation(Quaternion.LookRotation(rotationVec.Rotate(-90).ToVector3XZ()));
    }

    void FixedUpdate()
    {
        Vector3 movement = Vector3.zero;
        float velocityDelta = (accelerating ? ctrl.Acceleration : -ctrl.Deceleration) * Time.fixedDeltaTime;
        velocity = Mathf.Clamp(velocity + velocityDelta, 0, ctrl.MaxVelocity);
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