using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
     

    public Transform target;

    void Start()
    {
       
    }
    
    Vector3 x_rotationAxis;
    Vector3 y_rotationAxis;
    public float xSpeed;
    public float ySpeed;
    
    void Update()
    {

        if (Input.touchCount == 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                var xMove = Input.GetTouch(0).deltaPosition.x * xSpeed;
                var yMove = Input.GetTouch(0).deltaPosition.y * ySpeed;

                 if(yMove!=0)
                 {  
                   y_rotationAxis  =  Vector3.Cross(target.position-transform.position,transform.up);
                   transform.RotateAround(target.position,y_rotationAxis,yMove);
                }
          
                if(xMove !=0 )
                {  
                   x_rotationAxis  =  Vector3.Cross(target.position-transform.position,transform.right);
                   transform.RotateAround(target.position,x_rotationAxis,xMove);
                }
            }
        }
    }

}
