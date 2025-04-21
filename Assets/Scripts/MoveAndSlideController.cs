using System.Security.Cryptography;
using UnityEngine;

public class MoveAndSlideController : MonoBehaviour
{
    public float SkinWidth = 0.02f;
    public int MaxSlideBounces = 3;
    public float Acceleration = 10.0f;
    public float MaxVelocity = 3.0f;
    public int CollisionPointCount = 100;
    public bool DebugDrawColliderPoints;

    private Collider coll;
    // private Bounds bounds;
    private Vector2 inputMovement;
    private Vector3 velocity;
    private Vector3[] colliderPoints;

    // THE PLAN:
    // 1. Fix gravity
    // Which will probably involve looking down and then going down as far as we can
    // Make sliding only work vertically (?)
    // ????
    // Profit

    public void Start()
    {
        coll = GetComponent<Collider>();
        GenerateColliderPoints();
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
        Vector3 targetMoveDir = inputMovement.normalized.ToVector3XZ();
        // Vector3 currMoveDir = velocity.normalized; 
        // Vector3 actualMoveDir = Vector3.MoveTowards(currMoveDir, targetMoveDir, TurnSpeed);
        Vector3 currVelocity = velocity;
        Vector3 targetVelocity = targetMoveDir * MaxVelocity;
        Vector3 baseVelocity = Vector3.MoveTowards(currVelocity, targetVelocity, Acceleration);
        velocity = Collide(transform.position, baseVelocity, 0, false);
        // Vector3 gravity = Collide(transform.position, Vector3.down, 0, true);
        transform.position += (velocity) * Time.fixedDeltaTime;
    }

    // WHILE Depth < MaxDepth
    //  Sweep(Bounce Position → Bounce Position + ?? Velocity)
    //  IF sweep hit
    //    Distance to hit surface = Normalize(?? Velocity)
    //    if (length(Distance to hit surface) < Skin Width)
    //    Remaining velocity = ?? Velocity - Distance to hit surface
    //    
    //  ELSE
    //    return ?? Velocity 


    private Vector3 Collide(Vector3 position, Vector3 velocity, int depth, bool gravityPass)
    {
        if (depth >= MaxSlideBounces)
            return Vector3.zero;
        
        float dist = velocity.magnitude + SkinWidth;

        RaycastHit hit;
        // float extents = Mathf.Min(bounds.extents.x, Mathf.Min(bounds.extents.y, bounds.extents.z));
        if (SweepCollider(position, velocity.normalized, out hit, dist))
        {
            print($"Collider swept. Dist: {hit.distance}");
            Vector3 snapToSurface = velocity.normalized * (hit.distance - SkinWidth);
            Vector3 leftover = velocity - snapToSurface;
            if (snapToSurface.magnitude <= SkinWidth)
                snapToSurface = Vector3.zero;

            float angle = Vector3.Angle(Vector3.up, hit.normal);
            if (angle < 55.0f)
            {
                if (gravityPass)
                {
                    return snapToSurface;
                }
                float mag = leftover.magnitude;
                leftover = Vector3.ProjectOnPlane(leftover, hit.normal).normalized;
                leftover *= mag;
            }
            return snapToSurface + Collide(leftover, position + snapToSurface, depth + 1, gravityPass);
        }
        return velocity;
    }

    // Generate a Fibonacci sphere, then map each point to the collider
    private void GenerateColliderPoints()
    {
        Collider collider = GetComponent<Collider>();
        Bounds bounds = collider.bounds;
        bounds.Expand(SkinWidth * -2.0f);
        float boundsMaxDim = Mathf.Max(bounds.size.x, Mathf.Max(bounds.size.y, bounds.size.z));

        Vector3[] points = new Vector3[CollisionPointCount];
        float phi = Mathf.PI * (Mathf.Sqrt(5.0f) - 1.0f);
        for (int i = 0; i < CollisionPointCount; i++)
        {
            float y = 1.0f - i / (CollisionPointCount - 1.0f) * 2.0f;
            float radius = Mathf.Sqrt(1.0f - Mathf.Pow(y, 2.0f));
            float theta = phi * i;
            float x = Mathf.Cos(theta) * radius;
            float z = Mathf.Sin(theta) * radius;
            // Scale well outside of the collider’s bounds
            // So our point isn’t inside the bounds and we don’t just
            // return the same point again
            Vector3 scaledPos = new Vector3(x, y, z) * boundsMaxDim * 2.0f;
            Vector3 projectedPos = bounds.ClosestPoint(scaledPos + bounds.center) - bounds.center;
            points[i] = projectedPos;
        }
        colliderPoints = points;
    }

    private bool SweepCollider(Vector3 origin, Vector3 direction, out RaycastHit hit, float maxDist)
    {
        RaycastHit closestHit = new RaycastHit();
        bool hasHit = false;
        foreach (Vector3 point in colliderPoints)
        {
            RaycastHit thisRaycast;
            bool thisHit = Physics.Raycast(origin + point, direction, out thisRaycast, maxDist);
            if (thisHit)
            {
                if (DebugDrawColliderPoints)
                {
                    Debug.DrawRay(origin + point, thisRaycast.point - (origin + point), Color.green);
                }
                if (!hasHit || thisRaycast.distance < closestHit.distance)
                {
                    closestHit = thisRaycast;
                }
            }
            hasHit = hasHit || thisHit;
        }
        hit = closestHit;
        return hasHit;
    }

    // private void DrawColliderPoints()
    // {
    //     foreach (Vector3 point in colliderPoints)
    //     {
    //         Vector3 dir = point.normalized;
    //         Debug.DrawRay(point + coll.bounds.center, dir * 0.3f, Color.green);
    //     }
    // }
}