using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialCreator : MonoBehaviour
{
    Shader shader;
    Material currentMat;

    public void InitMaterial(Texture texture = null)
    {
        shader = GameManager.Instance.GetShader();
        GetComponent<Renderer>().material = CreateMaterialFactor(texture);
    }

    Material CreateMaterialFactor(Texture texture = null)
    {
        shader = GameManager.Instance.GetShader(); ;
        currentMat = new Material(shader);
        if (texture != null)
        {
            currentMat.SetTexture("_MainTex", texture);
        }

        SetOutLineState(true);
        SetAlpha(0.5f);
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
        Debug.Log(alpha);
        currentMat.SetFloat("_AlphaScale", alpha);
    }
    
}
