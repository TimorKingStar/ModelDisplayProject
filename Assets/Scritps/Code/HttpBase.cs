using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class HttpBase:MonoBehaviour
{

    RequestResult requestResult;
    public void DownLoadAssets(string url, RequestResult result)
    {
        requestResult = result;
        StartCoroutine(RunDownloadPost(url));
    }

    IEnumerator RunDownloadPost(string url)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);

        if (request != null)
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                requestResult.SetComplate(request.downloadHandler.data);
            }
            else
            {
                requestResult.SetError(request.error);
            }

        }
    }

}

public class RequestResult
{
     Action<string> OnErrorCallBack;
     Action<byte[]> OnComplateCallBack;

    public void SetError(string msg)
    {
        OnErrorCallBack?.Invoke(msg);
    }

    public void SetComplate(byte[] msg)
    {
        OnComplateCallBack?.Invoke(msg);
    }
                 
    public RequestResult OnError(Action<string> err)
    {
        OnErrorCallBack = err;
        return this;
    }

    public RequestResult OnComplate(Action<byte[]> complate)
    {
        OnComplateCallBack = complate;
        return this;
    }
}