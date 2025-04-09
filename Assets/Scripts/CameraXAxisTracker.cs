using UnityEngine;

public class CameraXAxisTracker : MonoBehaviour
{
    [Header("Tracked Entities")]
    [SerializeField] private Transform entity1;
    [SerializeField] private Transform entity2;

    [Header("Camera Settings")]
    [SerializeField] private float padding = 1.0f; // Extra space on sides of view
    [SerializeField] private float smoothTime = 0.3f; // For smooth camera movement
    [SerializeField] private float minXPosition = float.MinValue; // Optional min bound
    [SerializeField] private float maxXPosition = float.MaxValue; // Optional max bound

    // Camera properties
    private Camera mainCamera;
    private float cameraHalfWidth;
    private Vector3 currentVelocity = Vector3.zero;

    private void Awake()
    {
        // Get the camera component
        mainCamera = GetComponent<Camera>();
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            Debug.LogWarning("Camera component not found on this GameObject. Using Camera.main instead.");
        }
    }

    private void Start()
    {
        // Calculate camera width in world units
        cameraHalfWidth = mainCamera.orthographicSize * mainCamera.aspect;
    }

    private void LateUpdate()
    {
        if (entity1 == null || entity2 == null)
        {
            Debug.LogWarning("One or both tracked entities are missing.");
            return;
        }

        // Get the left and right positions of the entities
        float leftEntityX = Mathf.Min(entity1.position.x, entity2.position.x);
        float rightEntityX = Mathf.Max(entity1.position.x, entity2.position.x);

        // Calculate the required camera position to keep both in view
        float entityDistance = rightEntityX - leftEntityX;
        float targetX = leftEntityX + (entityDistance / 2);

        // Check if the entities fit within the current view width
        float requiredHalfWidth = (entityDistance / 2) + padding;
        
        // If the entities fit within the camera view, move to the center point
        // No zoom adjustment happens here - we're only handling X positioning
        
        // Apply bounds if specified
        targetX = Mathf.Clamp(targetX, minXPosition, maxXPosition);

        // Create the target position (only changing X)
        Vector3 targetPosition = new Vector3(
            targetX,
            transform.position.y, // Keep current Y position
            transform.position.z  // Keep current Z position
        );

        // Smoothly move the camera
        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref currentVelocity,
            smoothTime
        );
    }

    // Optional method to visualize the camera bounds
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying || mainCamera == null) return;

        Vector3 cameraPos = transform.position;
        float halfWidth = cameraHalfWidth;
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(cameraPos, new Vector3(halfWidth * 2, mainCamera.orthographicSize * 2, 0.1f));
    }
}