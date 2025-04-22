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
        // Make sure the button is hidden initially
        if (buttonToReveal != null)
            buttonToReveal.SetActive(false);
            
        if (playOnStart)
            PlayCutscene();
            
        // Add listener for video completion
        videoPlayer.loopPointReached += OnVideoFinished;
    }
    
    public void PlayCutscene()
    {
        if (videoPlayer != null)
        {
            // Hide button when playing
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