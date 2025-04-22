using UnityEngine;

public class ColorZoneTrigger : MonoBehaviour
{
    public GameObject correctBox;
    public string ZoneName;
    private ColorPuzzle puzzleManager;
    void Start()
    {
        puzzleManager = FindObjectOfType<ColorPuzzle>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == correctBox)
        {
            puzzleManager.SetBoxState(ZoneName, true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == correctBox)
        {
            puzzleManager.SetBoxState(ZoneName, false);
        }
    }
}



