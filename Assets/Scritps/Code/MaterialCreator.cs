using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialCreator : MonoBehaviour
{
    Shader shader;
    [SerializeField]
    Material currentMat;

    public void InitMaterial(Texture baseColor,Texture normalColor,Texture roughNess )
    {
        currentMat = null; 
        currentMat = new Material( GameManager.Instance.GetMaterial());
        currentMat.name = gameObject.name;
        GetComponent<Renderer>().material = CreateMaterialFactor(baseColor, normalColor, roughNess);
    }
     
    Material CreateMaterialFactor(Texture baseColor, Texture normalColor, Texture roughNess)
    {
   
        if (baseColor != null)
            currentMat.SetTexture("_MainTex", baseColor);
        if (normalColor != null)
            currentMat.SetTexture("_BumpMap", normalColor);
        if (roughNess != null)
            currentMat.SetTexture("_SpecGlossMap", roughNess);
        return currentMat;
    }

    public void SetOutLineState(bool state, float width = 0.2f)
    {
        currentMat.SetInt("_EnableOutLine", state ? 1 : 0);
        currentMat.SetFloat("_OutLineWidth", width);
        currentMat.SetColor("_OutLineColor", Color.black);
    }

    public void SetAlpha(float alpha)
    {
        currentMat.SetFloat("_AlphaScale", alpha);
    }
    
}
