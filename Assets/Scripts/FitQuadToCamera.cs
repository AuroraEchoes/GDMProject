using UnityEngine;

public class FitQuadToCamera : MonoBehaviour
{
    void Start()
    {
        Camera cam = Camera.main;
        float distance = transform.localPosition.z;
        float height = 2f * distance * Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
        float width = height * cam.aspect;
        
        transform.localScale = new Vector3(width, height, 1f);
    }
}