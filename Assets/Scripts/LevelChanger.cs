using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private string gameLevelOne = "Level1";
    [SerializeField] private string gameLevelTwo = "Level2";
    [SerializeField] private string gameLevelThree = "Level3";
    [SerializeField] private string gameLevelFour = "Level4";
    [SerializeField] private string gameLevelTutorial = "DesignInitalTest";
    [SerializeField] private string gameLevelMenu = "MainMenu";
    [SerializeField] private string gameIntroCutscene = "CutScene";


    private bool trigger1Activated = false;
    private bool trigger2Activated = false;


    public void ActivateTrigger1()
    {
        trigger1Activated = true;
        CheckBothTriggers();
    }

    public void ActivateTrigger2()
    {
        trigger2Activated = true;
        CheckBothTriggers();
    }

    private void CheckBothTriggers()
    {
        if (trigger1Activated && trigger2Activated)
        {
            SceneManager.LoadScene(gameLevelMenu);
        }
    }





    public void GameButtonLvl1()
    {
        SceneManager.LoadScene(gameLevelOne);
    }

    public void GameButtonLvl3()
    {
        SceneManager.LoadScene(gameLevelThree);
    }


    public void GameButtonLvl4()
    {
        SceneManager.LoadScene(gameLevelFour);
    }


    public void GameButtonLvlTut()
    {
        SceneManager.LoadScene(gameLevelTutorial);
    }

    public void GameButtonMenu()
    {
        SceneManager.LoadScene(gameLevelMenu);
    }
    
    public void GameButtonIntroCutScene()
    {
        SceneManager.LoadScene(gameIntroCutscene);
    }
}