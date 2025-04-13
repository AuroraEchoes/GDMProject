using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class ControllableCharacter : MonoBehaviour
{
    private MovementController ctrl;
    private Rigidbody rb;
    private Vector2 moveDir;
    private Vector2 nonZeroMoveDir;
    private Vector2 prevNonZeroMoveDir;
    private bool accelerating;
    private float velocity;
    private float rotationCompletion = 0.0f;
    private float rotationTime => 1.0f / ctrl.MovementParams.RotationSpeed;
    private float gravityDisabledTime;

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
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        // Position
        Vector3 movement = Vector3.zero;
        float velocityDelta = (accelerating ? ctrl.Acceleration : -ctrl.Deceleration) * Time.fixedDeltaTime;
        velocity = Mathf.Clamp(velocity + velocityDelta, 0, ctrl.MaxVelocity);
        Vector3 walk = moveDir.ToVector3XZ() * velocity;
        movement += walk;
        Vector3 climbWall = ClimbWall(movement);
        if (climbWall.Equals(Vector3.zero))
            rb.MovePosition(transform.position + (movement * Time.fixedDeltaTime));
        else
            rb.position += climbWall;

        // Prevent sliding around
        rb.linearVelocity = Vector3.Min(rb.linearVelocity, movement);

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
            rb.MoveRotation(Quaternion.LookRotation(rotationVec.Rotate(-90).ToVector3XZ()));
    }

    private Vector3 ClimbWall(Vector3 movement)
    {
        Vector3 moveDir = nonZeroMoveDir.ToVector3XZ();
        Vector3 wallRayStartPos = transform.position + Vector3.up * 0.2f;
        RaycastHit wallRangefinder;
        bool rangefinderHit = Physics.Raycast(wallRayStartPos, moveDir, out wallRangefinder, 1.0f);
        if (!rangefinderHit) return Vector3.zero;
        if (wallRangefinder.rigidbody is null || !wallRangefinder.rigidbody.isKinematic) return Vector3.zero;
        // Debug.Log($"Rangefinder: {wallRangefinder.distance} Target: {wallRangefinder.collider.gameObject.name} Position: {wallRangefinder.transform.position}");
        float minClearDist = Mathf.Tan(ctrl.MovementParams.MaxSlopeAngle.ToRadians()) * ctrl.MovementParams.MaxClimbHeight;
        Vector3 slopeRayStartPoint = wallRangefinder.point + Vector3.up * ctrl.MovementParams.MaxClimbHeight;
        bool climbableSlope = !Physics.Raycast(slopeRayStartPoint, moveDir, minClearDist);

        Vector3 climbRayStartPos = wallRangefinder.point
            + movement * Time.fixedDeltaTime 
            + Vector3.up * ctrl.MovementParams.MaxClimbHeight;
        RaycastHit climbRay;
        bool climbRayHit = Physics.Raycast(climbRayStartPos, Vector3.down, out climbRay, ctrl.MovementParams.MaxClimbHeight);
        if (!climbRayHit) return Vector3.zero;
        return climbRay.point - wallRangefinder.point;
    }
}