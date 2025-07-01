using UnityEngine;

public class WayfindingStars : MonoBehaviour
{
    private MovementController movementController;
    private RectTransform rectTransform;
    private SpriteRenderer[] children;
    [SerializeField] private float fadeInTime = 0.5f;
    private float timeFading;
    private bool show = false;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        movementController = FindFirstObjectByType<MovementController>();
        movementController.MovementCalibratedEvent += ShowWayfindingStars;
        children = GetComponentsInChildren<SpriteRenderer>();
        timeFading = fadeInTime;
        SetChildOpacity(0.0f);
    }

    void Update()
    {
        if (show && timeFading > 0.0f)
        {
            timeFading -= Time.deltaTime;
            float alpha = 1.0f - (timeFading / fadeInTime);
            SetChildOpacity(alpha);
        }
    }

    void ShowWayfindingStars()
    {
        movementController.MovementCalibratedEvent -= ShowWayfindingStars;
        show = true;
        if (MovementController.UseIsometricControls)
        {
            rectTransform.Rotate(new Vector3(90.0f, 45.0f, 0.0f));
        }
        else
        {
            rectTransform.Rotate(new Vector3(90.0f, 90.0f, 0.0f));
        }
    }

    void SetChildOpacity(float alpha)
    {
        foreach (SpriteRenderer child in children)
        {
            Color color = child.color;
            color.a = alpha;
            child.color = color;
        }
    }
}