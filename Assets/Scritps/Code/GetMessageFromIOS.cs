using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

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
#if UNITY_IOS
         Debug.Log(modelName + " _progress: " + progress);
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

    float offset;
    Vector2 dir = new Vector2();
    float dirX, dirY;

    //private void OnGUI()
    //{   
    //    if (GUILayout.Button(">>>>>����", GUILayout.Width(200f), GUILayout.Height(30f)))
    //    {
    //        string url = "file://"+Application.streamingAssetsPath + "/HeadMeters.zip";
    //        SetModelurl(url); 
    //    }
    //}

    /// <summary>
    /// ���ص�����
    /// </summary>
    /// <param name="url">�ļ���ѹ�����ķ�ʽ����</param> 
    public void SetModelurl(string url)
    {
        Debug.Log(">>>>>>>>>> Get Url From IOS:"+url);
        AssetLoadManager.Instance.DownModeFromWeb(url);
    }

    /// <summary>
    /// ����ӽǹ�λ
    /// </summary>
    public void ResetCameraRotate()
    {
        GameManager.Instance.inputManage.ResetCameraRotateEvent?.Invoke();
    }

    /// <summary>
    /// �����ת�����¼�
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
    /// ���õƹ���ת
    /// </summary>
    /// <param name="x">ˮƽ������ת</param>
    /// <param name="y">��ֱ������ת</param>
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
    /// ���õƹ���ת
    /// </summary>
    /// <param name="x">ˮƽ������ת</param>
    /// <param name="y">��ֱ������ת</param>
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
    /// ���õƹ���תƫ���� Ĭ��Ϊ 1
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
                GameManager.Instance.inputManage.SetHeadLayerShowEvent?.Invoke(m[0], state);
            }
        }
    }

    /// <summary>
    /// ����ͷ���ṹ�ֲ���ʾ
    /// </summary>
    public void ResetHeadLayerShow()
    {
        GameManager.Instance.inputManage.ResetHeadLayerShowEvent?.Invoke();
    }

    /// <summary>
    /// ��ʼoutline line=1 ����
    /// </summary>
    /// <param name="line"></param>
    public void SetOutlineState(string line)
    {
        Debug.Log("����ģ�ͱ�Ե�⣺ " + line);
        float lineState = 0;
        if (float.TryParse(line,out lineState))
        { 
           GameManager.Instance.inputManage.OutLineStateEvent?.Invoke(lineState);
        }
    }

    /// <summary>
    /// ����ģ��͸����
    /// </summary>
    /// <param name="alpha"></param>
    public void SetAlphaState(string alpha)
    {
        Debug.Log("����ģ��͸���ȣ� "+alpha);
        float alpahState = 0;
        if (float.TryParse(alpha, out alpahState))
        {
            GameManager.Instance.inputManage.AlphaStateEvent?.Invoke(alpahState);
        }
        
    }

    /// <summary>
    /// ȡ������ģ��
    /// </summary>
    public void CancleLoadModel()
    {   
        GameManager.Instance.inputManage.CancleLoadedModelEvent?.Invoke();
    }

}
