using Pathfinding.Serialization.JsonFx;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using static DataModel;

public class GameManager : MonoBehaviour
{
    [SerializeField] Text placeHolder;
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] AudioSource audioSource;
    [SerializeField] VideoResponse videoResponse;
    [SerializeField] GameObject loadingScreen;

    void Start()
    {
        loadingScreen.SetActive(false);
    }

    public void ButtonClicked(string value)
    {
        Debug.Log($"Button Clicked:{value}");
        loadingScreen.SetActive(true);
        Dictionary<string, string> data = new Dictionary<string, string>
        {
            { "customer_state", value }
        };
        APIManager.instance.Post(data, delegate (string response)
        {
            Debug.Log($"RESPINSE: {response}");
            videoResponse = JsonReader.Deserialize<VideoResponse>(response);
            placeHolder.text = videoResponse.placeholder;
            PlayVideo(videoResponse.name);
        });
    }

    private void PlayVideo(string value)
    {
        videoPlayer.Stop();
        videoPlayer.url = videoResponse.data.video;
        videoPlayer.Prepare();
        videoPlayer.prepareCompleted += delegate (VideoPlayer source)
        {
            Debug.Log($"Video prepared!!!: {source.length}");
            videoPlayer.Play();
            loadingScreen.SetActive(false);
        };
        string fullpath = Application.persistentDataPath + "/" + value + ".wav";

        Debug.Log($"fullpath: {fullpath}");

        if (File.Exists(fullpath))
        {
            Debug.Log($"Audio File Exist");
            StartCoroutine(Utilities.LoadAudioFile(fullpath, delegate (AudioClip audioClip)
            {
                PlayAudio(audioClip);
            }));
        }
        else
        {
            Debug.Log($"Audio File doesn't Exist");
            APIManager.instance.Get(videoResponse.response_channels.voice, APIManager.GetType.MULTIMEDIA, delegate (string response)
               {
               }, delegate (byte[] byteData)
                {
                    Stream t = new FileStream(fullpath, FileMode.Create);
                    BinaryWriter b = new BinaryWriter(t);
                    b.Write(byteData);
                    t.Close();
                    StartCoroutine(Utilities.LoadAudioFile(fullpath, delegate (AudioClip audioClip)
                    {
                        PlayAudio(audioClip);
                    }));
                });
        }
    }

    private void PlayAudio(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        if (audioSource.clip != null)
        {
            if (audioSource.isPlaying)
                audioSource.Stop();
            audioSource.Play();
        }
    }
}