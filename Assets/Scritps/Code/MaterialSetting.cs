using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MaterialSetting 
{
    [SerializeField]
    Material material;

    public string name;
    public bool OnOpenOutline; 

    public Material GetMaterial()
    {
        return material;
    }

    public  MaterialSetting(Material mt,string name,bool openOutLine)
    {
        material =new Material(mt);
        this.name = name;
        material.name = name;
        OnOpenOutline = openOutLine;
    }
    
    float minWidth = 0.01f;
    float maxWidth =20f;
    public void SetOutlineWidth(float w)
    {
        w = Mathf.Clamp(w, minWidth, maxWidth);
        if (OnOpenOutline)
            material.SetFloat(Utils.ShaderOutlineWidth, w);
    }

    public void SetAlpha(float alp)
    {
        if (OnOpenOutline)
            material.SetFloat(Utils.ShaderAlpha, alp);
    }

    public void SetOutlineState(bool state)
    {
        if (OnOpenOutline)
            material.SetFloat(Utils.ShaderEnableOutline, state ? 1 : 0);
    }

    public bool SetTexture(string proprety,Texture2D tex)
    {
        if (HasProgrety(proprety))
        {
            if (proprety == Utils.ShaderNormalMap)
            {
                tex = ConvertToNormalMap(tex);
            }
            
            material.SetTexture(proprety, tex);
            return true;
        }
        
        return false;
    }

    bool HasProgrety(string p)
    {
        return p == Utils.ShaderBaseMap || p == Utils.ShaderNormalMap || p == Utils.ShaderRoughnessMap;
    }

    private Texture2D ConvertToNormalMap(Texture2D source)
    {
       Texture2D normalMap = new Texture2D(source.width, source.height, TextureFormat.RGB565, false);
        normalMap.SetPixels(source.GetPixels());
        normalMap.Apply(false, true);
         
        normalMap.wrapMode = TextureWrapMode.Repeat;
        normalMap.filterMode = FilterMode.Bilinear;
        normalMap.anisoLevel = 1;
        return normalMap; 
    }
}
