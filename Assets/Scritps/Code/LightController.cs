using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{

    Light directionalLight;
    private void Awake()
    {
        directionalLight = GetComponent<Light>();
    }

   public void Rotate(Vector2 dir)
    {
        directionalLight.transform.Rotate(Vector3.up, dir.x, Space.World);
        directionalLight.transform.Rotate(Vector3.right, dir.y, Space.World);
    }

}
