using System.Collections;
using UnityEngine;

public class HiddenTutorial : MonoBehaviour
{
    private bool hasTriggered = false;

    public GameObject boxPrefab;

    void OnTriggerExit(Collider other)
    {
        if (!hasTriggered)
        {
            hasTriggered = true;
            GameObject instance = Instantiate(boxPrefab, new Vector3(-12.20428f, 5.43f, -8.23f), Quaternion.identity);
            instance.GetComponent<PushBlockLevelRestart>().RestartOnPushOff = false;
            StartCoroutine(RotationLockHack(instance.GetComponent<Rigidbody>()));
        }
    }

    IEnumerator RotationLockHack(Rigidbody rb)
    {
        yield return new WaitForSeconds(4.0f);
        rb.freezeRotation = true;

    }
}
