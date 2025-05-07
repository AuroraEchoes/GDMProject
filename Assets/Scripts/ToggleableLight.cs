using System;
using System.Linq;
using UnityEngine;

public class ToggleableLight : ToggleableEntity
{

    [SerializeField] private float lightSpreadAngle;
    [SerializeField] private bool debugShowLightRays;
    [SerializeField] private Color lightBulbOnColor;
    private Color lightBulbDefaultColor;
    private Light toggleableLight;
    private ShadowCat shadowCat;
    private Material lightBulbMat;
    private bool isEffectingCat;
    private static bool shadowCatNotFoundDisplayed = false;
    private bool raycastHitsCat;
    private bool hitting => raycastHitsCat && toggleableLight.enabled;
    const int layerMask = ~(1 << 8);

    public override void Toggle()
    {
        toggleableLight.enabled = !toggleableLight.enabled;
        if (toggleableLight.enabled)
        {
            lightBulbMat.color = lightBulbOnColor;
        }
        else
        {
            lightBulbMat.color = lightBulbDefaultColor;
            if (isEffectingCat)
                shadowCat.Materialise();
        }
    }

    void Start()
    {
        if (shadowCat is null)
        {
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Shadow"))
            shadowCat ??= obj.GetComponent<ShadowCat>();
        }
 
        toggleableLight = GetComponent<Light>();
        toggleableLight.enabled = defaultState;
        lightBulbMat = GetComponent<MeshRenderer>().materials.First();
        lightBulbDefaultColor = lightBulbMat.color;
        lightBulbMat.color = toggleableLight.enabled ? lightBulbOnColor : lightBulbDefaultColor;

    }

    void Update()
    {
        if (debugShowLightRays) DebugShowLightRays();

        if (!hitting && isEffectingCat)
        {
            shadowCat.Materialise();
            isEffectingCat = false;
        }
        RaycastToCat();
    }

    private void DebugShowLightRays()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 100.0f, layerMask))
        {
            for (int i = 0; i < 36; i++)
            {
                float rotAngle = (i * 10.0f).ToRadians();
                float lightAngle = lightSpreadAngle.ToRadians();
                Vector3 dir = new Vector3(
                    Mathf.Cos(rotAngle) * Mathf.Sin(lightAngle),
                    -Mathf.Cos(lightAngle),
                    Mathf.Sin(rotAngle) * Mathf.Sin(lightAngle)
                ).normalized;
                Debug.DrawRay(transform.position, dir * hit.distance, Color.magenta);
            }
        }
        Vector3 lightToCat = shadowCat.transform.position - transform.position;
        Debug.DrawRay(transform.position, lightToCat * (lightToCat.magnitude + 2.0f), Color.cyan);
    }

    private void RaycastToCat()
    {
        if (!toggleableLight.enabled && !isEffectingCat)
        {
            return;
        }
        if (shadowCat == null)
        {
            if (!shadowCatNotFoundDisplayed)
            {
                Debug.LogWarning("Light: Could not find an entity with the tag “Shadow” and script ShadowCat");
                shadowCatNotFoundDisplayed = true;
            }
            return;
        }
        Vector3 lightToCat = shadowCat.transform.position - transform.position;
        Vector3 facingDirection = transform.rotation * Vector3.up;
        float angle = Vector3.Angle(facingDirection, lightToCat.normalized);
        RaycastHit hit;
        bool raycastHits = Physics.Raycast(transform.position, lightToCat, out hit, lightToCat.magnitude + 2.0f, layerMask);
        if (raycastHits)
        {
            if (hit.rigidbody is null) return;
            raycastHitsCat = hit.rigidbody.CompareTag("Shadow");
            if (!raycastHitsCat) return;
            if (angle <= lightSpreadAngle && !isEffectingCat)
            {
                isEffectingCat = true;
                shadowCat.Fade();
            }
            else if (angle > lightSpreadAngle && isEffectingCat)
            {
                isEffectingCat = false;
                shadowCat.Materialise();
            }
        }
    }
}
