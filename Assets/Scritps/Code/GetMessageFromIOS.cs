using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class GetMessageFromIOS : MonoBehaviour
{
    public GameObject cube;
   
   
   
     [DllImport("__Internal")]
     private static extern void CallObjCFunc(string funcName);
    //游戏物体名称：GetMessageFromIOS  方法名称和参数如下：

    public void SetModelStateBool(bool state)
    {
        cube.gameObject.SetActive(state);
    }

    public void SetModelStateString(string state)
    {
        if (state=="active")
        {
            cube.gameObject.SetActive(true);
        }
        if (state == "close")
        {
            cube.gameObject.SetActive(false);
        }
    }

    public void SetModelStateFloat(float state)
    {
        if (state==1f)
        {
            cube.gameObject.SetActive(true);
        }
        if (state == 0)
        {
            cube.gameObject.SetActive(false);
        }
    }
    public void SetModelStateVector2(Vector2 state)
    {
        if (state.x >0f && state.y>0f)
        { 
            cube.gameObject.SetActive(true);
        }
        if (state.x <= 0f && state.y <= 0f)
        {
            cube.gameObject.SetActive(false);
        }
    }

}
