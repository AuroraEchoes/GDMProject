using UnityEngine;

public class MovingDoor : ToggleableEntity
{
    [SerializeField] private float moveDistance = 5f;
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
        targetPosition = isRaised ? originalPosition + Vector3.up * moveDistance : originalPosition;
    }

    private void Update()
    {

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }
}