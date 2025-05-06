using UnityEngine;

public class HiddenTutorial : MonoBehaviour
{

    public GameObject boxPrefab;

    bool triggeronce = true;

    void OnTriggerExit(Collider other)
    {
        if (triggeronce)
        {
            Debug.Log("Triggered");
            Instantiate(boxPrefab, new Vector3(-12.20428f, 5.43f, -8.23f), Quaternion.identity);
            triggeronce = false;
        }


    }


}
