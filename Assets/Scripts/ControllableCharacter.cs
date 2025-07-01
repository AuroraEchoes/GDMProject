using UnityEngine;
using UnityEngine.Rendering;

public class ControllableCharacter : MonoBehaviour
{
    public ControllableEntityParams Params;
    private Animator anim;

    private Vector2 moveDir;
    private bool accelerating = false;
    private float velocity;
    private Vector3 lastMove;
    public MovementController MovementController;
    [SerializeField] float stepClimbSpeed = 2f;
    [SerializeField] private GameObject stairBottom;
    [SerializeField] private GameObject stairTop;

    public Rigidbody rb;

    public void SetInputAxes(Vector2 input)
    {
        accelerating = !input.Equals(Vector2.zero);
        if (accelerating)
        {
            Vector2 baseMoveDir = input.Rotate(MovementController.ForwardDirection.Angle()).normalized;
            if (MovementController.UseIsometricControls)
                moveDir = ToIsometricDirection(input);
            else
                moveDir = baseMoveDir;
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = gameObject.GetComponentInChildren<Animator>();

    }

    void Update()
    {
        // Rotate cat to look in moving direction (VERY CRUDE)
        if (!moveDir.Equals(Vector2.zero))
        {
            float totalRotation = Vector2.SignedAngle(transform.rotation.eulerAngles.y.ToVector2(), moveDir);
            float rotDelta = Mathf.MoveTowards(0, totalRotation, 400.0f * Time.deltaTime);
            transform.Rotate(0.0f, -rotDelta, 0.0f);
        }
    }

    void FixedUpdate()
    {
        Vector3 movement = Vector3.zero;
        float velocityDelta = (accelerating ? Params.Acceleration : -Params.Acceleration) * Time.fixedDeltaTime;
        velocity = Mathf.Clamp(velocity + velocityDelta, 0, Params.MaxVelocity);
        Vector3 walk = moveDir.ToVector3XZ() * velocity;
        movement += walk;
        lastMove = movement;

        if (movement.magnitude > 0.01f)
        {
            anim.SetInteger("AnimationPar", 1);  // Run animation
        }
        else
        {
            anim.SetInteger("AnimationPar", 0);  // Idle animation
        }

        rb.MovePosition(rb.position + lastMove * Time.fixedDeltaTime);
        ClimbStairs();
        PushBox();
    }

    void ClimbStairs()
    {
        bool lowerHit = Physics.Raycast
        (
            stairBottom.transform.position,
            transform.TransformDirection(-Params.ForwardDirection),
            0.05f
        );
        if (lowerHit)
        {
            bool upperMissed = !Physics.Raycast(
                stairTop.transform.position,
                transform.TransformDirection(-Params.ForwardDirection),
                0.1f
            );
            if (upperMissed)
            {
                rb.MovePosition(rb.position + Vector3.up * Time.deltaTime * stepClimbSpeed);
            }
        }
    }

    void PushBox()
    {
        RaycastHit lowerRaycast;
        bool lowerHit = Physics.Raycast
        (
            stairBottom.transform.position,
            transform.TransformDirection(-Params.ForwardDirection),
            out lowerRaycast,
            0.05f
        );
        if (lowerRaycast.rigidbody is null) return;
        bool hitBox = lowerRaycast.rigidbody.CompareTag("Pushable");
        if (lowerHit && hitBox)
        {
            lowerRaycast.rigidbody.AddForce(lastMove * Time.fixedDeltaTime);
        }
    }

    private Vector2 IsometriseInputDirection(Vector2 baseDir)
    {
        // Vector2 ihat = new Vector2(1.0f, -1.0f);
        // Vector2 jhat = new Vector2(0.5f, 0.5f);
        // // Unity, why do you not have a 2x2 matrix??
        // Vector2 result = new Vector2(baseDir.x * ihat.x + baseDir.y * ihat.y, baseDir.x * jhat.x + baseDir.y * jhat.y);
        // return result;
        baseDir.y *= 0.5f;
        return baseDir;
    }

    private Vector2 ToIsometricDirection(Vector2 baseDir)
    {
        Matrix4x4 toIso = Matrix4x4.Rotate(Quaternion.Euler(0.0f, 45.0f, 0.0f));
        Vector3 isoVec3 = toIso.MultiplyPoint3x4(new Vector3(baseDir.x, 0.0f, baseDir.y));
        return new Vector2(isoVec3.x, isoVec3.z).normalized;
    }
}