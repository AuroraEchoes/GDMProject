using UnityEngine;

public class ControllableCharacter : MonoBehaviour
{
    public ControllableEntityParams Params;
    private Animator anim;

    private Vector2 moveDir;
    private bool accelerating = false;
    private float velocity;
    private Vector3 lastMove;
    public Vector2 ForwardDirection;
    [SerializeField] float stepClimbSpeed = 2f;
    [SerializeField] private GameObject stairBottom;
    [SerializeField] private GameObject stairTop;

    private Rigidbody rb;

    public void SetInputAxes(Vector2 input)
    {
        accelerating = !input.Equals(Vector2.zero);
        if (accelerating)
            moveDir = input.Rotate(ForwardDirection.Angle()).normalized;
    }

    public bool IsFallingIntoVoid()
    {
        // Two conditions here:
        // - Raycast downwards finds nothing
        // - Downwards velocity is high
        bool nothingBelow = !Physics.Raycast(transform.position + new Vector3(0.0f, 0.2f, 0.0f), Vector3.down, 100.0f);
        bool falling = rb.linearVelocity.y < -10.0f;
        return nothingBelow && falling;
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
            transform.rotation = Quaternion.LookRotation(moveDir.Rotate(0).ToVector3XZ());
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
}