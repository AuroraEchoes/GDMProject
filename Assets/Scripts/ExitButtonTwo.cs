using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitButtonTwo : MonoBehaviour
{
    [SerializeField] private string gameLevelMenu = "MainMenu";
    [SerializeField] private LevelManager levelManager;



    void OnTriggerEnter(Collider other)
    {
        levelManager.ActivateTrigger2();
    }
}