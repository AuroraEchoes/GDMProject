using UnityEngine;

public class HiddenTutorial : MonoBehaviour
{

    public GameObject boxPrefab;

    private bool hasTriggered = false;

    void OnTriggerExit(Collider other)
    {
        if (!hasTriggered)
        {
            hasTriggered = true;
            Instantiate(boxPrefab, new Vector3(-12.20428f, 5.43f, -8.23f), Quaternion.identity);
        }


    }


}
