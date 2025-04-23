using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class ShadowCat : MonoBehaviour
{
    [SerializeField] private float fadeTime = 1.0f;
    private Renderer rend;
    private float fadeCompletion;
    private bool fading;
    private bool materialising;
    private float lastFadeRepeatTime;

    public void Fade()
    {
        fading = true;
        fadeCompletion = 0.0f;
        lastFadeRepeatTime = Time.time;
    }

    public void Materialise()
    {
        materialising = true;
        fadeCompletion = 0.0f;
    }

    void Start()
    {
        rend = GetComponentInChildren<Renderer>();    
    }

    void Update()
    {
        if (Mathf.Approximately(rend.material.color.a, 0.0f) && !fading)
        {
            Materialise();
        }
        if (fading || materialising)        
        {
            fadeCompletion += Time.deltaTime / fadeTime;
            Material mat = rend.material;
            Color matColor = mat.color;
            matColor.a = fading ? 1.0f - fadeCompletion : fadeCompletion;
            mat.color = matColor;
            fading = fading && fadeCompletion < 1.0f;
            materialising = materialising && fadeCompletion < 1.0f;
        }
    }
}
