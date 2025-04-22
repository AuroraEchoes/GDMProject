using UnityEngine;

public class ColorPuzzle : MonoBehaviour
{
    public GameObject middleDoor;

    private bool redCorrect, blueCorrect, yellowCorrect;

    public void SetBoxState(string zone, bool isCorrect)
    {
        switch (zone)
        {
            case "RedGate": redCorrect = isCorrect; break;
            case "BlueGate": blueCorrect = isCorrect; break;
            case "YellowGate": yellowCorrect = isCorrect; break;
        }

        if (redCorrect && blueCorrect && yellowCorrect)
        {
            OpenMiddleDoor();
        }
    }

    public void OpenMiddleDoor()
    {
        middleDoor.SetActive(false);
    }
}
