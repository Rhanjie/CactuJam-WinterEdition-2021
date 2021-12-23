using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class MoviePlayer : MonoBehaviour
{
    public RawImage rawImage;
    public VideoPlayer videoPlayer;

    public Button[] toggleButtons;
    public TextMeshProUGUI[] texts;

    public void InteractWithButton(Button button)
    {
        foreach (var toggleButton in toggleButtons)
        {
            toggleButton.interactable = true;
        }

        button.interactable = false;
    }
    
    public void ShowHint(TextMeshProUGUI text)
    {
        foreach (var textObject in texts)
        {
            textObject.gameObject.SetActive(false);;
        }

        text.gameObject.SetActive(true);
    }

    public void InitAndPlay(VideoClip clip)
    {
        videoPlayer.clip = clip;

        StartCoroutine(Play());
    }

    private IEnumerator Play()
    {
        videoPlayer.Prepare();

        while (!videoPlayer.isPrepared)
        {
            yield return new WaitForSeconds(1);
        }

        rawImage.texture = videoPlayer.texture;
        videoPlayer.Play();
    }

    public void StopVideo()
    {
        videoPlayer.Stop();
    }
}
