using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitButton : MonoBehaviour
{

    [SerializeField] private LevelManager levelManager;

    void OnTriggerEnter(Collider other)
    {
        levelManager.ActivateTrigger1();
    }
}

