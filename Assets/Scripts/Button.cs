using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] private List<ToggleableEntity> toggleOnPress = new List<ToggleableEntity>();

    void OnTriggerEnter(Collider other)
    {
        TriggerCollide(other);
    }

    void OnTriggerExit(Collider other)
    {
        TriggerCollide(other);
    }

    private void TriggerCollide(Collider other)
    {
        foreach (ToggleableEntity entity in toggleOnPress)
            entity.Toggle();
    }
}
