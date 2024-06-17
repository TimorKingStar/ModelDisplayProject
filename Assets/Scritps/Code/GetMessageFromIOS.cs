using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class GetMessageFromIOS : MonoBehaviour
{

    [DllImport("__Internal")]
    private static extern void CallDownloadModelProgress(string modelName, string funcName);
    /// <summary>
    /// 下载模型进度
    /// </summary>
    /// <param name="modelName">模型名</param>
    /// <param name="progress">下载进度</param>
    public static void DownloadModelProgress(string modelName, string progress)
    {

#if UNITY_IOS
        CallDownloadModelProgress(modelName, progress);
#endif
    }

    [DllImport("__Internal")]
    private static extern void CallLoadModelProgress(string modelName,string progress);
    /// <summary>
    /// 加载模型进度
    /// </summary>
    /// <param name="modelName">模型名</param>
    /// <param name="progress">加载进度</param>
    public static void LoadModelProgress(string modelName,string progress)
    {
#if UNITY_IOS
         Debug.Log(modelName + " _progress: " + progress);
        CallLoadModelProgress(modelName, progress);
#endif
    }

    [DllImport("__Internal")]
    private static extern void CallHeadLayerInfo(string modelInfo);
    /// <summary>
    /// 头部分层模型信息
    /// </summary>
    /// <param name="modelInfo"></param>
    public static void ReturnHeadModelInfo(string modelInfo)
    {
#if UNITY_IOS
         CallHeadLayerInfo(modelInfo);
#endif
    }

    float offset;
    Vector2 dir = new Vector2();
    float dirX, dirY;

    //private void OnGUI()
    //{   
    //    if (GUILayout.Button(">>>>>走你", GUILayout.Width(200f), GUILayout.Height(30f)))
    //    {
    //        string url = "file://"+Application.streamingAssetsPath + "/HeadMeters.zip";
    //        SetModelurl(url); 
    //    }
    //}

    /// <summary>
    /// 下载的链接
    /// </summary>
    /// <param name="url">文件以压缩包的方式给定</param> 
    public void SetModelurl(string url)
    {
        Debug.Log(">>>>>>>>>> Get Url From IOS:"+url);
        AssetLoadManager.Instance.DownModeFromWeb(url);
    }

    /// <summary>
    /// 相机视角归位
    /// </summary>
    public void ResetCameraRotate()
    {
        GameManager.Instance.inputManage.ResetCameraRotateEvent?.Invoke();
    }

    /// <summary>
    /// 相机旋转锁定事件
    /// </summary>
    public void TurnOnModelRotateState(string state)
    {
        if (state=="OpenRotate")
        {
            GameManager.Instance.inputManage.TurnOnCameraRotateEvent?.Invoke(true);
        }
        else if (state == "CloseRotate")
        {
            GameManager.Instance.inputManage.TurnOnCameraRotateEvent?.Invoke(false);
        }
    }

    /// <summary>
    /// 设置灯光旋转
    /// </summary>
    /// <param name="x">水平方向旋转</param>
    /// <param name="y">垂直方向旋转</param>
    public void SetLightMoveDirY(string y)
    {
        if (float.TryParse(y, out dirY))
        {   
            dir.x = 0; dir.y = dirY;
            GameManager.Instance.lightController.Rotate(dir);
        }
        else
        {   
            Debug.Log(">>>>>>>Get ios SetLightMoveDirY data error");
        }
    }

    /// <summary>
    /// 设置灯光旋转
    /// </summary>
    /// <param name="x">水平方向旋转</param>
    /// <param name="y">垂直方向旋转</param>
    public void SetLightMoveDirX(string x)
    {
        if (float.TryParse(x, out dirX) )
        {
            dir.x = dirX; dir.y = 0;
            GameManager.Instance.lightController.Rotate(dir);
        }
        else
        {  Debug.Log(">>>>>>>Get ios SetLightMoveDirX data error");
        }
    }

    /// <summary>
    /// 设置灯光旋转偏移量 默认为 1
    /// </summary>
    /// <param name="o"></param>
    public void SetLightMoveOffset(string o)
    {
        if (float.TryParse(o, out offset))
        {
            GameManager.Instance.lightController.SetOffSet(offset);
        }
        else
        {
            Debug.Log(">>>>>>>Get ios SetLightMoveOffset data error");

        }
    }

    /// <summary>
    /// 设置分层显示模型   传输格式： Head+true  (模型名+设置状态)
    /// </summary>
    /// <param name="modelState"></param>
    public void SetHeadLayerShow(string modelState)
    {   
       var m = modelState.Split('+');
        if (m.Length==2)
        {   
            bool state;
            if (bool.TryParse(m[1],out state))
            {
                GameManager.Instance.inputManage.SetHeadLayerShowEvent?.Invoke(m[0], state);
            }
        }
    }

    /// <summary>
    /// 重置头部结构分层显示
    /// </summary>
    public void ResetHeadLayerShow()
    {
        GameManager.Instance.inputManage.ResetHeadLayerShowEvent?.Invoke();
    }

    /// <summary>
    /// 开始outline line=1 开启
    /// </summary>
    /// <param name="line"></param>
    public void SetOutlineState(string line)
    {
        Debug.Log("设置模型边缘光： " + line);
        float lineState = 0;
        if (float.TryParse(line,out lineState))
        { 
           GameManager.Instance.inputManage.OutLineStateEvent?.Invoke(lineState);
        }
    }

    /// <summary>
    /// 设置模型透明度
    /// </summary>
    /// <param name="alpha"></param>
    public void SetAlphaState(string alpha)
    {
        Debug.Log("设置模型透明度： "+alpha);
        float alpahState = 0;
        if (float.TryParse(alpha, out alpahState))
        {
            GameManager.Instance.inputManage.AlphaStateEvent?.Invoke(alpahState);
        }
        
    }

    /// <summary>
    /// 取消加载模型
    /// </summary>
    public void CancleLoadModel()
    {   
        GameManager.Instance.inputManage.CancleLoadedModelEvent?.Invoke();
    }

}
