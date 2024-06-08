using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    Camera mainCamera;
    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
    }
    Vector3 initPos;
    Quaternion initQua;

    /// <summary>
    /// ���������Ϣ
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
    /// ��ʼ�����λ��
    /// </summary>
    public void InitCameraInfo()
    {
        mainCamera.transform.position = initPos;
        mainCamera.transform.rotation = initQua;
    }

    float minView = 40f;
    float maxView = 90f;

    public void SetViewRange(float minView,float maxView)
    {
        this.minView = minView;
        this.maxView = maxView;
    }

    /// <summary>
    /// value С��0�ǷŴ�value����0����С
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

    public void RotateModel(Vector2 dir)
    {
        if (currentObj==null)
        {
            return;
        }

    }
}
