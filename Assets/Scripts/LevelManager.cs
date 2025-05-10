using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public Level CurrentLevel;
    [SerializeField] private string gameLevelOne = "Level1";
    [SerializeField] private string gameLevelTwo = "Level2";
    [SerializeField] private string gameLevelThree = "Level3";
    [SerializeField] private string gameLevelFour = "Level4";
    [SerializeField] private string gameLevelTutorial = "DesignInitalTest";
    [SerializeField] private string gameLevelMenu = "MainMenu";
    [SerializeField] private string gameIntroCutscene = "CutScene";

    private bool trigger1Activated = false;
    private bool trigger2Activated = false;

    private void Start()
    {

        if (CheckpointChecker.HasCheckpoint && CheckpointChecker.HasCheckpoint)
        {
            RestoreCharactersToCheckpoints();
        }
    }

    private void RestoreCharactersToCheckpoints()
    {
        GameObject[] lightCats = GameObject.FindGameObjectsWithTag("Light");
        GameObject[] shadowCats = GameObject.FindGameObjectsWithTag("Shadow");

        foreach (var lightCat in lightCats)
        {
            if (CheckpointChecker.LightCatCheckpoint != Vector3.zero)
            {
                lightCat.transform.position = CheckpointChecker.LightCatCheckpoint;
                Debug.Log("Restored Light Cat to checkpoint");
            }
        }

        foreach (var shadowCat in shadowCats)
        {
            if (CheckpointChecker.ShadowCatCheckpoint != Vector3.zero)
            {
                shadowCat.transform.position = CheckpointChecker.ShadowCatCheckpoint;
                Debug.Log("Restored Shadow Cat to checkpoint");
            }
        }
    }

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

    public void ReloadCurrentLevel()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadLevel(Level level)
    {

        if (level != CurrentLevel)
        {
            CheckpointChecker.ResetCheckpoints();
        }

        CurrentLevel = level;

        switch (level)
        {
            case Level.Tutorial:
                SceneManager.LoadScene(gameLevelTutorial);
                break;
            case Level.Level1:
                SceneManager.LoadScene(gameLevelOne);
                break;
            case Level.Level2:
                SceneManager.LoadScene(gameLevelTwo);
                break;
            case Level.Level3:
                SceneManager.LoadScene(gameLevelThree);
                break;
            case Level.Level4:
                SceneManager.LoadScene(gameLevelFour);
                break;
        }
    }

    private void CheckBothTriggers()
    {
        if (trigger1Activated && trigger2Activated)
        {
            switch (CurrentLevel)
            {
                case Level.Tutorial:
                    LoadLevel(Level.Level1);
                    break;
                case Level.Level1:
                    LoadLevel(Level.Level2);
                    break;
                case Level.Level2:
                    LoadLevel(Level.Level3);
                    break;
                case Level.Level3:
                    LoadLevel(Level.Level4);
                    break;
            }
        }
    }

    public void GameButtonLvl1()
    {
        LoadLevel(Level.Level1);
    }

    public void GameButtonLvl2()
    {
        LoadLevel(Level.Level2);
    }

    public void GameButtonLvl3()
    {
        LoadLevel(Level.Level3);
    }

    public void GameButtonLvl4()
    {
        LoadLevel(Level.Level4);
    }

    public void GameButtonLvlTut()
    {
        LoadLevel(Level.Tutorial);
    }

    public void GameButtonMenu()
    {
        CheckpointChecker.ResetCheckpoints();
        SceneManager.LoadScene(gameLevelMenu);
    }

    public void GameButtonIntroCutScene()
    {
        CheckpointChecker.ResetCheckpoints();
        SceneManager.LoadScene(gameIntroCutscene);
    }

    public enum Level
    {
        Tutorial,
        Level1,
        Level2,
        Level3,
        Level4
    }
}