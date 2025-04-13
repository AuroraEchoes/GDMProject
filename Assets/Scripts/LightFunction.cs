using UnityEngine;
using UnityEngine.Timeline;

public class LightFunction : MonoBehaviour
{
   

    [SerializeField] private Light lightSource;
    [SerializeField] private Collider blockerCollider;

    void Update()
    {
        
        blockerCollider.enabled = lightSource.enabled;
    }
}
