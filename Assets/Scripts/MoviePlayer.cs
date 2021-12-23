using System;
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
    
    private List<string> _videoUrls;

    public Button[] toggleButtons;
    public TextMeshProUGUI[] texts;

    private void Awake()
    {
        _videoUrls = new List<string>();
        
        for (int i = 0; i < 4; i++)
        {
            var path = System.IO.Path.Combine(Application.streamingAssetsPath, $"{i + 1}.mp4");
            _videoUrls.Add(path);
        }
    }

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

    public void InitAndPlay(int index)
    {
        videoPlayer.url = _videoUrls[index];

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
