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
    /// ����ģ�ͽ���
    /// </summary>
    /// <param name="modelName">ģ����</param>
    /// <param name="progress">���ؽ���</param>
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
    /// ����ģ�ͽ���
    /// </summary>
    /// <param name="modelName">ģ����</param>
    /// <param name="progress">���ؽ���</param>
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
    /// ͷ���ֲ�ģ����Ϣ
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
    /// ��ȡ�ƹ����ת��Ϣ dir.x + "_" + dir.y + "_" + dir.z;   ���ָ�ʽ��20_15_3.2��
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
    /// ��ȡ�������Ž����� ��Χ�� 0-1
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
    /// ����ģ������
    /// </summary>
    /// <param name="url">�ļ���ѹ�����ķ�ʽ����</param> 
    public void SetModelurl(string url)
    {
        Debug.Log(">>>>>>>>>> Get Url From IOS:"+url);
        AssetLoadManager.Instance.DownModeFromWeb(url);
    }

    private void OnGUI1()
    {
        if (GUILayout.Button(">>>>>>>>>>����ģ��", GUILayout.Width(200), GUILayout.Height(30)))
        {
            AssetLoadManager.Instance.DownModeFromWeb(Application.streamingAssetsPath + "/HeadMeters.zip");
        }
        if (GUILayout.Button(">>>>>>>>>>���ض���"))
        {
            SetAnimationPath(Application.streamingAssetsPath + "/coreapi.fbx");
        }

        if (GUILayout.Button(">>>>>>>>>>����"))
        {
            PlayAnimation("True");
        }
        if (GUILayout.Button(">>>>>>>>>>��ͣ"))
        {
            PlayAnimation("False");
        }
        if (GUILayout.Button(">>>>>>>>>>�����λ"))
        {
            ResetCameraRotate();
        }
        if (GUILayout.Button(">>>>>>>>>>��չ"))
        {
            SelectModelMode("1");
        }
    }


    /// <summary>
    ///  0 Ϊģ��չʾģ�飬1 Ϊ����չʾģ��
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
    /// ���ض���������
    /// </summary>
    /// <param name="path"></param>
    public void SetAnimationPath(string path)
    {
        AssetLoadManager.Instance.LoadAnimation(path);
    }


    /// <summary>
    /// ��ͣ/���� ���� play=True ����  =False��ͣ
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
    /// ����ӽǹ�λ
    /// </summary>
    public void ResetCameraRotate()
    {
        InputManage.Instance.ResetCameraRotateEvent?.Invoke();
    }

    /// <summary>
    /// �����ת�����¼�
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
    /// ���õƹ���ת
    /// </summary>
    /// <param name="x">ˮƽ������ת</param>
    /// <param name="y">��ֱ������ת</param>
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
    /// ���õƹ���ת
    /// </summary>
    /// <param name="x">ˮƽ������ת</param>
    /// <param name="y">��ֱ������ת</param>
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

    /// <summary>
    /// ��ԭ�ƹ���ת
    /// </summary>
    public void ResetLightRotate()
    {
        LightController.Instance.ResetLight();
    }   

    /// <summary>
    /// ���õƹ���ת
    /// </summary>
    /// <param name="x">ˮƽ������ת</param>
    /// <param name="y">��ֱ������ת</param>
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
    ///// ���õƹ���תƫ���� Ĭ��Ϊ 1
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
    /// ���÷ֲ���ʾģ��   �����ʽ�� Head+true  (ģ����+����״̬)
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
    /// ����ͷ���ṹ�ֲ���ʾ
    /// </summary>
    public void ResetHeadLayerShow()
    {
        InputManage.Instance.ResetHeadLayerShowEvent?.Invoke();
    }

    /// <summary>
    /// ��ʼoutline line=1 ����
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
    /// ���ñ�Ե����
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
    /// ����ģ��͸����
    /// </summary>
    /// <param name="alpha"></param>
    public void SetAlphaState(string alpha)
    {
        Debug.Log(">>>>>>>>>>>>Alpha�� "+alpha);
        if (float.TryParse(alpha, out var alpahState))
        {
           InputManage.Instance.AlphaStateEvent?.Invoke(alpahState);
        }
        
    }
    
    /// <summary>
    /// ȡ������ģ��
    /// </summary>
    public void CancleLoadModel()
    {   
        InputManage.Instance.CancleLoadedModelEvent?.Invoke();
    }

}
