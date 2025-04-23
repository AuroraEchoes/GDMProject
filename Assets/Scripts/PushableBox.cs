using UnityEngine;

public class PushableBox : MonoBehaviour
{
    [SerializeField] private GameObject stairBottom;
    [SerializeField] private GameObject stairTop;
    [SerializeField] private float stepClimbSpeed;
    private Rigidbody rb;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        ClimbStairs();
    }

    void ClimbStairs()
    {
        bool lowerHit = Physics.Raycast
        (
            stairBottom.transform.position,
            rb.linearVelocity,
            0.55f
        );
        if (lowerHit)
        {
            bool upperMissed = !Physics.Raycast(
                stairTop.transform.position, 
                rb.linearVelocity,
                0.6f
            );
            if (upperMissed)
            {
                rb.MovePosition(rb.position + Vector3.up * Time.deltaTime * stepClimbSpeed);
            }
        }
    }
}