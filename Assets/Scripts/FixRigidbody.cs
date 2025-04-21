using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;

public class FixRigidbody : MonoBehaviour
{
    private Vector3 lastValidPos;
    private Quaternion lastValidRotation;
    private Vector3 movementBuf;
    private Rigidbody rb;
    [SerializeField] private bool useGravity = true;

    public void Move(Vector3 dist)
    {
        movementBuf += dist;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = 0.0f;
        rb.maxDepenetrationVelocity = 0.0f;
        rb.maxLinearVelocity = 0.0f;
        lastValidPos = rb.position;
        lastValidRotation = rb.rotation;
    }

    void FixedUpdate()
    {
        if (rb.position != lastValidPos)
            rb.position = lastValidPos;
        if (rb.rotation != lastValidRotation)
            rb.rotation = lastValidRotation;
        Vector3 deltaMovement = movementBuf;
        Vector3 gravity = useGravity ? ApplyGravity(deltaMovement) : deltaMovement;
        Vector3 moveDir = gravity.normalized;
        RaycastHit hit;
        if (rb.SweepTest(moveDir, out hit, gravity.magnitude + 0.2f))
        {
            Vector3 climbingMovement;
            // Don’t use gravity when climbing
            if (ClimbWall(deltaMovement, out climbingMovement))
            {
                rb.position += climbingMovement;
            }
            else
            {
                rb.position += moveDir * (hit.distance - 0.01f);
            }
        }
        else
        {
            rb.position += gravity;
        };
        lastValidPos = rb.position;
        lastValidRotation = rb.rotation;
        movementBuf = Vector3.zero;
    }

    bool ClimbWall(Vector3 movement, out Vector3 climbingMovement)
    {
        climbingMovement = movement;
        float maxClimbDist = 0.4f;
        RaycastHit sweep;
        Vector3 moveDir = movement.normalized;
        bool wallExists = rb.SweepTest(moveDir, out sweep, movement.magnitude);
        if (!wallExists)
            return false;
        Vector3 floorContactOrigin = sweep.point - (moveDir * sweep.distance);
        Vector3 posWithClimb = rb.position + Vector3.up * maxClimbDist;
        bool climbable = !Physics.Raycast(posWithClimb, moveDir, movement.magnitude);
        if (!climbable)
            return false;
        RaycastHit climbedHit;
        Vector3 moveDirWithClimb = moveDir * (sweep.distance + 0.01f) + Vector3.up * maxClimbDist;
        Debug.DrawRay(floorContactOrigin + moveDirWithClimb, Vector3.down, Color.yellow, 10.0f);
        bool hitDown = Physics.Raycast(floorContactOrigin + moveDirWithClimb, Vector3.down, out climbedHit, maxClimbDist);
        if (!hitDown)
            return false;
        climbingMovement = climbedHit.point - floorContactOrigin + Vector3.up * 0.02f;
        return true; 
    }

    Vector3 ApplyGravity(Vector3 movement)
    {
        float gravityStrength = 1.0f;
        RaycastHit floor;
        bool hitFloor = rb.SweepTest(Vector3.down, out floor, gravityStrength);
        if (!hitFloor)
            return movement + Vector3.down * gravityStrength; float gravDist = Mathf.Min(floor.distance, gravityStrength) - 0.01f;
        return movement + Vector3.down * gravDist;
    }
}
