using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class CutscenePlayer : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private GameObject buttonToReveal;
    [SerializeField] private bool playOnStart = true;
    
    private bool hasFinishedPlaying = false;

    private void Start()
    {
        if (buttonToReveal != null)
            buttonToReveal.SetActive(false);
            
        if (playOnStart)
            PlayCutscene();
            
        videoPlayer.loopPointReached += OnVideoFinished;
    }
    
    public void PlayCutscene()
    {
        if (videoPlayer != null)
        {
            if (buttonToReveal != null)
                buttonToReveal.SetActive(false);
                
            hasFinishedPlaying = false;
            videoPlayer.Play();
        }
    }
    
    private void OnVideoFinished(VideoPlayer vp)
    {
        if (!hasFinishedPlaying)
        {
            hasFinishedPlaying = true;
            RevealButton();
        }
    }
    
    private void RevealButton()
    {
        if (buttonToReveal != null)
            buttonToReveal.SetActive(true);
    }
}