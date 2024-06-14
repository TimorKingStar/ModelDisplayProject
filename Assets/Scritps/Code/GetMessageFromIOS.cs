using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class GetMessageFromIOS : MonoBehaviour
{
    
     [DllImport("__Internal")]
     private static extern void CallObjCFunc(string funcName);


    //��Ϸ�������ƣ�GetMessageFromIOS  �������ƺͲ������£�
    float offset;
    Vector2 dir = new Vector2();
    float dirX, dirY;


    private void OnGUI()
    {
        if (GUILayout.Button("-----����",GUILayout.Width(200),GUILayout.Height(50)))
        {
            string url = "https://s.banbanfriend.com/Geo.zip";
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
    /// ���õƹ���ת
    /// </summary>
    /// <param name="x">ˮƽ������ת</param>
    /// <param name="y">��ֱ������ת</param>
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
