using System;
using UnityEngine;

public class ToggleableLight : ToggleableEntity
{

    [SerializeField] private float lightSpreadAngle;
    private Light toggleableLight;
    private ShadowCat shadowCat;
    private bool isEffectingCat;
    private static bool shadowCatNotFoundDisplayed = false;

    public override void Toggle()
    {
        toggleableLight.enabled = !toggleableLight.enabled;
        shadowCat = GameObject.FindWithTag("Shadow").GetComponent<ShadowCat>();
    }

    void Start()
    {
        toggleableLight = GetComponent<Light>();
        toggleableLight.enabled = defaultState;
    }

    void Update()
    {
        RaycastToCat();
    }

    private void RaycastToCat()
    {
        if (shadowCat == null)
        {
            if (!shadowCatNotFoundDisplayed)
            {
                Debug.LogWarning("Light: Could not find an entity with the tag “Shadow” and script ShadowCat");
                shadowCatNotFoundDisplayed = true;
            }
            return;
        }
        Vector3 lightToCat = (shadowCat.transform.position - transform.position).normalized;
        // RaycastHit castToShadow;
        // bool hitShadow = Physics.Raycast(transform.position, lightToCat, out castToShadow, 20.0f);
        // if (!hitShadow) return;
        Vector3 facingDirection = transform.rotation * Vector3.up;
        float angle = Vector3.Angle(facingDirection, lightToCat);
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
