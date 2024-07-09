
using System.Collections;
using System.Collections.Generic;
using TriLibCore.Dae.Schema;
using TriLibCore.Interfaces;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class CameraOrbit : MonoBehaviour
{

    public GameObject target;
    public float initDistance = 2.4f;
    public float minDistance=0.5f;
    public float maxDistance=20f;
    public float distance;
    
    public float xSpeed = 5f;
    public float ySpeed = 2f;
     
    public float panSpeed=0.1f;
    public float zoomSpeed=0.13f;
    
    public bool useable=false;
    
    public float velocityPanX;
    public float velocityPanY;
    
    Vector3 tempPanPosition;

    UnityEngine.Camera mainCamera;
    public Transform targetPanCam;

    float prevDistance;
    
    public float xRotation = 0.0f;
    public float yRotation = 0.0f;

   public  float initX;
   public float initY;

    private void Awake()
    {
        mainCamera=GetComponentInChildren<Camera>();
        targetPanCam = mainCamera.transform;
        distance=initDistance;
    }
    
    
    ICamera _camera;
    Vector3 beforeInitPos;
    Quaternion beforeInitQua;

    Vector3 _defaltPosition;
    Quaternion _defaltQuaternion;
    
       
    public Transform trans;
    public ICamera  camera;

    public void SetCameraInfo(Transform root,Transform trans , ICamera camera )
    {    
        
        target = root.gameObject;
        this.camera=camera;
        this.trans=trans; 

        if (camera != null)
        {
            _camera = camera;
            SetCameraInfo();
        }
        
        if (trans != null)
        {   
            ///相机带过来的position
            beforeInitPos = trans.position;
           UnityEngine.Debug.Log("position: "+trans.position.x);
            ///相机带过来的rotation 
            beforeInitQua = QuaterConverter(trans.rotation);
             //beforeInitQua = trans.rotation;
        }    
        else
        {
            beforeInitPos = _defaltPosition;
            beforeInitQua = _defaltQuaternion;
        }
                         

        //initDistance = Vector3.Distance(root.position,trans.position);
        InitCameraInfo();
    }
      
    void SetCameraInfo()
    {
        mainCamera.usePhysicalProperties = _camera.PhysicalCamera;
        mainCamera.fieldOfView = _camera.FarClipPlane;
        mainCamera.focalLength = _camera.FocalLength;
    }
    
    Vector3 afterinitpos;
    Quaternion  afterInitQua;
    public void InitCameraInfo()
    {   
        transform.position = beforeInitPos;
        transform.rotation = beforeInitQua;
        
        
       initDistance =  Vector3.Distance(beforeInitPos, target.transform.position);
        distance=initDistance;

       initY = transform.rotation.eulerAngles.y; 
       initX = transform.rotation.eulerAngles.x;
        
       yRotation = initX;   
       xRotation = initY;
       
        var ration= Quaternion.Euler(yRotation, xRotation, 0f); 
         Debug.DrawLine(ration * new Vector3(0.0f, 0.0f, - distance) + target.transform.position,target.transform.position,Color.red);
        
        transform.rotation = ration;       
        transform.position = ration * new Vector3(0.0f, 0.0f, - distance)+target.transform.position;
       
       //这里可以实现
       // mainCamera.transform.position=beforeInitPos;
       
       // mainCamera.transform.rotation=beforeInitQua;

        //velocityPanX = beforeInitPos.x;
       // velocityPanY = beforeInitPos.y;
        
        ///计算之后的默认位置
        afterinitpos = transform.position;
        afterInitQua = transform.rotation;
        

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

    void LateUpdate()
    {
        if (target == null || !useable)
        {
            return;
        }

#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            var rotateX = Input.GetAxis("Horizontal") * Time.deltaTime;
            var rotateY = Input.GetAxis("Vertical") * Time.deltaTime;

            xRotation += rotateX * xSpeed;
            yRotation -= rotateY * ySpeed; 
            
            Debug.Log(xRotation);
            var rot = Quaternion.Euler(yRotation,xRotation , 0);
            var po = rot * new Vector3(0.0f, 0.0f, -distance) + target.transform.position;
            transform.rotation = rot;
            transform.position = po;
        }

        float z = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed * Time.deltaTime;
        ZoomInOut(z);

        //pan 
        if (Input.GetMouseButton(1))
        {
            var panX = Input.GetAxis("Mouse X") * Time.deltaTime;
            var panY = Input.GetAxis("Mouse Y") * Time.deltaTime;


            velocityPanX += panX * panSpeed;
            Debug.Log(velocityPanX);
            velocityPanY += panY * panSpeed;
        }
#endif
       
        if (System. Math.Abs(prevDistance - distance) > 0.001f)
        {
            prevDistance = distance; 
            var rot = Quaternion.Euler(yRotation, xRotation, 0);
            var po = rot * new Vector3(0.0f, 0.0f, -distance) + target.transform.position;
            transform.rotation = rot;
            transform.position = po; 
        }

        if (Input.touchCount == 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                var xMove = Input.GetTouch(0).deltaPosition.x * 0.02f;
                var yMove = Input.GetTouch(0).deltaPosition.y * 0.02f;

                if (target && useable)
                {
                    xRotation += xMove * xSpeed;
                    yRotation -= yMove * ySpeed; 
                    
                    var rot = Quaternion.Euler(yRotation, xRotation, 0);
                    var po = rot * new Vector3(0.0f, 0.0f, -distance) + target.transform.position;
                    transform.rotation = rot;
                    transform.position = po;
                }
            }
        }
        else if (Input.touchCount == 2)
        {

            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            if (touchZero.phase == TouchPhase.Moved && touchOne.phase == TouchPhase.Moved)
            {
                Vector2 touchDelta1 = touchZero.deltaPosition;
                Vector2 touchDelta2 = touchOne.deltaPosition;

                bool sameDirection = Vector2.Dot(touchDelta1.normalized, touchDelta2.normalized) < 0;
                if (sameDirection)
                {
                    //缩放
                    Vector2 touchZeroPrevPos = touchZero.position - touchDelta1;
                    Vector2 touchOnePrevPos = touchOne.position - touchDelta2;

                    float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                    float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                    float deltaMagnitudeDiff = (prevTouchDeltaMag - touchDeltaMag) * 0.02f;

                    ZoomInOut(deltaMagnitudeDiff);

                    Debug.Log(">>>>>>deltaMagnitudeDiff: " + deltaMagnitudeDiff + "______distance:" + distance);
                }
                else
                {

                    
                    float veloDeltaX = (touchZero.deltaPosition.x + touchOne.deltaPosition.x) / 2;
                    float veloDeltaY = (touchZero.deltaPosition.y + touchOne.deltaPosition.y) / 2;

                    velocityPanX += veloDeltaX * panSpeed * 0.02f;
                    velocityPanY += veloDeltaY * panSpeed * 0.02f;

                    Debug.Log("velocityPanX: " + velocityPanX + "________velocityPanY" + velocityPanY);

                }
            }
        }
        
        tempPanPosition = new Vector3( -velocityPanX * distance / 10, -velocityPanY * distance / 10, 0);
        if (targetPanCam != null && useable)
        {
            targetPanCam.transform.localPosition = tempPanPosition;
        }
    }


    public void SetCameraState(bool openMovent)
    {
        useable = openMovent;
    }
      

    void ZoomInOut(float value)
    {
        distance = Mathf.Clamp(distance += value * zoomSpeed, minDistance, maxDistance);
    }
    
    public void ResetCameraInfo()
    {
        distance = initDistance;
        yRotation = initX;
        xRotation = initY;

        velocityPanX = 0;
        velocityPanY = 0;
        
        var rotation = Quaternion.Euler(yRotation, xRotation, 0);
        var position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.transform.position;
        transform.rotation = rotation;
        transform.position = position;
    }

}
