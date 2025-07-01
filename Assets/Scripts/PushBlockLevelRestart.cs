using UnityEngine;

public class PushBlockLevelRestart : MonoBehaviour
{
    [SerializeField] public bool RestartOnPushOff = true;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();        
    }

    void Update()
    {
        if (Utils.IsFallingIntoVoid(rb) && RestartOnPushOff)
        {
            FindFirstObjectByType<LevelManager>().ReloadCurrentLevel();
        }
    }
}
