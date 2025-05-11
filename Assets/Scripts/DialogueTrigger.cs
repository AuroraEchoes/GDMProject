using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueTyper dialogueTyper; 
    public int dialogueIndex = 0;       

    private bool hasTriggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
            dialogueTyper.StartDialogue(dialogueIndex);
        }
    }
}
