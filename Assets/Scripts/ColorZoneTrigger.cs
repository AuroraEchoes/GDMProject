using UnityEngine;

public class ColorZoneTrigger : MonoBehaviour
{
    public GameObject correctBox;
    public string ZoneName;
    private ColorPuzzle puzzleManager;
    void Start()
    {
        puzzleManager = FindObjectOfType<ColorPuzzle>();
        Debug.Log($"{ZoneName} expects: {correctBox.name}");
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{other.gameObject.name} entered {ZoneName} zone");
        if (other.gameObject == correctBox)
        {
            Debug.Log($"{ZoneName}: Correct box entered.");
            puzzleManager.SetBoxState(ZoneName, true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log($"{other.gameObject.name} exited {ZoneName} zone");
        if (other.gameObject == correctBox)
        {
            Debug.Log($"{ZoneName}: Correct box exited.");
            puzzleManager.SetBoxState(ZoneName, false);
        }
    }
}



