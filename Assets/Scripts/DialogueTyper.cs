using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueTyper : MonoBehaviour
{
    public GameObject dialoguePanel;
    public CanvasGroup canvasGroup;
    public TextMeshProUGUI dialogueText;
    public float typingSpeed = 0.05f;
    public float fadeDuration = 0.3f;

    public string[] dialogueLines;
    private int index = 0;

    void Start()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public void StartDialogue(int startIndex = 0)
    {
        index = startIndex;
        StartCoroutine(FadeIn());
    }

 IEnumerator FadeIn()
    {
        dialoguePanel.SetActive(true);
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        float time = 0f;
        while (time < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, time / fadeDuration);
            time += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1f;
        StartCoroutine(TypeLine());
    }

 IEnumerator FadeOut()
    {
       canvasGroup.interactable = false;
       canvasGroup.blocksRaycasts = false;

        float time = 0f;
        while (time < fadeDuration)
        {
           canvasGroup.alpha = Mathf.Lerp(1f, 0f, time / fadeDuration);
            time += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0f;
        dialoguePanel.SetActive(false);
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

        
        if (index >= dialogueLines.Length - 1)
        {
            yield return new WaitForSeconds(2f); 
            yield return StartCoroutine(FadeOut());
        }
    }

    
}



