using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoSingleton<LightController>
{

    Light directionalLight;
    Quaternion initQua;

    public override void Init()
    {
        directionalLight = GetComponent<Light>();
        initQua = directionalLight.transform.rotation;
    }

    public void GetLightInfo()
    {
        GetMessageFromIOS.ReturnLightRotateInfo(GetLightRotation());
    }

    public void ResetLight()
    {
        directionalLight.transform.rotation = initQua;
    }

    string GetLightRotation()
    {
        var dir=  directionalLight.transform.rotation.eulerAngles;
        return dir.x + "_" + dir.y + "_" + dir.z;
    }
    
    public void Rotate(Vector3 dir)
    {   
        directionalLight.transform.Rotate(Vector3.right, dir.x,Space.Self); 
        directionalLight.transform.Rotate(Vector3.up, dir.y, Space.Self);
        directionalLight.transform.Rotate(Vector3.forward, dir.z, Space.Self); 
    }

}
