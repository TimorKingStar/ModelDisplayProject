using System.Collections;
using System.Collections.Generic;
using TriLibCore.Interfaces;
using UnityEngine;

public class CameraController : MonoSingleton<CameraController>
{
    Camera mainCamera;
    public override void Init()
    {
        mainCamera = GetComponent<Camera>();
        _defaltPosition = mainCamera.transform.position;
        _defaltQuaternion = mainCamera.transform.rotation;
    }
    
    void OnEnable()
    {
        InputManage.Instance.RotateCameraEvent.AddListener(RotateAroundCamera);
        InputManage.Instance.ResetCameraRotateEvent.AddListener( ResetCameraTransform);
        InputManage.Instance.TurnOnCameraRotateEvent.AddListener(SetRotateState);
        InputManage.Instance.TouchZoomScaleEvent.AddListener(ZoomInOut);
    }



    [SerializeField]
    GameObject currentObj;
    [SerializeField]
    Vector3 beforeInitPos;
    Quaternion beforeInitQua;

    [SerializeField]
    Vector3 afterInitPos;
    Quaternion afterInitQua;
    

    [SerializeField]
    Vector3 _defaltPosition;
    Quaternion _defaltQuaternion;
    ICamera cameraInfo;


    /// <summary>
    /// 设置相机信息
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="qua"></param>
    public void SetCameraInfo(GameObject obj, Transform trans = null, ICamera camera = null)
    {
        if (camera != null)
        {
            ICameraInfo = camera;
            SetCameraInfo();
        }

        if (trans != null)
        {   
            beforeInitPos = trans.position;
            beforeInitQua = QuaterConverter(trans.rotation);
        }
        else
        {
            beforeInitPos = _defaltPosition;
            beforeInitQua = _defaltQuaternion;
        }

        currentObj = obj;
        InitCameraInfo();
    }

    Quaternion QuaterConverter(Quaternion qua)
    {
        var angle = qua.eulerAngles;
        Vector3 targetAngle = new Vector3();
        targetAngle.x = angle.z;
        targetAngle.z = angle.x * -1;
        targetAngle.y = angle.y - 90f;
        return Quaternion.Euler(targetAngle);
    }

    ICamera ICameraInfo;
    void SetCameraInfo()
    {
        mainCamera.usePhysicalProperties = ICameraInfo.PhysicalCamera;
        mainCamera.fieldOfView = ICameraInfo.FarClipPlane;
        mainCamera.focalLength = ICameraInfo.FocalLength;

        //mainCamera.aspect = info.AspectRatio;
        //mainCamera.orthographic = info.Ortographic;
        //mainCamera.nearClipPlane = info.NearClipPlane;
        //mainCamera.farClipPlane = info.FarClipPlane;
        //mainCamera.focalLength = info.FocalLength;
        //mainCamera.sensorSize = info.SensorSize;
        //mainCamera.lensShift = info.LensShift;
        //mainCamera.gateFit = info.GateFitMode;
    }

    /// <summary>
    /// 初始化相机位置
    /// </summary>
    public void InitCameraInfo()
    {   
        mainCamera.transform.position = beforeInitPos;
        mainCamera.transform.rotation = beforeInitQua;

        intervalDistance = Vector3.Distance(beforeInitPos, currentObj.transform.position);

        targetRotationX = beforeInitQua.eulerAngles.y;
        targetRotationY = beforeInitQua.eulerAngles.x;

        targetRotationY = Mathf.Clamp(targetRotationY, minVerticalAngle, maxVerticalAngle);
        mainCamera.transform.rotation = Quaternion.Euler(targetRotationY, targetRotationX, 0f);
        mainCamera.transform.position = currentObj.transform.position - mainCamera.transform.forward * intervalDistance;
        afterInitPos = mainCamera.transform.position;
        afterInitQua = mainCamera.transform.rotation;
    }


    float minView = 10f;
    float maxView = 110f;
    
    public void SetViewRange(float minView, float maxView)
    {
        this.minView = minView;
        this.maxView = maxView;
    }


    /// <summary>
    /// value 小于0是放大，value大于0是缩小
    /// </summary>
    /// <param name="value"></param>
    public void ZoomInOut(float value)
    {
        float viewValue = mainCamera.fieldOfView + value;
        mainCamera.fieldOfView = Mathf.Clamp(viewValue, minView, maxView);
    }
    
     float rotationSpeed = 50;
     float maxVerticalAngle = 60f;
     float minVerticalAngle = 0f;
     

    [SerializeField]
    float targetRotationX = 0f;
    [SerializeField]
    float targetRotationY = 0f;
    [SerializeField] 
    float intervalDistance;

     
     bool openRotateState=true;

    public void ResetCameraTransform()
    {
        mainCamera.transform.position = afterInitPos;
        mainCamera.transform.rotation = afterInitQua;
        targetRotationX = afterInitQua.eulerAngles.y;
        targetRotationY = afterInitQua.eulerAngles.x;

        SetCameraInfo();
    }

    public void SetRotateState(bool state)
    {
        openRotateState = state;
    }

    /// <summary>
    /// 相机围绕目标旋转
    /// </summary>
    /// <param name="dir"></param>
    public void RotateAroundCamera(Vector2 dir)
    {   
        if (!openRotateState)
        {
            return;
        }

        if (currentObj == null)
        {
            return;
        }

        if (currentObj)
        {
            targetRotationX += dir.x * rotationSpeed ;
            targetRotationY += dir.y * rotationSpeed ;

            targetRotationY = ClampAngle(targetRotationY, minVerticalAngle, maxVerticalAngle);
            Quaternion targetRotation = Quaternion.Euler(targetRotationY, targetRotationX, 0f);
            mainCamera.transform.rotation = targetRotation;

            // mainCamera.transform.position = currentObj.transform.position - mainCamera.transform.forward * intervalDistance;
            mainCamera.transform.position = targetRotation * new Vector3(0.0f, 0.0f, -intervalDistance) + currentObj.transform.position;
        }
    }

    static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}
