using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{

    Light directionalLight;
    private void Awake()
    {
        directionalLight = GetComponent<Light>();
        offset = 1;
    }

    public float offset;
    public void SetOffSet(float offset)
    {
       this.offset = offset;
        Debug.Log("offset: "+offset);
    }

   public void Rotate(Vector2 dir)
   {
        dir.x *= offset;
        dir.y *= offset;

        directionalLight.transform.Rotate(Vector3.up, dir.x, Space.World);
        directionalLight.transform.Rotate(Vector3.right, dir.y, Space.World);
    }

}
