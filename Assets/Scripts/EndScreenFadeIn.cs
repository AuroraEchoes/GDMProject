using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenFadeIn : MonoBehaviour
{
    [SerializeField] private List<CanvasRenderer> objectList;
    [SerializeField] private List<float> fadeInStartList;
    [SerializeField] private float fadeInTime = 0.5f;

    void Start()
    {
        for (int i = 0; i < objectList.Count; i++)
        {
            fadeInStartList[i] += fadeInTime;
            Color color = objectList[i].GetColor();
            color.a = 0.0f;
            objectList[i].SetColor(color);
        }
        StartCoroutine(ResetToMainMenu());
    }

    void Update()
    {
        for (int i = 0; i < objectList.Count; i++)
        {
            fadeInStartList[i] -= Time.deltaTime;
            float time = fadeInStartList[i];
            if (time < fadeInTime)
            {
                Color color = objectList[i].GetColor();
                color.a = 1.0f - (time / fadeInTime);
                objectList[i].SetColor(color);
            }
        }
    }

    IEnumerator ResetToMainMenu()
    {
        yield return new WaitForSeconds(10.0f);
        SceneManager.LoadScene(0);
    }
}
