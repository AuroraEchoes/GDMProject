using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour
{

    [SerializeField] private string gameLevelOne = "Level1";
    [SerializeField] private string gameLevelTwo = "Level2";
    [SerializeField] private string gameLevelThree = "Level3";
    [SerializeField] private string gameLevelFour = "Level4";
    [SerializeField] private string gameLevelTutorial = "Tutorial";
    [SerializeField] private string gameLevelMenu = "MainMenu";

    public void GameButtonLvl1()
    {
        SceneManager.LoadScene(gameLevelOne);

    }

    public void GameButtonLvl3()
    {
        SceneManager.LoadScene(gameLevelThree);

    }

    public void GameButtonLvlTut()
    {
        SceneManager.LoadScene(gameLevelTutorial);

    }

    public void GameButtonMenu()
    {
        SceneManager.LoadScene(gameLevelMenu);
    }






}


