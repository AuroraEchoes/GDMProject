using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private List<ControllableCharacter> controllingCharacters = new List<ControllableCharacter>();
    [SerializeField] private ControllableEntityParams entityParams;
    [SerializeField] private Vector2 forwardDirection = Vector2.right;
    [SerializeField] private bool overrideForwardDirection = false;
    [SerializeField] private LevelManager levelManager;
    private bool levelManagerNotFoundErrorShown;

    private bool checkPointTriggeredOne = false;
    private bool checkPointTwoTriggeredOne = false;

    void Start()
    {
        foreach (ControllableCharacter character in controllingCharacters)
        {
            character.Params = entityParams;
            character.ForwardDirection = overrideForwardDirection ? forwardDirection : entityParams.ForwardDirection;
        }
    }


    public void CheckPointOne()
    {
        checkPointTriggeredOne = true;
    }

    public void CheckPointTwo()
    {
        checkPointTwoTriggeredOne = true;
    }



    void Update()
    {
        // Handle input movement
        Vector2 inputMovement = Vector2.zero;
        if (Input.GetKey(KeyCode.W)) 
            inputMovement.y += 1;
        if (Input.GetKey(KeyCode.S)) 
            inputMovement.y -= 1;
        if (Input.GetKey(KeyCode.A)) 
            inputMovement.x -= 1;
        if (Input.GetKey(KeyCode.D)) 
            inputMovement.x += 1;

        // Process character movement and check for falls
        bool anyCharacterFalling = false;
        foreach (ControllableCharacter character in controllingCharacters)
        {
            if (character == null) continue;

            character.SetInputAxes(inputMovement);
            anyCharacterFalling = anyCharacterFalling || character.IsFallingIntoVoid();
        }

        if (anyCharacterFalling && levelManager != null)
        {
            levelManager.ReloadCurrentLevel();
        }
    }
}
