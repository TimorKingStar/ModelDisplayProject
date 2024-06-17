using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class HttpBase:MonoBehaviour
{

    RequestResult requestResult;
    Coroutine coroutine;

    public void CancleDownload()
    {
        if (coroutine!=null)
        {
            StopCoroutine(coroutine);
            Destroy(this);
        }
    }

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
            requestResult.SetProgress(request.downloadProgress);
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                requestResult.SetComplate(request.downloadHandler.data);
                Destroy(this);
            }
            else
            {   
                requestResult.SetError(request.error);
                Destroy(this);
            }
        }
    }

}

public class RequestResult
{
     Action<string> OnErrorCallBack;
     Action<byte[]> OnComplateCallBack;
    Action<float> OnProgressCallBack;

    public void SetError(string msg)
    {
        OnErrorCallBack?.Invoke(msg);
    }

    public void SetProgress(float p)
    {   
        OnProgressCallBack?.Invoke(p);
    }

    public void SetComplate(byte[] msg)
    {
        OnComplateCallBack?.Invoke(msg);
    }

    public RequestResult OnProgress(Action<float> Progress)
    {
        OnProgressCallBack = Progress;
        return this;
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