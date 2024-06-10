using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 管理相机运动事件
/// </summary>
public class GameManager : MonoBehaviour
{
    public InputManage inputManage;
    public GameObject target;
    CameraController cameraController;
    public LightController lightController;
    private void Awake()
    { 
        cameraController = GetComponent<CameraController>();
    }
    private void Start()
    {
        cameraController.SetCurrentMode(target);
    }
    private void OnEnable()
    {
        inputManage.touchZoomScaleEventl.AddListener(cameraController.ZoomInOut);
        inputManage.moveDirectionEvent.AddListener(cameraController.RotateModel);
        inputManage.moveDirectionEvent.AddListener(lightController.Rotate);

    }

    private void OnDisable()
    {
        inputManage.touchZoomScaleEventl.RemoveListener(cameraController.ZoomInOut);
        inputManage.moveDirectionEvent.RemoveListener(cameraController.RotateModel);
        inputManage.moveDirectionEvent.RemoveListener(lightController.Rotate);
    }
}
