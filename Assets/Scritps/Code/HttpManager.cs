using System.Collections.Generic;

public class HttpManager : MonoSingleton<HttpManager>
{
    public RequestResult DownLoadAssets(string url)
    {
        var ap = new RequestResult();
        var tool = gameObject.AddComponent<HttpBase>();
        tool.DownLoadAssets(url, ap);
        return ap;
    }


}



