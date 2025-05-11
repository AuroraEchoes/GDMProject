using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueTyper : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public float typingSpeed = 0.05f;

    [TextArea(3, 10)]
    public string[] dialogueLines;

    private int index = 0;

  void Start()
  {
     StartCoroutine(TypeLine());
   }

  IEnumerator TypeLine()
    {
       dialogueText.text = "";
        string line = dialogueLines[index];
       for (int i = 0; i < line.Length; i++)
        {
            dialogueText.text += line[i];
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void StartDialogue(int startIndex = 0)
    {
        index = startIndex;
        StopAllCoroutines();
        StartCoroutine(TypeLine());
    }

    public void NextLine()
   {
        if (index < dialogueLines.Length - 1)
        {
            index++;
            StopAllCoroutines();
            StartCoroutine(TypeLine());
        }
    }
}

