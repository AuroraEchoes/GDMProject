using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class ShadowCat : MonoBehaviour
{
    [SerializeField] private float fadeTime = 0.8f;
    private Renderer rend;
    private float fadeCompletion;
    private bool fading;
    private bool materialising;
    public bool Faded => rend.material.color.a < 0.8f;
    private Rigidbody rb;
    private bool colliding;
    private Transform otherTransform;

    public void Fade()
    {
        fading = true;
        materialising = false;
        fadeCompletion = 0.0f;
    }

    public void Materialise()
    {
        materialising = true;
        fading = false;
        fadeCompletion = 0.0f;
    }

    void Start()
    {
        rend = GetComponentInChildren<Renderer>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
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

    void FixedUpdate()
    {
        if (Faded)
        {
            RaycastHit hit;
            bool didHit = rb.SweepTest
            (
                transform.forward,
                out hit,
                rb.linearVelocity.magnitude + 1.0f
            );
            if (didHit)
            {
                rb.linearVelocity = Vector3.zero;
                rb.constraints = RigidbodyConstraints.FreezeAll;
            }
            else
            {
                rb.constraints = RigidbodyConstraints.FreezeRotation;
            }
        }
        else
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

}