using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputManage : MonoBehaviour
{
    float moveX;
    float moveY;
    Vector2 moveDirection = new Vector2();

    public UnityEvent<Vector2> moveDirectionEvent;
    public UnityEvent<float> touchZoomScaleEventl;
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
    /// �Ŵ���С��������ģ�͵Ĵ�С��Ҳ�����������View
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

            touchZoomScaleEventl?.Invoke(scaleFactor);
            lastTouch_1 = currentTouch_1;
            lastTouch_2 = currentTouch_2;
        }
    }

    void Test()
    {
        //#if UNITY_EDITOR
        //        moveX = Input.GetAxis("Horizontal") * moveSpeed;
        //        moveY = Input.GetAxis("Vertical") * moveSpeed;
        //        moveDirection.x = moveX;
        //        moveDirection.y = moveY;
        //        moveDirectionEvent?.Invoke(moveDirection);
        //        return;
        //#endif
        moveX = Input.GetAxis("Mouse X");
        moveY = Input.GetAxis("Mouse Y") ;
    }

    /// <summary>
    /// ������Ļ��ʱ����תģ�ͻ��ߵƹ�
    /// </summary>
    void TouchSlider()
    {

#if UNITY_EDITOR
        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical") ;
        moveDirection.x = moveX;
        moveDirection.y = moveY;
        moveDirectionEvent?.Invoke(moveDirection);
        return;
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
