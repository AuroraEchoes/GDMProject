using UnityEngine;

public abstract class ToggleableEntity : MonoBehaviour
{
    public bool defaultState = false;
    public abstract void Toggle();
}
