using System;
using UnityEngine;

public class ToggleableLight : ToggleableEntity
{
    private Light toggleableLight;

    public override void Toggle()
    {
        toggleableLight.enabled = !toggleableLight.enabled;
    }

    void Start()
    {
        toggleableLight = GetComponent<Light>();
    }

    void Update()
    {
        
    }
}
