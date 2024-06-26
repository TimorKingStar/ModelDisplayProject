using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;

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
        Debug.Log(modelName + " _Downprogress: " + progress);
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
        Debug.Log(modelName + " _Loadprogress: " + progress);
#if UNITY_IOS
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


    [DllImport("__Internal")]
    private static extern void CallLightRotateInfo(string lightInfo);

    /// <summary>
    /// 获取灯光的旋转信息 dir.x + "_" + dir.y + "_" + dir.z;   这种格式（20_15_3.2）
    /// </summary>
    /// <param name="info"></param>
    public static void ReturnLightRotateInfo(string info)
    {
        CallLightRotateInfo(info);
    }


    //------------------------------

    [DllImport("__Internal")]
    private static extern void CalAnimationLengthOfClip(string info);

    /// <summary>
    /// 获取动画播放进度条 范围： 0-1
    /// </summary>
    /// <param name="info"></param>
    public static void ReturnAnimationLengthOfClip(string info)
    {
         
#if UNITY_IOS
        CalAnimationLengthOfClip(info);
#endif
    }

    float offset;
    Vector3 dir = new Vector3();
   

    /// <summary>
    /// 下载模型链接
    /// </summary>
    /// <param name="url">文件以压缩包的方式给定</param> 
    public void SetModelurl(string url)
    {
        Debug.Log(">>>>>>>>>> Get Url From IOS:"+url);
        AssetLoadManager.Instance.DownModeFromWeb(url);
    }

    private void OnGUI1()
    {
        if (GUILayout.Button(">>>>>>>>>>加载模型", GUILayout.Width(200), GUILayout.Height(30)))
        {
            AssetLoadManager.Instance.DownModeFromWeb(Application.streamingAssetsPath + "/HeadMeters.zip");
        }
        if (GUILayout.Button(">>>>>>>>>>加载动画"))
        {
            SetAnimationPath(Application.streamingAssetsPath + "/coreapi.fbx");
        }

        if (GUILayout.Button(">>>>>>>>>>播放"))
        {
            PlayAnimation("True");
        }
        if (GUILayout.Button(">>>>>>>>>>暂停"))
        {
            PlayAnimation("False");
        }
        if (GUILayout.Button(">>>>>>>>>>相机归位"))
        {
            ResetCameraRotate();
        }
        if (GUILayout.Button(">>>>>>>>>>跳展"))
        {
            SelectModelMode("1");
        }
    }


    /// <summary>
    ///  0 为模型展示模块，1 为动画展示模块
    /// </summary>
    /// <param name="mode"></param>
    public void SelectModelMode(string mode)
    {
        if (mode == "1")
        {
            SceneManager.LoadScene(Utils.AnimationScene);
        }
        else if (mode == "0")
        {
            SceneManager.LoadScene(Utils.ModelScene);
        }

    }
  
    /// <summary>
    /// 加载动画的链接
    /// </summary>
    /// <param name="path"></param>
    public void SetAnimationPath(string path)
    {
        AssetLoadManager.Instance.LoadAnimation(path);
    }


    /// <summary>
    /// 暂停/播放 动画 play=True 播放  =False暂停
    /// </summary>
    /// <param name="play"></param>
    public void PlayAnimation(string play)
    {
        bool state = false;
        if (play== "True")
        {
            state = true;
        }
        else if(play == "False")
        {
            state = false;
        }

        InputManage.Instance.PlayAnimationEvent?.Invoke(state);
    }

    /// <summary>
    /// 相机视角归位
    /// </summary>
    public void ResetCameraRotate()
    {
        InputManage.Instance.ResetCameraRotateEvent?.Invoke();
    }

    /// <summary>
    /// 相机旋转锁定事件
    /// </summary>
    public void TurnOnModelRotateState(string state)
    {
        if (state==Utils.OpenCameraRotateState)
        {
           InputManage.Instance.TurnOnCameraRotateEvent?.Invoke(true);
        } 
        else if (state == Utils.CloseCameraRotateState)
        {
            InputManage.Instance.TurnOnCameraRotateEvent?.Invoke(false);
        }
    }

    /// <summary>
    /// 设置灯光旋转
    /// </summary>
    /// <param name="x">水平方向旋转</param>
    /// <param name="y">垂直方向旋转</param>
    public void SetLightMoveDirY(string y)
    {
        if (float.TryParse(y, out var dirY))
        {   
            dir.x = 0; dir.y = dirY; dir.z = 0;
            LightController.Instance.Rotate(dir);
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
        if (float.TryParse(x, out var dirX) )
        {
            dir.x = dirX; dir.y = 0; dir.z = 0;
            LightController.Instance.Rotate(dir);
        }
        else
        {  Debug.Log(">>>>>>>Get ios SetLightMoveDirX data error");
        }
    }

    /// <summary>
    /// 获取灯光的旋转信息  
    /// </summary>
    public void GetLightInfo()
    {
        LightController.Instance.GetLightInfo();
    }

    /// <summary>
    /// 复原灯光旋转
    /// </summary>
    public void ResetLightRotate()
    {
        LightController.Instance.ResetLight();
    }   

    /// <summary>
    /// 设置灯光旋转
    /// </summary>
    /// <param name="x">水平方向旋转</param>
    /// <param name="y">垂直方向旋转</param>
    public void SetLightMoveDirZ(string z)
    {
        if (float.TryParse(z, out var dirZ))
        {
            dir.x = 0; dir.y = 0; dir.z = dirZ;
            LightController.Instance.Rotate(dir);
        }
        else
        {
            Debug.Log(">>>>>>>Get ios SetLightMoveDirZ data error");
        }
    }

    ///// <summary>
    ///// 设置灯光旋转偏移量 默认为 1
    ///// </summary>
    ///// <param name="o"></param>
    //public void SetLightMoveOffset(string o)
    //{
    //    if (float.TryParse(o, out offset))
    //    {
    //        GameManager.Instance.lightController.SetOffSet(offset);
    //    }
    //    else
    //    {
    //        Debug.Log(">>>>>>>Get ios SetLightMoveOffset data error");

    //    }
    //}

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
               InputManage.Instance.SetHeadLayerShowEvent?.Invoke(m[0], state);
            }
        }
    }

    /// <summary>
    /// 重置头部结构分层显示
    /// </summary>
    public void ResetHeadLayerShow()
    {
        InputManage.Instance.ResetHeadLayerShowEvent?.Invoke();
    }

    /// <summary>
    /// 开始outline line=1 开启
    /// </summary>
    /// <param name="line"></param>
    public void SetOutlineState(string line)
    {
        Debug.Log(">>>>>>>>Outline: " + line);
        float lineState = 0;
        if (float.TryParse(line,out lineState))
        { 
          InputManage.Instance.OutLineStateEvent?.Invoke(lineState);
        }
    }

    /// <summary>
    /// 设置边缘光宽度
    /// </summary>
    /// <param name="width"></param>
    public void SetOutlineWidth(string width)
    {   
        if (float.TryParse(width, out var w))
        {
           InputManage.Instance.SetOutlineWidthEvent?.Invoke(w);
        }
    }

    /// <summary>
    /// 设置模型透明度
    /// </summary>
    /// <param name="alpha"></param>
    public void SetAlphaState(string alpha)
    {
        Debug.Log(">>>>>>>>>>>>Alpha： "+alpha);
        if (float.TryParse(alpha, out var alpahState))
        {
           InputManage.Instance.AlphaStateEvent?.Invoke(alpahState);
        }
        
    }
    
    /// <summary>
    /// 取消加载模型
    /// </summary>
    public void CancleLoadModel()
    {   
        InputManage.Instance.CancleLoadedModelEvent?.Invoke();
    }

}
