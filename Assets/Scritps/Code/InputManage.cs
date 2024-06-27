using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputManage : MonoSingleton<InputManage>
{

    public Material alphaMaterial;

    [SerializeField]
    float moveX;
    [SerializeField]
    float moveY;
    
    Vector2 moveDirection = new Vector2();

    public UnityEvent<bool> PlayAnimationEvent;

    public UnityEvent<float> AnimationLengthOfClipEvent;

    /// <summary>
    /// 分层显示事件
    /// </summary>
    public UnityEvent<string, bool> SetHeadLayerShowEvent;
    /// <summary>
    /// 相机围绕旋转事件
    /// </summary>
    public UnityEvent<Vector2> RotateCameraEvent;
    /// <summary>
    /// 放大缩小视角事件
    /// </summary>
    public UnityEvent<float> TouchZoomScaleEvent;
    /// <summary>
    /// 显示边缘线事件
    /// </summary>
    public UnityEvent<float> OutLineStateEvent;
    /// <summary>
    /// 设置透明度事件
    /// </summary>
    public UnityEvent<float> AlphaStateEvent;
    /// <summary>
    /// 重置相机位置事件
    /// </summary>
    public UnityEvent ResetCameraRotateEvent;
    /// <summary>
    /// 开启相机旋转事件
    /// </summary>
    public UnityEvent<bool> TurnOnCameraRotateEvent;
    /// <summary>
    /// 取消加载模型事件
    /// </summary>
    public UnityEvent CancleLoadedModelEvent;
    /// <summary>
    /// 重置头部结构
    /// </summary>
    public UnityEvent ResetHeadLayerShowEvent;

    /// <summary>
    /// 设置OutLine宽度
    /// </summary>
    public UnityEvent<float> SetOutlineWidthEvent;

    public UnityEvent<bool> MoveCameraStateEvent;

    private void OnDisable()
    {
        PlayAnimationEvent.RemoveAllListeners();
        AnimationLengthOfClipEvent.RemoveAllListeners();
        SetHeadLayerShowEvent.RemoveAllListeners();
        RotateCameraEvent.RemoveAllListeners();
        TouchZoomScaleEvent.RemoveAllListeners();
        OutLineStateEvent.RemoveAllListeners();
        AlphaStateEvent.RemoveAllListeners();
        ResetCameraRotateEvent.RemoveAllListeners();
        TurnOnCameraRotateEvent.RemoveAllListeners();
        CancleLoadedModelEvent.RemoveAllListeners();
        ResetHeadLayerShowEvent.RemoveAllListeners();
        SetOutlineWidthEvent.RemoveAllListeners();
    }

    private void Start()
    {
        MoveCameraStateEvent?.Invoke(false);
    }
    void LateUpdate()
    {
        TouchZoom();
        TouchSlider();
    }

    float scaleFactor;
    float factorSpeed=2;
    Touch lastTouch_1;
    Touch lastTouch_2;


    Touch currentTouch_1;
    Touch currentTouch_2;




    /// <summary>
    /// 放大缩小，可以是模型的大小，也可以是相机的View
    /// </summary>
    void TouchZoom()
    {
#if UNITY_EDITOR
        TouchZoomScaleEvent?.Invoke(Input.GetAxis("Mouse ScrollWheel")*2f); 
#endif

        if (Input.touchCount == 2)
        {
            currentTouch_1 = Input.GetTouch(0);
            currentTouch_2 = Input.GetTouch(1);
            if (currentTouch_2.phase == TouchPhase.Began)
            {
                lastTouch_2 = currentTouch_2;
                lastTouch_1 = currentTouch_1;
                return;
            }

            scaleFactor = (Vector2.Distance(lastTouch_1.position, lastTouch_2.position)
                - Vector2.Distance(currentTouch_1.position, currentTouch_2.position))*Time.deltaTime* factorSpeed;

            TouchZoomScaleEvent?.Invoke(scaleFactor);
            lastTouch_1 = currentTouch_1;
            lastTouch_2 = currentTouch_2;
        }
    }

    void Test()
    {
        moveX = Input.GetAxis("Horizontal")*Time.deltaTime;
        moveY = Input.GetAxis("Vertical") * Time.deltaTime;
        moveDirection.x = moveX;
        moveDirection.y = -moveY;
        RotateCameraEvent?.Invoke(moveDirection);
    } 
     
     
    /// <summary>
    /// 滑动屏幕的时候，旋转模型或者灯光
    /// </summary>
    void TouchSlider()
    {

#if UNITY_EDITOR
        Test();
#endif
        if (Input.touchCount == 1)
        {   
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                MoveCameraStateEvent?.Invoke(true);
                moveX = Input.GetAxis("Mouse X")*Time.deltaTime ;
                moveY = Input.GetAxis("Mouse Y") * Time.deltaTime;

                moveDirection.x = moveX;
                moveDirection.y = -moveY;
                RotateCameraEvent?.Invoke(moveDirection);
                
            }
            else
            {
                MoveCameraStateEvent?.Invoke(false);
                Debug.Log("Touch move sphase: "+Input.GetTouch(0).phase);
            }
        }
        
    }

}
