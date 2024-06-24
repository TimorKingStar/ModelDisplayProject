using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 管理相机运动事件
/// </summary>
public class GameManager : MonoSingleton<GameManager>
{
    public InputManage inputManage;
    public CameraController cameraController;
    public LightController lightController;
    public Material alphaMaterial;


    private void OnEnable() // this only works if the Camera Controller is set up correctly
    {
        try
        {
            inputManage.ResetCameraRotateEvent.AddListener(cameraController.ResetCameraTransform);
            inputManage.TouchZoomScaleEvent.AddListener(cameraController.ZoomInOut);
            inputManage.RotateCameraEvent.AddListener(cameraController.RotateAroundCamera);
            inputManage.TurnOnCameraRotateEvent.AddListener(cameraController.SetRotateState);
            inputManage.CancleLoadedModelEvent.AddListener(AssetLoadManager.Instance.CancleDownload);
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
            inputManage.TouchZoomScaleEvent.RemoveListener(cameraController.ZoomInOut);
            inputManage.RotateCameraEvent.RemoveListener(cameraController.RotateAroundCamera);
            inputManage.TurnOnCameraRotateEvent.RemoveListener(cameraController.SetRotateState);
            inputManage.CancleLoadedModelEvent.RemoveListener(AssetLoadManager.Instance.CancleDownload);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
}
