using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] private List<ToggleableEntity> toggleOnPress = new List<ToggleableEntity>();
    private BoxCollider pressTrigger;

    void Start()
    {
        pressTrigger = GetComponent<BoxCollider>();
    }

    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        // TODO: Maybe use tags instead
        if (other.gameObject.GetComponent<ControllableCharacter>())
            ToggleChildren();
    }

    void OnTriggerExit(Collider other)
    {
        // TODO: Maybe use tags instead
        if (other.gameObject.GetComponent<ControllableCharacter>())
            ToggleChildren();
    }

    private void ToggleChildren()
    {
        foreach (ToggleableEntity entity in toggleOnPress)
            entity.Toggle();
    }
}
