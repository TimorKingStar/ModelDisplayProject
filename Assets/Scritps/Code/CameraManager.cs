using System.Collections;
using System.Collections.Generic;
using TriLibCore.Interfaces;
using UnityEngine;

public class CameraManager : MonoSingleton<CameraManager>
{
    
    SmoothOrbitCam smoothOrbitCam;
    public SmoothOrbitViewchanger viewchanger;
    public override void Init()
    {  
       
       smoothOrbitCam = GetComponent<SmoothOrbitCam>();
    }
    
    void OnEnable()
    {
        InputManage.Instance.ResetCameraRotateEvent.AddListener( ResetTransform);
        InputManage.Instance.TurnOnCameraRotateEvent.AddListener(SetRotateState);
    }
    public float defaletDis;

    public bool rotateState;
    void Start()
    {
        smoothOrbitCam.distance=defaletDis;
    }

     public void ResetTransform()
     {
        viewchanger.TriggerViewChange();
     }
        
     public void SetRotateState(bool state)
     {
        smoothOrbitCam.useable=state;
     }

#region  待定

      GameObject camGO;
     public void SetCameraInfo(Transform cam,Transform root)
     {
         if(camGO==null)
         {
              camGO=new GameObject("CAM");
         }
         
         camGO.transform.position=cam.position+new Vector3(0,-1,0);
         camGO.transform.rotation=QuaterConverter(cam.rotation);
         var angle =camGO.transform.rotation.eulerAngles;
         Debug.Log("angle: "+angle.ToString());
         //viewchanger.Init(angle,Vector3.Distance(camGO.transform.position,root.position));
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
     #endregion
     
}
