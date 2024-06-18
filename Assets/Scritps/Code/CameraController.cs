using System.Collections;
using System.Collections.Generic;
using TriLibCore.Interfaces;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Camera mainCamera;
    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
        _defaltPosition = mainCamera.transform.position;
        _defaltQuaternion = mainCamera.transform.rotation;
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
            SetCameraInfo(camera);
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

    void SetCameraInfo(ICamera info)
    {
        mainCamera.usePhysicalProperties = info.PhysicalCamera;
        //mainCamera.aspect = info.AspectRatio;
        //mainCamera.orthographic = info.Ortographic;
        //mainCamera.fieldOfView = info.FarClipPlane;
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
    
     float rotationSpeed = 200f;
     float maxVerticalAngle = 60f;
     float minVerticalAngle = -60f;
     float lerpSpeed = 200f;

    [SerializeField]
    float targetRotationX = 0f;
    [SerializeField]
    float targetRotationY = 0f;
    [SerializeField] 
    float intervalDistance;

    [SerializeField] 
     bool openRotateState;


    public void ResetCameraTransform()
    {
        mainCamera.transform.position = afterInitPos;
        mainCamera.transform.rotation = afterInitQua;

        targetRotationX = afterInitQua.eulerAngles.y;
        targetRotationY = afterInitQua.eulerAngles.x;
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
            targetRotationX += dir.x * rotationSpeed * Time.deltaTime;
            targetRotationY += dir.y * rotationSpeed * Time.deltaTime;

            targetRotationY = Mathf.Clamp(targetRotationY, minVerticalAngle, maxVerticalAngle);

            Quaternion targetRotation = Quaternion.Euler(targetRotationY, targetRotationX, 0f);
            mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, targetRotation, lerpSpeed * Time.deltaTime);

            mainCamera.transform.position = currentObj.transform.position - mainCamera.transform.forward * intervalDistance;
        }
    }

}
