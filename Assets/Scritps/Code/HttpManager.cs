using System.Collections.Generic;

public class HttpManager : MonoSingleton<HttpManager>
{
    HttpBase currentHttpBase;
    public RequestResult DownLoadAssets(string url)
    {
        CancleDownload();
        var ap = new RequestResult();
        currentHttpBase = gameObject.AddComponent<HttpBase>();
        currentHttpBase.DownLoadAssets(url, ap);
        return ap;
    }

    public void CancleDownload()
    {
        if (currentHttpBase!=null)
        {   
            currentHttpBase.CancleDownload();
        }
    }
}



