using UnityEngine;

public class FinalLightHack : MonoBehaviour
{
    bool dimmed = false;
    [SerializeField] private Light actualLight;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Pushable"))
            dimmed = true;
    }

    void Update()
    {
        if (dimmed)
        {
            actualLight.intensity = Mathf.MoveTowards(actualLight.intensity, 30.0f, 60.0f * Time.deltaTime);
        }
    }
}
