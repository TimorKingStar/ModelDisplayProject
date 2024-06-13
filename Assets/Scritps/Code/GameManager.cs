using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ��������˶��¼�
/// </summary>
public class GameManager : MonoSingleton<GameManager>
{
    public InputManage inputManage;
    public CameraController cameraController;
    public LightController lightController;

    public Material currentMaterial;
    public Material GetMaterial()
    {
        return currentMaterial;
    }
    private void OnEnable()
    {
        inputManage.touchZoomScaleEvent.AddListener(cameraController.ZoomInOut);
        //inputManage.moveDirectionEvent.AddListener(cameraController.RotateModel);
       // inputManage.moveDirectionEvent.AddListener(lightController.Rotate);
    }

    private void OnDisable()
    {
        inputManage.touchZoomScaleEvent.RemoveListener(cameraController.ZoomInOut);
        //inputManage.moveDirectionEvent.RemoveListener(cameraController.RotateModel);
        //inputManage.moveDirectionEvent.RemoveListener(lightController.Rotate);
    }
}
