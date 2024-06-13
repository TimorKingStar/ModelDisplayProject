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

    public void SetModelurl(string url)
    {
        AssetLoadManager.Instance.DownModeFromWeb(url);
    }
    
    //方法名：SetLightMoveDir 
    public void SetLightMoveDir(string x,string y)
    {
        if (float.TryParse(x, out dirX) && float.TryParse(y, out dirY))
        {
            dir.x = dirX;
            dir.y = dirY;
            GameManager.Instance.lightController.Rotate(dir);
        }
        else
        {
            Debug.Log(">>>>>>>Get ios SetLightMoveDir data error");
        }
        
    }

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
