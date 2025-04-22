using UnityEngine;
using UnityEngine.UI;

public class UIIButtonManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] buttonsToToggle;
    
    [Header("Settings")]
    [SerializeField] private string expandText = "Expand";
    [SerializeField] private string collapseText = "Collapse";
    
    private bool isExpanded = false;
    
    [SerializeField] private Text buttonTextComponent;
    
    private void Start()
    {
        SetButtonsActive(false);
        UpdateButtonText();
    }
    
    public void ToggleButtons()
    {
        isExpanded = !isExpanded;
        SetButtonsActive(isExpanded);
        UpdateButtonText();
    }
    
    private void SetButtonsActive(bool active)
    {
        foreach (GameObject button in buttonsToToggle)
        {
            if (button != null)
            {
                button.SetActive(active);
            }
        }
    }
    
    private void UpdateButtonText()
    {
        if (buttonTextComponent != null)
        {
            buttonTextComponent.text = isExpanded ? collapseText : expandText;
        }
    }
}