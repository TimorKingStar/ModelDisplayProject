using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Camera mainCamera;
    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
    }

    Vector3 initPos;
    Quaternion initQua;

    /// <summary>
    /// 设置相机信息
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="qua"></param>
    public void SetCameraInfo(Vector3 pos, Quaternion qua)
    {
        initPos = pos;
        initQua = qua;
        InitCameraInfo();
    }

    /// <summary>
    /// 初始化相机位置
    /// </summary>
    public void InitCameraInfo()
    {
        mainCamera.transform.position = initPos;
        mainCamera.transform.rotation = initQua;
    }

    float minView = 30f;
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

    GameObject currentObj;

    public void SetCurrentMode(GameObject obj)
    {
        currentObj = obj;
    }

     float rotationSpeed = 5f;
     float maxVerticalAngle = 60f;
     float minVerticalAngle = -60f;
     float lerpSpeed = 200f;
    
    float targetRotationX = 0f;
    float targetRotationY = 0f;
    float intervalDistance = 10;

    public void SetCamera2TargetDis(float dis)
    {
        intervalDistance = dis;
    }

    /// <summary>
    /// 相机围绕目标旋转
    /// </summary>
    /// <param name="dir"></param>
    public void RotateModel(Vector2 dir)
    {
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
