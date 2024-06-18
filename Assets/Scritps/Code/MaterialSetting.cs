using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MaterialSetting 
{
    [SerializeField]
    Material material;

    public string name;
    public string baseTex=Utils.ShaderBaseMap;
    public string normalTex= Utils.ShaderNormalMap;
    public string roughness= Utils.ShaderRoughnessMap;

    public bool OnOpenOutline; 
    public string outline= Utils.ShaderEnableOutline;
    public string alpha= Utils.ShaderAlpha;


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

    public void SetAlpha(float alp)
    {
        if (OnOpenOutline)
            material.SetFloat(alpha, alp);
    }

    public void SetOutlineState(bool state)
    {
        if (OnOpenOutline)
            material.SetFloat(outline, state ? 1 : 0);
    }

    public bool SetTexture(string proprety,Texture tex)
    {
        if (HasProgrety(proprety))
        {
            material.SetTexture(proprety, tex);
            return true;
        }

        return false;
    }

    bool HasProgrety(string p)
    {
        return p == baseTex || p == normalTex || p == roughness;
    }

}
