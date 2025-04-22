using UnityEngine;

public class ControllableCharacter : MonoBehaviour
{
    public ControllableEntityParams Params;
    private Vector2 moveDir;
    private bool accelerating = false;
    private float velocity;
    private Vector3 lastMove;
    public Vector2 ForwardDirection;
    [SerializeField] float stepClimbSpeed = 2f;
    [SerializeField] GameObject stairBottom;
    [SerializeField] GameObject stairTop;

    private Rigidbody rb;

    public void SetInputAxes(Vector2 input)
    {
        accelerating = !input.Equals(Vector2.zero);
        if (accelerating)
            moveDir = input.Rotate(ForwardDirection.Angle()).normalized;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
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
        float velocityDelta = (accelerating ? Params.Acceleration : -Params.Acceleration) * Time.fixedDeltaTime;
        velocity = Mathf.Clamp(velocity + velocityDelta, 0, Params.MaxVelocity);
        Vector3 walk = moveDir.ToVector3XZ() * velocity;
        movement += walk;
        lastMove = movement;
        rb.MovePosition(rb.position + lastMove * Time.fixedDeltaTime);
        ClimbStairs();
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
}