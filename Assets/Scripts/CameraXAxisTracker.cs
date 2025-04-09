using UnityEngine;

public class CameraXAxisTracker : MonoBehaviour
{
    [Header("Tracked Entities")]
    [SerializeField] private Transform entity1;
    [SerializeField] private Transform entity2;

    [Header("Camera Settings")]
    [SerializeField] private float padding = 1.0f;
    [SerializeField] private float smoothTime = 0.3f;
    [SerializeField] private float minXPosition = float.MinValue;
    [SerializeField] private float maxXPosition = float.MaxValue;

    private Camera mainCamera;
    private float cameraHalfWidth;
    private Vector3 currentVelocity = Vector3.zero;

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            Debug.LogWarning("Camera not found");
        }
    }

    private void Start()
    {
        cameraHalfWidth = mainCamera.orthographicSize * mainCamera.aspect;
    }

    private void LateUpdate()
    {
        if (entity1 == null || entity2 == null)
        {
            Debug.LogWarning("Cats missing");
            return;
        }

        float leftEntityX = Mathf.Min(entity1.position.x, entity2.position.x);
        float rightEntityX = Mathf.Max(entity1.position.x, entity2.position.x);

        float entityDistance = rightEntityX - leftEntityX;
        float targetX = leftEntityX + (entityDistance / 2);

        float requiredHalfWidth = (entityDistance / 2) + padding;
        
        targetX = Mathf.Clamp(targetX, minXPosition, maxXPosition);

        Vector3 targetPosition = new Vector3(
            targetX,
            transform.position.y,
            transform.position.z
        );

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref currentVelocity,
            smoothTime
        );
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying || mainCamera == null) return;

        Vector3 cameraPos = transform.position;
        float halfWidth = cameraHalfWidth;
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(cameraPos, new Vector3(halfWidth * 2, mainCamera.orthographicSize * 2, 0.1f));
    }
}