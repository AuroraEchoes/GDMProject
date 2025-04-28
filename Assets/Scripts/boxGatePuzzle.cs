using UnityEngine;

public class BoxGatePuzzle : MonoBehaviour
{
    public MovingDoor movingDoor;

    private bool zone1occupied, zone2occupied, zone3occupied;

    public void SetZoneState(string zoneName, bool isOccupied)
    {
        switch (zoneName)
        {
            case "zone1":
                zone1occupied = isOccupied;
                break;
            case "zone2":
                zone2occupied = isOccupied;
                break;
            case "zone3":
                zone3occupied = isOccupied;
                break;
        }

        if (zone1occupied && zone2occupied && zone3occupied)
        {
            OpenMiddleDoor();
        }
    }

    public void OpenMiddleDoor()
    {
        movingDoor.Toggle();
    }
}

