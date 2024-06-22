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

    private void OnEnable()
    {
        inputManage.ResetCameraRotateEvent.AddListener(cameraController.ResetCameraTransform);
        inputManage.TouchZoomScaleEvent.AddListener(cameraController.ZoomInOut);
        inputManage.RotateCameraEvent.AddListener(cameraController.RotateAroundCamera);
        inputManage.TurnOnCameraRotateEvent.AddListener(cameraController.SetRotateState);
        inputManage.CancleLoadedModelEvent.AddListener(AssetLoadManager.Instance.CancleDownload);
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
