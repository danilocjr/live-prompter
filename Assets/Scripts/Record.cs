
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using NatSuite.Recorders;
using NatSuite.Recorders.Clocks;
using NatSuite.Recorders.Inputs;
using System;
using System.Diagnostics;
using UnityEngine.Audio;
using System.IO;
using System.Collections.Generic;
using UnityEngine.Android;

public class Record : MonoBehaviour
{

    [Header(@"UI")]
    public RawImage rawImage;

    private WebCamTexture webCamTexture;
    private IMediaRecorder recorder;
    private IClock clock;
    [SerializeField] private bool recording;

    AudioSource microphoneSource;
    AudioInput audioInput;
    CameraInput videoInput;
    public string outputPath;
    private bool recordPressed;
    int videoIndex = 1;

    string saveFolderPath;

    [SerializeField] GalleryManager gallery;
    [SerializeField] AutoScroll autoScroll;
    [SerializeField] Animator recordButtonAnim;

    [SerializeField] Camera videoCamera;
    float timer = 0;
    private void Awake()
    {

#if UNITY_EDITOR

        saveFolderPath = Path.Combine(Application.streamingAssetsPath, "media_paths");
#else
        saveFolderPath = Path.Combine(Application.persistentDataPath, "media_paths");
#endif

        if (!Directory.Exists(saveFolderPath))
            Directory.CreateDirectory(saveFolderPath);

    }



    private IEnumerator Start()
    {

        if (PlayerPrefs.HasKey("index"))
            videoIndex = PlayerPrefs.GetInt("index");

        microphoneSource = gameObject.GetComponent<AudioSource>();
        recordPressed = false;



#if UNITY_ANDROID

        Permission.RequestUserPermission(Permission.Camera);
        while (!Permission.HasUserAuthorizedPermission(Permission.Camera))
            yield return null;

        Permission.RequestUserPermission(Permission.Microphone);
        while (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
            yield return null;
#else

yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (!Application.HasUserAuthorization(UserAuthorization.WebCam))
            yield break;
#endif


        WebCamDevice[] devices = WebCamTexture.devices;
        WebCamDevice device = new WebCamDevice();
        foreach (WebCamDevice d in devices)
        {
            if (d.isFrontFacing)
                device = d;
        }
        webCamTexture = new WebCamTexture(device.name, 1280, 720, 30);
        webCamTexture.Play();


        rawImage.texture = webCamTexture;
        rawImage.rectTransform.sizeDelta = new Vector2(1280, 720);
    }


    public void ToggleRecording()
    {
        recordPressed = !recordPressed;

        if (recordPressed)
            StartRecording();
        else
        {
            timer = 30;
        }
    }


    private IEnumerator SetRecording()
    {
        microphoneSource.mute =
        microphoneSource.loop = true;
        microphoneSource.bypassEffects =
        microphoneSource.bypassListenerEffects = false;
        microphoneSource.clip = Microphone.Start(null, true, 30, 48000);
        yield return new WaitUntil(() => Microphone.GetPosition(null) > 0);
        microphoneSource.Play();

        var frameRate = 30;
        var sampleRate = 48000;
        var channelCount = (int)AudioSettings.speakerMode;

        clock = new RealtimeClock();

        recorder = new MP4Recorder(720, 1280, frameRate, sampleRate, 2);


        audioInput = new AudioInput(recorder, clock, microphoneSource, true);
        videoInput = new CameraInput(recorder, clock, videoCamera);

        
        microphoneSource.mute = audioInput == null;

        microphoneSource.Play();

        recording = true;

        timer = 0;

        while (timer < 30)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        StopRecording();

    }

    private void StartRecording()
    {
        autoScroll.OnClickToggleRun();
        recordButtonAnim.SetTrigger("press");
        StartCoroutine(SetRecording());

    }


    private async void StopRecording()
    {
        recordButtonAnim.SetTrigger("release");

        autoScroll.Reset();

        recording = false;

        autoScroll.OnClickToggleRun();

        microphoneSource.mute = true;

        microphoneSource.Stop();

        videoInput?.Dispose();

        audioInput?.Dispose();

        

        Microphone.End(null);

        var path = await recorder.FinishWriting();
#if UNITY_EDITOR
        outputPath = Path.Combine(saveFolderPath, $"video_{videoIndex}.mp4");

        if (File.Exists(outputPath))
            File.Delete(outputPath);

        File.Move(path, outputPath);

        videoIndex++;

        gallery.ShowVideo(outputPath);

#else
        recordButtonAnim.SetTrigger("release");

        NativeGallery.SaveVideoToGallery(path, "Teleprompter", "video_" + videoIndex, GetVideoPath);
#endif

        PlayerPrefs.SetInt("index", videoIndex);
        PlayerPrefs.Save();

    }

    public void GetVideoPath(bool success, string path)
    {
        if (success)
        {
            videoIndex++;
            gallery.ShowVideo(path);
        }

    }
    void Update()
    {
        // Record frames from the webcam
        if (webCamTexture != null)
        {
            if (recording && webCamTexture.didUpdateThisFrame)
            {
                //recorder?.CommitFrame(webCamTexture.GetPixels32(), clock.timestamp);
            }
        }
    }



}
