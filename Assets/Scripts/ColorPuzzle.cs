using UnityEngine;

public class ColorPuzzle : MonoBehaviour
{
    public MovingDoor movingDoor;

    private bool redCorrect, blueCorrect, yellowCorrect;

    public void SetBoxState(string zone, bool isCorrect)
    {
        switch (zone)
        {
            case "red zone": redCorrect = isCorrect; break;
            case "blue zone": blueCorrect = isCorrect; break;
            case "yellow zone": yellowCorrect = isCorrect; break;
        }

        if (redCorrect && blueCorrect && yellowCorrect)
        {
            OpenMiddleDoor();
        }
    }

    public void OpenMiddleDoor()
    {
        movingDoor.Toggle();
    }
}
