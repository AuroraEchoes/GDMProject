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
            actualLight.range = Mathf.MoveTowards(actualLight.range, 5.0f, 60.0f * Time.deltaTime);
            actualLight.intensity = Mathf.MoveTowards(actualLight.intensity, 50.0f, 60.0f * Time.deltaTime);
        }
    }
}
