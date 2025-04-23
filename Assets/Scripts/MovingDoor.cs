using UnityEngine;

public class MovingDoor : ToggleableEntity
{
    [SerializeField] private float moveDistance = 5f;
    [SerializeField] private float xMoveDistance = 0.0f;
    [SerializeField] private float zMoveDistance = 0.0f;
    [SerializeField] private float moveSpeed = 2f;

    private Vector3 originalPosition;
    private Vector3 targetPosition;
    private bool isRaised = false;

    private void Start()
    {
        originalPosition = transform.position;
        targetPosition = originalPosition;
    }

    public override void Toggle()
    {
        isRaised = !isRaised;
        targetPosition = isRaised 
            ? originalPosition + new Vector3(xMoveDistance, moveDistance, zMoveDistance)
            : originalPosition;
    }

    private void Update()
    {

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }
}