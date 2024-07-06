using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
     
     public Transform camera;
     public Transform cube;
    
    public float distance;

    public Transform target;
    void Update()
    {
         distance = Vector3.Distance(camera.position,target.position); 
         
         var targetRotationY = camera.eulerAngles.y;
         var targetRotationX = camera.eulerAngles.x;
         
         var ration= Quaternion.Euler(targetRotationX, targetRotationY, 0f);
        
        cube.position=ration*new Vector3(0,0,-distance)+target.position;
         cube.rotation=ration;
    }

}
