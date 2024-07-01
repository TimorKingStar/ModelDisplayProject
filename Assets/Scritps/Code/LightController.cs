using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoSingleton<LightController>
{

    Light directionalLight;
    Quaternion initQua;
    
    public float intensity;
    public override void Init()
    {
        directionalLight = GetComponent<Light>();
        initQua = directionalLight.transform.rotation;
    }
    public void OnEnable()
    {
      
    }

    private void Start()
    {
       intensity=directionalLight.intensity;
    }

    public void SetIntensity(float inten)
    {
        inten=Mathf.Clamp(inten,0.1f,5f);
        directionalLight.intensity=inten;
    }
     private void Update()
    {
        //lightArrow.transform.rotation = directionalLight.transform.rotation;
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
        directionalLight.transform.Rotate(Vector3.right, dir.x,Space.World); 
        directionalLight.transform.Rotate(Vector3.up, dir.y, Space.World);
        directionalLight.transform.Rotate(Vector3.forward, dir.z, Space.World); 
    }

}
