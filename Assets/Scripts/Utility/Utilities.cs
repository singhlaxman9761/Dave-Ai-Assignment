using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class Utilities
{
    public static IEnumerator LoadAudioFile(string fullpath, System.Action<AudioClip> action)
    {
        Debug.Log("LOADING CLIP " + fullpath);
        if (!File.Exists(fullpath))
        {
            Debug.Log("DIDN'T EXIST: " + fullpath);
            yield break;
        }
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://" + fullpath, AudioType.WAV))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError)
            {
                Debug.Log(www.error);
                action?.Invoke(null);
            }
            else
            {
                action?.Invoke(DownloadHandlerAudioClip.GetContent(www));
            }
        }
    }
}