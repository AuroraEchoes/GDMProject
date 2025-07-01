using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

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
    [SerializeField] private float minBorderPadding = 0.1f;
    [SerializeField] private float targetCatDistance = 1.3f;
    [SerializeField] private float transformSpeed = 1.0f;
    [SerializeField] private float scaleSpeed = 1.0f;
    [SerializeField] private float maxZoom = 1.5f;

    private Camera mainCamera;
    private float cameraHalfWidth;
    private Vector3 currentVelocity = Vector3.zero;
    private float currentZoomVelocity = 0.0f;
    private bool catsMissingWarnShown = false;

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
            if (!catsMissingWarnShown)
            {
                Debug.LogWarning("Cats missing");
                catsMissingWarnShown = true;
            }
            return;
        }

        float leftEntityX = Mathf.Min(entity1.position.x, entity2.position.x);
        float rightEntityX = Mathf.Max(entity1.position.x, entity2.position.x);

        float entityDistance = rightEntityX - leftEntityX;
        float targetX = leftEntityX + (entityDistance / 2);

        float requiredHalfWidth = (entityDistance / 2) + padding;

        Vector2 firstCatPos = CatClipPos(entity1);
        Vector2 secondCatPos = CatClipPos(entity2);

        // The idea here is that we draw a line between the two cats
        // We try to position the centre point of that line in the centre of the screen
        // And we scale to try and keep the length of the line equal to targetCatDistance

        Vector2 catLine = secondCatPos - firstCatPos;
        Vector2 midpoint = firstCatPos + (catLine.normalized * catLine.magnitude * 0.5f);
        float catDist = catLine.magnitude;
        float targetZoom = catDist - targetCatDistance;
        float targetSize = Mathf.Max(mainCamera.orthographicSize + targetZoom * 10.0f, maxZoom);
        mainCamera.orthographicSize = Mathf.SmoothDamp(mainCamera.orthographicSize, targetSize, ref currentZoomVelocity, smoothTime);
        Vector2 fromIso = FromIsometricDirection(midpoint);
        Vector3 firstCatScreenPos = CatScreenPos(entity1);
        Vector3 secondCatScreenPos = CatScreenPos(entity2);
        Vector3 catLineScreen = secondCatScreenPos - firstCatScreenPos;
        Vector3 midpointScreen = firstCatScreenPos + (catLineScreen.normalized * catLineScreen.magnitude * 0.5f) + Vector3.back * 30.0f;
        Vector3 midpointWorld = mainCamera.ScreenToWorldPoint(midpointScreen);
        transform.position = Vector3.SmoothDamp(transform.position, midpointWorld, ref currentVelocity, smoothTime);
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying || mainCamera == null) return;

        Vector3 cameraPos = transform.position;
        float halfWidth = cameraHalfWidth;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(cameraPos, new Vector3(halfWidth * 2, mainCamera.orthographicSize * 2, 0.1f));
    }

    private Vector3 CatScreenPos(Transform cat)
    {
        Vector3 catPos = cat.position;
        Vector3 catScreenPos = mainCamera.WorldToScreenPoint(catPos);
        return catScreenPos;
    }

    private Vector2 CatClipPos(Transform cat)
    {
        Vector3 catPos = cat.position;
        Vector3 catScreenPos = mainCamera.WorldToScreenPoint(catPos);
        Vector2 catPosClip = new Vector2(catScreenPos.x, catScreenPos.y) / new Vector2(Screen.width, Screen.height) * 2.0f - new Vector2(1.0f, 1.0f);
        return catPosClip;
    }

    private Vector2 FromIsometricDirection(Vector2 baseDir)
    {
        Matrix4x4 toIso = Matrix4x4.Rotate(Quaternion.Euler(0.0f, 45.0f, 0.0f));
        Vector3 isoVec3 = toIso.inverse.MultiplyPoint3x4(new Vector3(baseDir.x, 0.0f, baseDir.y));
        return new Vector2(isoVec3.x, isoVec3.z).normalized;
    }
}