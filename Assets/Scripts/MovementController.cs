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

    void Start()
    {
        foreach (ControllableCharacter character in controllingCharacters)
        {
            character.Params = entityParams;
            character.ForwardDirection = overrideForwardDirection ? forwardDirection : entityParams.ForwardDirection;
        }
    }

    void Update()
    {
        Vector2 inputMovement = Vector2.zero;
        if (Input.GetKey(KeyCode.W))
            inputMovement.y += 1;
        if (Input.GetKey(KeyCode.S))
            inputMovement.y -= 1;
        if (Input.GetKey(KeyCode.A))
            inputMovement.x -= 1;
        if (Input.GetKey(KeyCode.D))
            inputMovement.x += 1;
        bool resetLevel = false;
        foreach (ControllableCharacter character in controllingCharacters)
        {
            character.SetInputAxes(inputMovement);
            resetLevel = resetLevel || character.IsFallingIntoVoid();
        }
        if (resetLevel)
        {
            if (levelManager is not null)
            {
                levelManager.ReloadCurrentLevel();
            }
            else if (!levelManagerNotFoundErrorShown)
            {
                levelManagerNotFoundErrorShown = true;
                Debug.LogWarning("Movement Controller does not store a reference to Level Manager (and thus cannot reset levels)");
            }
        }
    }

}