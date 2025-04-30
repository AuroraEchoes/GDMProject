using UnityEngine;

public class ZoneTrigger : MonoBehaviour
{
   
    public string zoneName;
    private BoxGatePuzzle puzzleManager;

    private void Start()
    {
        puzzleManager = FindObjectOfType<BoxGatePuzzle>();

    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Pushable"))
        {
        
            puzzleManager.SetZoneState(zoneName, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pushable"))
        {
       
            puzzleManager.SetZoneState(zoneName, false);
        }
    }
}