using UnityEngine;

public class CheckpointChecker : MonoBehaviour
{

    public static Vector3 LightCatCheckpoint { get; private set; }
    public static Vector3 ShadowCatCheckpoint { get; private set; }
    public static bool HasCheckpoint { get; private set; }
    public static bool HasCheckpointTwo { get; private set; }


    private bool triggerLightOnce = false;
    private bool triggerShadowOnce = false;



    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CatLight") && !triggerLightOnce)
        {
            LightCatCheckpoint = other.transform.position;
            triggerLightOnce = true;
            HasCheckpoint = true;
            Debug.Log("Light kitty is at " + LightCatCheckpoint);
        }
        else if (other.CompareTag("Shadow") && !triggerShadowOnce)
        {
            ShadowCatCheckpoint = other.transform.position;
            triggerShadowOnce = true;
            HasCheckpointTwo = true;
            Debug.Log("Shadow Kitty is at " + ShadowCatCheckpoint);
        }

    }





    public static void ResetCheckpoints()
    {
        LightCatCheckpoint = Vector3.zero;
        ShadowCatCheckpoint = Vector3.zero;
        HasCheckpoint = false;
    }
}