using System;
using UnityEngine;

public class ControllableCharacter : MonoBehaviour
{
    private CharacterController charCtrl;
    private Rigidbody childRb;
    private MovementController ctrl;
    private Vector2 moveDir;
    private Vector2 nonZeroMoveDir;
    private Vector2 prevNonZeroMoveDir;
    private Vector3 moveForce;
    private bool accelerating;
    private float velocity;
    private float rotationCompletion = 0.0f;
    private float rotationTime => 1.0f / ctrl.MovementParams.RotationSpeed;

    public void SetInputAxes(Vector2 input)
    {
        Vector2 rotatedInput = input.Rotate(ctrl.ForwardDirection.Angle()).normalized;
        accelerating = !input.Equals(Vector2.zero);
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
        charCtrl = GetComponent<CharacterController>();
        childRb = GetComponentInChildren<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 movement = Vector3.zero;
        Vector3 gravity = Vector3.down * 6.0f;
        float velocityDelta = (accelerating ? ctrl.Acceleration : -ctrl.Deceleration) * Time.fixedDeltaTime;
        velocity = Mathf.Clamp(velocity + velocityDelta, 0, ctrl.MaxVelocity);
        Vector3 walk = moveDir.ToVector3XZ() * velocity;
        movement += gravity;
        movement += walk;
        Vector3 movementTimeScaled = movement * Time.fixedDeltaTime;
        RaycastHit hit;
        Vector3 avoidCollisions = childRb.SweepTest(movement.normalized, out hit, movementTimeScaled.magnitude)
            ? movementTimeScaled.normalized * (hit.distance + 0.01f)
            : movementTimeScaled;
        charCtrl.Move(avoidCollisions);
        moveForce = new Vector3(movementTimeScaled.x, 0.0f, movementTimeScaled.z);

        // Rotation
        Vector2 rotationVec = nonZeroMoveDir;
        if (rotationCompletion < 1.0f)
        {
            float deltaCompletion = Time.fixedDeltaTime / rotationTime;
            rotationCompletion += deltaCompletion;
            float totalRotation = Vector2.SignedAngle(prevNonZeroMoveDir, nonZeroMoveDir);
            float thisRotation = Mathf.Lerp(0, totalRotation, rotationCompletion);
            rotationVec = prevNonZeroMoveDir.Rotate(thisRotation);
        }
        if (!rotationVec.Equals(Vector2.zero))
            transform.rotation = Quaternion.LookRotation(rotationVec.Rotate(-90).ToVector3XZ());
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        PushBlock(hit.gameObject, hit.rigidbody);
    }

    public void PushBlock(GameObject obj, Rigidbody rb)
    {
        if (obj.CompareTag("Pushable"))
        {
            Vector3 blockRelativePos = (rb.position - transform.position).normalized;
            Vector3 moveDir = nonZeroMoveDir.ToVector3XZ();
            float angle = Vector3.Angle(moveDir, blockRelativePos);
            if (angle < 90.0f)
            {
                obj.GetComponent<FixRigidbody>().Move(moveForce);
            }
        }
    }
}