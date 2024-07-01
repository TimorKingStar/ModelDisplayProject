using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;

public class GetMessageFromIOS : MonoBehaviour
{

    [DllImport("__Internal")]
    private static extern void CallDownloadModelProgress(string modelName, string funcName);
    
    public static void DownloadModelProgress(string modelName, string progress)
    {
        Debug.Log(modelName + " _Downprogress: " + progress);
     #if UNITY_EDITOR
        return;
     #endif
     
        CallDownloadModelProgress(modelName, progress);

    }

    [DllImport("__Internal")]
    private static extern void CallLoadModelProgress(string modelName,string progress);
   
    public static void LoadModelProgress(string modelName,string progress)
    {

        Debug.Log(modelName + " _Loadprogress: " + progress);
#if UNITY_EDITOR
         return;
#endif
        CallLoadModelProgress(modelName, progress);
    }
    
    [DllImport("__Internal")]
    private static extern void CallHeadLayerInfo(string modelInfo);
    
    public static void ReturnHeadModelInfo(string modelInfo)
    {
        
 #if UNITY_EDITOR
         return;
#endif
         CallHeadLayerInfo(modelInfo);

    }


    [DllImport("__Internal")]
    private static extern void CallLightRotateInfo(string lightInfo);

   
    public static void ReturnLightRotateInfo(string info)
    {
        CallLightRotateInfo(info);
    }


    //------------------------------

    [DllImport("__Internal")]
    private static extern void CallAnimationLengthOfClip(string info);

  
    public static void ReturnAnimationLengthOfClip(string info)
    {
         #if UNITY_EDITOR
         return;
         #endif
        CallAnimationLengthOfClip(info);

    }

    float offset;
    Vector3 dir = new Vector3();
   

   
    public void SetModelurl(string url)
    {
        Debug.Log(">>>>>>>>>> Get Url From IOS:"+url);
        AssetLoadManager.Instance.DownModeFromWeb(url);
    }
    
    public float timeline;
    private void OnGUI1()
    {
        if (GUILayout.Button(">>>>>>>>>>load Model", GUILayout.Width(200), GUILayout.Height(50)))
        {
            AssetLoadManager.Instance.DownModeFromWeb(@"file://"+Application.streamingAssetsPath + "/MultiCharsCentimeters.zip");
        }
        if (GUILayout.Button(">>>>>>>>>>load Animation", GUILayout.Width(200), GUILayout.Height(50)))
        {
            SetAnimationPath(Application.streamingAssetsPath + "/coreapi.fbx");
        }

        if (GUILayout.Button(">>>>>>>>>>Play Animation", GUILayout.Width(200), GUILayout.Height(50)))
        {
            PlayAnimation("True");
        }
        if (GUILayout.Button(">>>>>>>>>>Pause Animation" ,GUILayout.Width(200), GUILayout.Height(50)))
        {
            PlayAnimation("False");
        }
        if (GUILayout.Button(">>>>>>>>>>Reset Camera", GUILayout.Width(200), GUILayout.Height(50)))
        {
            ResetCameraRotate();
        }
        if (GUILayout.Button("LoadAnimationScene",GUILayout.Width(200), GUILayout.Height(50)))
        {
            SelectModelMode("1");
        }

         if (GUILayout.Button("PlayTime",GUILayout.Width(200), GUILayout.Height(50)))
        {
             PlayApppointAnima(timeline.ToString());
        }
           if (GUILayout.Button("Intensity",GUILayout.Width(200), GUILayout.Height(50)))
        {
             SetLightIntensity(timeline.ToString());
        }
    }
    
    /// <summary>
    /// 动画进度条范围 0-1，当传入0 时人物回到复原状态
    /// </summary>
    /// <param name="p"></param>
    public void PlayApppointAnima(string p)
    {
       if(float.TryParse(p,out var pro))
       {
           InputManage.Instance.PlayApppointAnimEvent?.Invoke(pro);
       }
    }
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
    /// 设置灯光强度
    /// </summary>
    /// <param name="inten"></param>
    public void SetLightIntensity(string inten)
    {
        Debug.Log(inten); 
        if(float.TryParse(inten,out var t))
        {
            
            LightController.Instance.SetIntensity(t);
        }
    }
    public void SetAnimationPath(string path)
    {
        AssetLoadManager.Instance.LoadAnimation(path);
    }

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
    public void ResetCameraRotate()
    {
        InputManage.Instance.ResetCameraRotateEvent?.Invoke();
    }

   
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
    /// ��ȡ�ƹ����ת��Ϣ  
    /// </summary>
    public void GetLightInfo()
    {
        LightController.Instance.GetLightInfo();
    }


    public void ResetLightRotate()
    {
        LightController.Instance.ResetLight();
    }   

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

   
    public void ResetHeadLayerShow()
    {
        InputManage.Instance.ResetHeadLayerShowEvent?.Invoke();
    }
    
    public void SetOutlineState(string line)
    {
        Debug.Log(">>>>>>>>Outline: " + line);
         
        if (float.TryParse(line,out var lineState))
        { 
          InputManage.Instance.OutLineStateEvent?.Invoke(lineState);
        }
    }

    
    public void SetOutlineWidth(string width)
    {   
        if (float.TryParse(width, out var w))
        {
           InputManage.Instance.SetOutlineWidthEvent?.Invoke(w);
        }
    }

 
    public void SetAlphaState(string alpha)
    {
        Debug.Log(">>>>>>>>>>>>Alpha�� "+alpha);
        if (float.TryParse(alpha, out var alpahState))
        {
           InputManage.Instance.AlphaStateEvent?.Invoke(alpahState);
        }
        
    }
    

    public void CancleLoadModel()
    {   
        InputManage.Instance.CancleLoadedModelEvent?.Invoke();
    }

}
