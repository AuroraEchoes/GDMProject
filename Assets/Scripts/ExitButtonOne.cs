using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitButton : MonoBehaviour
{
    [SerializeField] private string gameLevelMenu = "MainMenu";
    [SerializeField] private LevelManager levelManager;

 

    void OnTriggerEnter(Collider other)
    {
        levelManager.ActivateTrigger1();
    }
}