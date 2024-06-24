using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 管理相机运动事件
/// </summary>
public class GameManager : MonoSingleton<GameManager>
{


    private void OnEnable() // this only works if the Camera Controller is set up correctly
    {
        try
        {
           
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    private void OnDisable()
    {
        try
        {
            
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
}
