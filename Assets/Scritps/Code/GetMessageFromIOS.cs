using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class GetMessageFromIOS : MonoBehaviour
{
    
     [DllImport("__Internal")]
     private static extern void CallObjCFunc(string funcName);


    //游戏物体名称：GetMessageFromIOS  方法名称和参数如下：
    float offset;
    Vector2 dir = new Vector2();
    float dirX, dirY;
    public string state;

    //private void OnGUI()
    //{
    //    if (GUILayout.Button("-----下载", GUILayout.Width(200), GUILayout.Height(50)))
    //    {
    //        string url = "https://s.banbanfriend.com/Geo.zip";
    //        SetModelurl(url);
    //    }
    //    if (GUILayout.Button("-----归位",GUILayout.Width(200),GUILayout.Height(50)))
    //    {
    //        ResetModelRotate();
    //    }
    //    if (GUILayout.Button("-----锁定旋转", GUILayout.Width(200), GUILayout.Height(50)))
    //    {
    //        TurnOnModelRotateState(state);
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
    /// 模型归位
    /// </summary>
    public void ResetModelRotate()
    {
        GameManager.Instance.inputManage.ResetModelRotateEvent?.Invoke();
    }

    /// <summary>
    /// 旋转锁定事件
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
}
