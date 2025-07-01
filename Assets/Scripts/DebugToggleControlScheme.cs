using UnityEngine;
using UnityEngine.UI;

public class DebugToggleControlScheme : MonoBehaviour
{
    private MovementController movementController;
    [SerializeField] private Text icon;

    void Start()
    {
        movementController = GameObject.Find("Movement Controller").GetComponent<MovementController>();
    }

    public void ToggleControlScheme()
    {
        bool curr = MovementController.UseIsometricControls;
        MovementController.UseIsometricControls = !curr;
        if (!curr)
            icon.text = "";
        else icon.text = "";

    }
}
