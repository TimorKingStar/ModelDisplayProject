using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class GetMessageFromIOS : MonoBehaviour
{
    
     [DllImport("__Internal")]
     private static extern void CallLoadModelProgress(string modelName,string funcName);

    public static void LoadModelProgress(string modelName,string progress)
    {
        Debug.Log(modelName + " _progress: " + progress);
#if UNITY_IOS
        CallLoadModelProgress(modelName, progress);
#endif
    }


    //��Ϸ�������ƣ�GetMessageFromOS  �������ƺͲ������£�
    float offset;
    Vector2 dir = new Vector2();
    float dirX, dirY;
    public string state;


    private void OnGUI()
    {
        if (GUILayout.Button(">>>>>����", GUILayout.Width(200f), GUILayout.Height(30f)))
        {
            string url = Application.streamingAssetsPath + "/GeoWithCamera.zip";
            SetModelurl(url);
        }
    }

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
    /// ģ�͹�λ
    /// </summary>
    public void ResetModelRotate()
    {
        GameManager.Instance.inputManage.ResetModelRotateEvent?.Invoke();
    }

    /// <summary>
    /// ��ת�����¼�
    /// </summary>
    public void TurnOnModelRotateState(string state)
    {
        if (state=="OpenRotate")
        {
            GameManager.Instance.inputManage.TurnOnModelRotateEvent?.Invoke(true);
        }
        else if (state == "CloseRotate")
        {
            GameManager.Instance.inputManage.TurnOnModelRotateEvent?.Invoke(false);
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
}
