using System;
using UnityEngine;

public class ControllableCharacter : MonoBehaviour
{
    public Vector2 InputAxes;
    [SerializeField] private float maxSpeed = 1.0f;
    [SerializeField] private float acceleration = 1.0f;
    [SerializeField] private float deceleration = 1.0f;
    private Vector3 velocity;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (!InputAxes.Equals(Vector2.zero))
            velocity = new Vector3(InputAxes.x, 0, InputAxes.y) * Time.deltaTime * maxSpeed;
        rb.MovePosition(transform.position + velocity);
        // transform.position += velocity * Time.deltaTime;
    }
}
