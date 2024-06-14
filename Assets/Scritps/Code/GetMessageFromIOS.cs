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


    private void OnGUI()
    {
        if (GUILayout.Button("-----下载",GUILayout.Width(200),GUILayout.Height(50)))
        {
            string url = "https://s.banbanfriend.com/Geo.zip";
            SetModelurl(url);
        }
    }



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
    /// 设置灯光旋转
    /// </summary>
    /// <param name="x">水平方向旋转</param>
    /// <param name="y">垂直方向旋转</param>
    public void SetLightMoveDir(string x,string y)
    {
        if (float.TryParse(x, out dirX) && float.TryParse(y, out dirY))
        {
            dir.x = dirX; dir.y = dirY;
            GameManager.Instance.lightController.Rotate(dir);
        }
        else
        {  Debug.Log(">>>>>>>Get ios SetLightMoveDir data error");
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
