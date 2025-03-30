using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private List<ControllableCharacter> controllingCharacters = new List<ControllableCharacter>();

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
        foreach (ControllableCharacter character in controllingCharacters)
            character.SetInputAxes(inputMovement);

        if (Input.GetKeyDown(KeyCode.Space))
            foreach (ControllableCharacter character in controllingCharacters)
                character.JumpWindowStart();
        if (Input.GetKeyUp(KeyCode.Space))
            foreach (ControllableCharacter character in controllingCharacters)
                character.JumpWindowEnd();
    }
}