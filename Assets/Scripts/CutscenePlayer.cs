using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Linq;
using System.Collections;

public class CutscenePlayer : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private GameObject buttonToReveal;
    [SerializeField] private bool playOnStart = true;

    private bool hasFinishedPlaying = false;
    private FullScreenPassRendererFeature feature;

    private void Start()
    {
        UniversalRenderPipelineAsset pipeline = (UniversalRenderPipelineAsset)GraphicsSettings.currentRenderPipeline;
        UniversalRendererData data = (UniversalRendererData)pipeline.rendererDataList[0];
        feature = (FullScreenPassRendererFeature)data.rendererFeatures.Where(f => f is FullScreenPassRendererFeature).First();
        feature.passMaterial.SetInt("_Enabled", 0);

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

    public void LoadTutorial()
    {
        FindFirstObjectByType<LevelManager>().GameButtonLvlTut();
    }

    void OnDestroy()
    {
        feature.passMaterial.SetInt("_Enabled", 1);
    }
}