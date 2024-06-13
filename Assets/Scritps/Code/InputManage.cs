using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputManage : MonoBehaviour
{
    [SerializeField]
    float moveX;
    [SerializeField]
    float moveY;
    Vector2 moveDirection = new Vector2();

    public UnityEvent<Vector2> moveDirectionEvent;
    public UnityEvent<float> touchZoomScaleEvent;
    public UnityEvent<float> outLineStateEvent;
    public UnityEvent<float> modelAlphaStateEvent;

    void Update()
    {
        TouchZoom();
        TouchSlider();
    }

    float scaleFactor;
    Touch lastTouch_1;
    Touch lastTouch_2;

    Touch currentTouch_1;
    Touch currentTouch_2;

    /// <summary>
    /// 放大缩小，可以是模型的大小，也可以是相机的View
    /// </summary>
    void TouchZoom()
    {
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
                - Vector2.Distance(currentTouch_1.position, currentTouch_2.position));

            touchZoomScaleEvent?.Invoke(scaleFactor);
            lastTouch_1 = currentTouch_1;
            lastTouch_2 = currentTouch_2;
        }
    }

    void Test()
    {
        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");
        moveDirection.x = moveX;
        moveDirection.y = moveY;
        moveDirectionEvent?.Invoke(moveDirection);
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
                moveX = Input.GetAxis("Mouse X") ;
                moveY = Input.GetAxis("Mouse Y") ;
                moveDirection.x = moveX;
                moveDirection.y = moveY;
                moveDirectionEvent?.Invoke(moveDirection);

                Debug.Log("Mousex : " + moveX);
                Debug.Log("Mousey : " + moveY);
            }
            else
            {   
                Debug.Log("Touch move sphase: "+Input.GetTouch(0).phase);
            }
        }
    }

}
