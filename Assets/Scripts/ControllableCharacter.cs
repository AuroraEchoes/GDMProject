using UnityEngine;

public class ControllableCharacter : MonoBehaviour
{
    public ControllableEntityParams Params;
    private Vector2 moveDir;
    private bool accelerating = false;
    private float velocity;
    private Vector3 lastMove;

    private Rigidbody rb;

    public void SetInputAxes(Vector2 input)
    {
        accelerating = !input.Equals(Vector2.zero);
        if (accelerating)
            moveDir = input.normalized;
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
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Pushable"))
        {
            Vector3 moveDir = lastMove.normalized;
            Vector3 blockDir = rb.position - collision.rigidbody.position;
            float angle = Vector3.Angle(moveDir, blockDir);
            if (angle < 75.0f)
                collision.rigidbody.MovePosition(collision.rigidbody.position + lastMove * (Time.fixedDeltaTime + 0.05f));
        }
    }
}