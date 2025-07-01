using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using NUnit.Framework.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovementController : MonoBehaviour
{
    [SerializeField] private List<ControllableCharacter> controllingCharacters = new List<ControllableCharacter>();
    [SerializeField] private ControllableEntityParams entityParams;
    [SerializeField] public Vector2 ForwardDirection = Vector2.right;
    public static bool UseIsometricControls = false;
    public Action MovementCalibratedEvent;
    [SerializeField] private bool overrideForwardDirection = false;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private bool useDetectControlScheme = true;
    [SerializeField] private float detectControlSchemeTime = 1.0f;
    [SerializeField] private bool defaultControlsAreIso = true;
    [SerializeField] private float detectControlThresholdInputRatio = 0.35f;
    private float detectControlSchemeRemaining;
    private float detectControlTimePressingCardinal;
    private float detectControlTimePressingDiagonal;
    private bool levelManagerNotFoundErrorShown;
    private bool firstUserInputDetected = false;

    private bool checkPointTriggeredOne = false;
    private bool checkPointTwoTriggeredOne = false;

    void Start()
    {
        foreach (ControllableCharacter character in controllingCharacters)
        {
            character.Params = entityParams;
            character.MovementController = this;
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

        // Only start detecting preferred control scheme once the user
        // starts pressing buttons
        bool nowHaveInputMovement = inputMovement != Vector2.zero;
        if (!firstUserInputDetected && nowHaveInputMovement && SceneManager.GetActiveScene().name == "DesignInitalTest")
        {
            firstUserInputDetected = true;
            if (useDetectControlScheme)
            {
                detectControlSchemeRemaining = detectControlSchemeTime;
                UseIsometricControls = defaultControlsAreIso;
            }
        }

        // I should be shot for this code. for the record
        if
        (
            useDetectControlScheme
            && detectControlSchemeRemaining > 0
            && SceneManager.GetActiveScene().name == "DesignInitalTest"
            && firstUserInputDetected
        )
        {
            DetectControlScheme();
            inputMovement = EnforceCardinalDirections(inputMovement);
        }

        // Process character movement and check for falls
        bool anyCharacterFalling = false;
        foreach (ControllableCharacter character in controllingCharacters)
        {
            if (character == null) continue;

            character.SetInputAxes(inputMovement);
            anyCharacterFalling = anyCharacterFalling || Utils.IsFallingIntoVoid(character.rb);
        }

        if (anyCharacterFalling && levelManager != null)
        {
            levelManager.ReloadCurrentLevel();
        }
    }

    private void DetectControlScheme()
    {
        detectControlSchemeRemaining -= Time.deltaTime;
        // The criteria I’m going to set is
        // - User spent > n% (35% by default) of time spent pressing W/S also pressing A/D → Iso
        // - Else → World
        if (detectControlSchemeRemaining >= 0.0f)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
                    detectControlTimePressingDiagonal += Time.deltaTime;
                else
                    detectControlTimePressingCardinal += Time.deltaTime;
        }
        else
        {
            // Calculate results
            // More time pressing cardinal — don’t use iso controls
            if (detectControlTimePressingCardinal * detectControlThresholdInputRatio > detectControlTimePressingDiagonal)
                UseIsometricControls = false;
            else
                UseIsometricControls = true;
            MovementCalibratedEvent?.Invoke();
        }
    }

    // This is bad, dumb code that I’m writing in a rush
    private Vector2 EnforceCardinalDirections(Vector2 inputBase)
    {
        if (UseIsometricControls)
        {
            // W only. Go foward (W+D)
            if (inputBase.y == 1 && inputBase.x == 0)
                return new Vector2(1, 1);
            // S only. Go backward (S+A)
            else if (inputBase.y == -1 && inputBase.x == 0)
                return new Vector2(-1, -1);
            // D only. Go right (S+D)
            else if (inputBase.x == 1 && inputBase.y == 0)
                return new Vector2(-1, 1);
            // A only. Go left (W+A)
            else if (inputBase.x == -1 && inputBase.y == 0)
                return new Vector2(1, 1);
            // Already isometric
            else
                return inputBase;
        }
        else
        {
            // W+D. Go foward (W)
            if (inputBase.y == 1 && inputBase.x == 1)
                return new Vector2(0, 1);
            // S+A. Go backward (S)
            else if (inputBase.y == -1 && inputBase.x == -1)
                return new Vector2(0, -1);
            // S+D. Go right (D)
            else if (inputBase.x == 1 && inputBase.y == -1)
                return new Vector2(1, 0);
            // W+A. Go left (A)
            else if (inputBase.x == -1 && inputBase.y == 1)
                return new Vector2(-1, 0);
            // Already world
            else
                return inputBase;
        }
    }
}
