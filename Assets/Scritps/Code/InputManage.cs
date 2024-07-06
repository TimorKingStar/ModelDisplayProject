using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputManage : MonoSingleton<InputManage>
{

    public Material alphaMaterial;
    public UnityEvent<bool> PlayAnimationEvent;
    
    public UnityEvent<float> AnimationLengthOfClipEvent;

    public UnityEvent<string, bool> SetHeadLayerShowEvent;
    public UnityEvent<float> OutLineStateEvent;

    public UnityEvent<float> AlphaStateEvent;

    public UnityEvent ResetCameraRotateEvent;

    public UnityEvent<bool> TurnOnCameraRotateEvent;
    public UnityEvent CancleLoadedModelEvent;

    public UnityEvent ResetHeadLayerShowEvent;

    public UnityEvent<float> SetOutlineWidthEvent;

     
     public UnityEvent<float> SetLightIntensityEvent;
    public UnityEvent<float> PlayApppointAnimEvent;
    private void OnDisable()
    {
        PlayAnimationEvent.RemoveAllListeners();
        AnimationLengthOfClipEvent.RemoveAllListeners();
        SetHeadLayerShowEvent.RemoveAllListeners();
       
        OutLineStateEvent.RemoveAllListeners();
        AlphaStateEvent.RemoveAllListeners();
        ResetCameraRotateEvent.RemoveAllListeners();
        TurnOnCameraRotateEvent.RemoveAllListeners();
        CancleLoadedModelEvent.RemoveAllListeners();
        ResetHeadLayerShowEvent.RemoveAllListeners();
        SetOutlineWidthEvent.RemoveAllListeners();
        PlayApppointAnimEvent.RemoveAllListeners();
        SetLightIntensityEvent.RemoveAllListeners();
    }
 
 #region  删除

//     float scaleFactor;
//     float factorSpeed=2;
//     Touch lastTouch_1;
//     Touch lastTouch_2;


//     Touch currentTouch_1;
//     Touch currentTouch_2;




//     /// <summary>
//     /// �Ŵ���С��������ģ�͵Ĵ�С��Ҳ�����������View
//     /// </summary>
//     void TouchZoom()
//     {
// #if UNITY_EDITOR
//         TouchZoomScaleEvent?.Invoke(Input.GetAxis("Mouse ScrollWheel")*2f); 
// #endif

//         if (Input.touchCount == 2)
//         {
//             currentTouch_1 = Input.GetTouch(0);
//             currentTouch_2 = Input.GetTouch(1);
//             if (currentTouch_2.phase == TouchPhase.Began)
//             {
//                 lastTouch_2 = currentTouch_2;
//                 lastTouch_1 = currentTouch_1;
//                 return;
//             }

//             scaleFactor = (Vector2.Distance(lastTouch_1.position, lastTouch_2.position)
//                 - Vector2.Distance(currentTouch_1.position, currentTouch_2.position))*Time.deltaTime* factorSpeed;

//             TouchZoomScaleEvent?.Invoke(scaleFactor);
//             lastTouch_1 = currentTouch_1;
//             lastTouch_2 = currentTouch_2;
//         }
//     }

//     void Test()
//     {
//         moveX = Input.GetAxis("Horizontal")*Time.deltaTime;
//         moveY = Input.GetAxis("Vertical") * Time.deltaTime;
//         moveDirection.x = moveX;
//         moveDirection.y = -moveY;
//         RotateCameraEvent?.Invoke(moveDirection);
//     } 
     
     
//     /// <summary>
//     /// ������Ļ��ʱ����תģ�ͻ��ߵƹ�
//     /// </summary>
//     void TouchSlider()
//     {

// #if UNITY_EDITOR
//         Test();
// #endif
//         if (Input.touchCount == 1)
//         {   
//             if (Input.GetTouch(0).phase == TouchPhase.Moved)
//             {
//                 MoveCameraStateEvent?.Invoke(true);
//                 moveX = Input.GetAxis("Mouse X")*Time.deltaTime ;
//                 moveY = Input.GetAxis("Mouse Y") * Time.deltaTime;

//                 moveDirection.x = moveX;
//                 moveDirection.y = -moveY;
//                 RotateCameraEvent?.Invoke(moveDirection);
                
//             }
//             else
//             {
//                 MoveCameraStateEvent?.Invoke(false);
//                 Debug.Log("Touch move sphase: "+Input.GetTouch(0).phase);
//             }
//         }
        
//     }
#endregion

}
