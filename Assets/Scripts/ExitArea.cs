using UnityEngine;

public class ExitArea : MonoBehaviour
{
    [SerializeField] private Cat catToTriggerFor;
    [SerializeField] private LevelManager levelManager;

    void Start()
    {
        if (levelManager is null)
        {
            Debug.LogWarning("ExitArea does not have a LevelManager set");
        }
    }

    bool CatTriggers(Rigidbody rigidbody)
    {
        return (rigidbody.CompareTag("Shadow") && catToTriggerFor.Equals(Cat.Shadow))
            || ((rigidbody.CompareTag("CatLight") || rigidbody.CompareTag("Light")) && catToTriggerFor.Equals(Cat.Light));
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody is null) return;
        if (CatTriggers(other.attachedRigidbody))
            if (catToTriggerFor == Cat.Light)
                levelManager.ActivateTrigger1();
            else
                levelManager.ActivateTrigger2();
    }

    public enum Cat
    {
        Shadow,
        Light
    }
}
