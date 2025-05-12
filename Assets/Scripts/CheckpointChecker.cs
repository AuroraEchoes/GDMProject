using System.Collections;
using UnityEngine;

public class CheckpointChecker : MonoBehaviour
{
    private Renderer rend;
    public static Vector3 LightCatCheckpoint { get; private set; }
    public static Vector3 ShadowCatCheckpoint { get; private set; }
    public static bool HasCheckpoint { get; private set; }
    public static bool HasCheckpointTwo { get; private set; }
    public Color DeactivatedColor;
    public Color ActivatedColor;

    private bool triggerLightOnce = false;
    private bool triggerShadowOnce = false;

    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material.color = DeactivatedColor;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CatLight") && !triggerLightOnce)
        {
            LightCatCheckpoint = other.transform.position;
            triggerLightOnce = true;
            HasCheckpoint = true;
            StartCoroutine(TweenFlagColor());
        }
        else if (other.CompareTag("Shadow") && !triggerShadowOnce)
        {
            ShadowCatCheckpoint = other.transform.position;
            triggerShadowOnce = true;
            HasCheckpointTwo = true;
            StartCoroutine(TweenFlagColor());
        }
    }

    private IEnumerator TweenFlagColor()
    {
        float t = 0.0f;
        while (t < 0.2f)
        {
            t += Time.deltaTime;
            rend.material.color = Color.Lerp(DeactivatedColor, ActivatedColor, t);
            yield return null;
        }
        rend.material.color = ActivatedColor;
    }

    public static void ResetCheckpoints()
    {
        LightCatCheckpoint = Vector3.zero;
        ShadowCatCheckpoint = Vector3.zero;
        HasCheckpoint = false;
    }

    public enum Cat
    {
        Light,
        Shadow
    }
}