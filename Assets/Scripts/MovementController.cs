using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovementController : MonoBehaviour
{
    [SerializeField] private List<ControllableCharacter> controllingCharacters = new List<ControllableCharacter>();

    void Update()
    {
        Vector2 inputMovement = Vector2.zero;
        if (Input.GetKeyDown(KeyCode.W))
            inputMovement.y += 1;
        if (Input.GetKeyDown(KeyCode.S))
            inputMovement.y -= 1;
        if (Input.GetKeyDown(KeyCode.A))
            inputMovement.x -= 1;
        if (Input.GetKeyDown(KeyCode.D))
            inputMovement.x += 1;
        foreach (ControllableCharacter character in controllingCharacters)
            character.InputAxes = inputMovement;
    }
}