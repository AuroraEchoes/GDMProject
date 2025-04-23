using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] private List<ToggleableEntity> toggleOnPress = new List<ToggleableEntity>();
    [SerializeField] private float buttonMoveSpeed = 1.0f;
    private GameObject buttonChild;
    private float targetHeight;

    void Start()
    {
        buttonChild = transform.GetChild(0).gameObject;
    }

    void FixedUpdate()
    {
        float height = buttonChild.transform.localPosition.y;
        float delta = Mathf.MoveTowards(height, targetHeight, buttonMoveSpeed * Time.fixedDeltaTime);
        buttonChild.transform.localPosition = Vector3.up * delta;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Light") || other.CompareTag("Shadow") || other.CompareTag("Pushable"))
        {
            TriggerCollide(other);
            targetHeight = -0.02f;
        }
    }

    void OnTriggerExit(Collider other)
    {
        TriggerCollide(other);
        targetHeight = 0.0f;
    }

    private void TriggerCollide(Collider other)
    {
        foreach (ToggleableEntity entity in toggleOnPress)
            entity.Toggle();
    }
}
