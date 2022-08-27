using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
public class GalleryButton : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    private string videoUrl;
    private RenderTexture videoTex;
    [SerializeField] RawImage videoThumb;

    public void Set(string url)
    {
        if (videoTex!=null)
        {
            Destroy(videoTex);
            videoTex = null;
        }

        videoUrl = url;

        StartCoroutine(SetVideoThumbnail());
    }

    private IEnumerator SetVideoThumbnail()
    {
        videoTex = new RenderTexture(720, 1280, 16);

        videoPlayer.targetTexture = videoTex;
        videoThumb.texture = videoTex;

        videoThumb.enabled = true;

        videoPlayer.url = videoUrl;

        videoPlayer.Play();

        yield return new WaitForSeconds(0.3f);

        videoPlayer.Pause();
        
    }
}
