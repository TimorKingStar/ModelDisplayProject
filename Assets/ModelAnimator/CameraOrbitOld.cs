//  A simple Unity C# script for orbital movement around a target gameobject
//  Author: Ashkan Ashtiani
//  Gist on Github: https://gist.github.com/3dln/c16d000b174f7ccf6df9a1cb0cef7f80

using System;
using UnityEngine;

         
public class CameraOrbitOld : MonoBehaviour
{
    public GameObject target;
        
    public float initDistance=2.4f;
    public float distance ;

    public float xSpeed = 250.0f;
    public float ySpeed = 120.0f;

    public float yMinLimit = -20;
    public float yMaxLimit = 80;

    float x = 0.0f;
    float y = 0.0f;

    float initX;
    float initY;

    void Start()
    {
        var angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        initDistance = Vector3.Distance(transform.position, target.transform.position);
        distance = initDistance;

        initX = x;
        initY = y;
    }

    private void OnEnable()
    { 
        // InputManage.Instance.RotateCameraEvent.AddListener(RotateCamera);
        // InputManage.Instance.ResetCameraRotateEvent.AddListener(ResetCameraInfo);
        // InputManage.Instance.TouchZoomScaleEvent.AddListener(ZoomInOut);
        // InputManage.Instance.TurnOnCameraRotateEvent.AddListener(SetCameraState);
    }
     

    float prevDistance;

    void LateUpdate()
    {
        if (Math.Abs(prevDistance - distance) > 0.001f)
        {
            prevDistance = distance;
            var rot = Quaternion.Euler(y, x, 0);
            var po = rot * new Vector3(0.0f, 0.0f, -distance) + target.transform.position;
            transform.rotation = rot;
            transform.position = po;
        }

        if (Input.touchCount == 1)
        {
           var x = Input.GetTouch(0).deltaPosition.x *(Time.deltaTime / (Input.GetTouch(0).deltaTime + 0.001f));
           var y = Input.GetTouch(0).deltaPosition.y*(Time.deltaTime / (Input.GetTouch(0).deltaTime + 0.001f));
           RotateCamera(new Vector2(x,y));

        }
        else if(Input.touchCount == 2)
        {
            
        }
    }


    bool openMovent=true;
    void SetCameraState(bool openMovent)
    {
      this.openMovent = openMovent;
    }
  
    private void RotateCamera(Vector2 move)
    {
        if (target && openMovent)
        {
            x += move.x * xSpeed ;
            y -= move.y * ySpeed ;
             
            y = ClampAngle(y, yMinLimit, yMaxLimit);
            var rotation = Quaternion.Euler(y, x, 0);
            var position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.transform.position;
            transform.rotation = rotation;
            transform.position = position;
        }
    }

    public void ZoomInOut(float value)
    {
        distance += value * 2;
        if (distance<2)
        {
            distance = 2;
        }
    }

    public void ResetCameraInfo()
    {
        distance = initDistance;
        x = initX;
        y = initY;

        var rotation = Quaternion.Euler(initY, initX, 0);
        var position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.transform.position;
        transform.rotation = rotation;
        transform.position = position;
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
