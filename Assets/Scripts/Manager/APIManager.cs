using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class APIManager : MonoBehaviour
{
    public static APIManager instance;
    private const string URL = "http://test.iamdave.ai/conversation/exhibit_aldo/74710c52-42a5-3e65-b1f0-2dc39ebe42c2";
    public enum GetType
    {
        TEXT = 0,
        MULTIMEDIA
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {

    }

    public void Post(Dictionary<string, string> data, Action<string> action = null)
    {
        StartCoroutine(PostEnum(data, action));
    }

    private static IEnumerator PostEnum(Dictionary<string, string> data, System.Action<string> callback)
    {
        if (data == null)
        {
            Debug.Log("data is null");
            yield break;
        }
        Debug.Log("Url=" + URL);
        data.Add("system_response", "sr_init");
        data.Add("engagement_id", "NzQ3MTBjNTItNDJhNS0zZTY1LWIxZjAtMmRjMzllYmU0MmMyZXhoaWJpdF9hbGRv");
        UnityWebRequest www = UnityWebRequest.Post(URL, data);
        www.SetRequestHeader("Content-Type", "application/json");
        www.SetRequestHeader("X-I2CE-ENTERPRISE-ID", "dave_expo");
        www.SetRequestHeader("X-I2CE-USER-ID", "74710c52-42a5-3e65-b1f0-2dc39ebe42c2");
        www.SetRequestHeader("X-I2CE-API-KEY", "NzQ3MTBjNTItNDJhNS0zZTY1LWIxZjAtMmRjMzllYmU0MmMyMTYwNzIyMDY2NiAzNw__");

        yield return www.Send();

        if (www.isNetworkError)
        {
            Debug.Log(www.error);
            callback?.Invoke(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!" + www.responseCode);
            Debug.Log("Form upload complete2!" + www.downloadHandler.text);
            Debug.Log(URL);
            if (www.responseCode != 200)
            {
                callback?.Invoke(www.downloadHandler.text);

                yield break;
            }
            callback?.Invoke(www.downloadHandler.text);
        }
    }
    public void Get(string url, GetType getType, Action<string> action = null, Action<byte[]> byteData = null)
    {
        StartCoroutine(get(url, getType, action, byteData));
    }

    private static IEnumerator get(string url, GetType getType, Action<string> action = null, Action<byte[]> byteData = null)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        Debug.Log("Get Url =" + url);
        yield return www.Send();
        if (www.isNetworkError)
        {
            Debug.Log(www.error);
            action?.Invoke(null);
        }
        else
        {
            Debug.Log("get  upload complete!" + www.responseCode);
            Debug.Log("get upload complete2!" + www.downloadHandler.text);
            Debug.Log(url);

            string str = JsonUtility.ToJson(www.downloadHandler);
            Debug.Log("get data " + str);
            if (www.responseCode != 200)
            {
                action?.Invoke(null);
                yield break;
            }
            if (getType == GetType.TEXT)
            {
                action?.Invoke(www.downloadHandler.text);
            }
            else
            {
                byteData?.Invoke(www.downloadHandler.data);
            }
        }
    }
}