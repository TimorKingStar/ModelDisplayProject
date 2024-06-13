using System.Collections;
using System.Collections.Generic;
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
    Vector3 initPos;
    Quaternion initQua;

    [SerializeField]
    Vector3 _defaltPosition ;
    Quaternion _defaltQuaternion;

    /// <summary>
    /// 设置相机信息
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="qua"></param>
    public void SetCameraInfo(GameObject obj,Transform trans=null)
    {
        if (trans != null)
        {
            initPos = trans.localPosition;
            initQua = trans.localRotation;
        }
        else
        {
            initPos = _defaltPosition;
            initQua = _defaltQuaternion;
        }
        currentObj = obj;
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
    
     float rotationSpeed = 10f;
     float maxVerticalAngle = 60f;
     float minVerticalAngle = -60f;
     float lerpSpeed = 200f;

    [SerializeField]
    float targetRotationX = 0f;
    [SerializeField]
    float targetRotationY = 0f;
    [SerializeField] 
    float intervalDistance;


    /// <summary>
    /// 相机围绕目标旋转
    /// </summary>
    /// <param name="dir"></param>
    public void RotateModel(Vector2 dir)
    {
        //if (currentObj == null)
        //{
        //    return;
        //}
        //if (currentObj)
        //{
        //    targetRotationX += dir.x * rotationSpeed * Time.deltaTime;
        //    targetRotationY += dir.y * rotationSpeed * Time.deltaTime;

        //    targetRotationY = Mathf.Clamp(targetRotationY, minVerticalAngle, maxVerticalAngle);

        //    Quaternion targetRotation = Quaternion.Euler(targetRotationY, targetRotationX, 0f);
        //    mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, targetRotation, lerpSpeed * Time.deltaTime);

        //    mainCamera.transform.position = currentObj.transform.position - mainCamera.transform.forward * intervalDistance;
        //}
    }

}
