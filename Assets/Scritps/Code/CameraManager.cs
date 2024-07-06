using System.Collections;
using System.Collections.Generic;
using TriLibCore.Interfaces;
using UnityEngine;

public class CameraManager : MonoSingleton<CameraManager>
{
    CameraOrbit cameraOrbit;
    public override void Init()
    {  
       cameraOrbit = GetComponent<CameraOrbit>();
    }
    
    void OnEnable()
    {
        InputManage.Instance.ResetCameraRotateEvent.AddListener( ResetTransform);
        InputManage.Instance.TurnOnCameraRotateEvent.AddListener(SetRotateState);
    }
      
      public void SetCameraInfo(Transform root,Transform trans , ICamera camera )
      {
           cameraOrbit.SetCameraInfo(root,trans,camera);
      }
    
     public void ResetTransform()
     {
        cameraOrbit.ResetCameraInfo();
     }
        
     public void SetRotateState(bool state)
     {
         cameraOrbit.SetCameraState(state);
     }
    
     
}
