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
            || (rigidbody.CompareTag("Light") && catToTriggerFor.Equals(Cat.Light));
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody is null) return;
        if (CatTriggers(collision.rigidbody))
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
