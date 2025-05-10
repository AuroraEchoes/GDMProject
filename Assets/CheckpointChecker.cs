using UnityEngine;

public class CheckpointChecker : MonoBehaviour
{
    // Static variables to persist across scene reloads
    public static Vector3 LightCatCheckpoint { get; private set; }
    public static Vector3 ShadowCatCheckpoint { get; private set; }
    public static bool HasCheckpoint { get; private set; }
    public static bool HasCheckpointTwo { get; private set; }

    


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Light"))
        {
            LightCatCheckpoint = other.transform.position;
            HasCheckpoint = true;
            Debug.Log("Light checkpoint set at: " + LightCatCheckpoint);
        }
        else if (other.CompareTag("Shadow"))
        {
            ShadowCatCheckpoint = other.transform.position;
            HasCheckpointTwo = true;
            Debug.Log("Shadow checkpoint set at: " + ShadowCatCheckpoint);
        }
    }

    public static void ResetCheckpoints()
    {
        LightCatCheckpoint = Vector3.zero;
        ShadowCatCheckpoint = Vector3.zero;
        HasCheckpoint = false;
    }
}