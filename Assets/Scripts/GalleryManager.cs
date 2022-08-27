using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class GalleryManager : MonoBehaviour
{
    [SerializeField] private Record recordManager;

    [SerializeField] private RawImage videoViewer;
    [SerializeField] private GameObject previewWindow;


    private VideoPlayer videoPlayer;

    [SerializeField] private string currentUrl;

    private void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        previewWindow.SetActive(false);
    }

   

    public void CloseGallery()
    {
        videoPlayer.Stop();
        previewWindow.SetActive(false);
    }

    public void ShowVideo(string _url)
    {
        currentUrl = _url;

        videoPlayer.url = _url;

        videoPlayer.Stop();
        videoPlayer.Play();

        previewWindow.SetActive(true);
    }

    public void DeleteMedia()
    {
        CloseGallery();

        if (File.Exists(currentUrl))
            File.Delete(currentUrl);
        
    }

}
