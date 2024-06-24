using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialCreator : MonoBehaviour
{
    public Dictionary<string, string> materialPropers = new Dictionary<string, string>();
    private void AddProperKeyValue()
    {
        materialPropers.Add(Utils.TextureBaseMap, Utils.ShaderBaseMap);
        materialPropers.Add(Utils.TextureNormalMap, Utils.ShaderNormalMap);
        materialPropers.Add(Utils.TextureRoughnessMap, Utils.ShaderRoughnessMap);
    }

    public List<MaterialSetting> InitMaterial(Dictionary<string, Dictionary<string, Texture2D>> materialDice)
    {
        AddProperKeyValue();

        List<MaterialSetting> totalMaterials = new List<MaterialSetting>();

        foreach (var mat in materialDice)
        {  
            MaterialSetting materialSetting = null;
            if (mat.Key == Utils.Ground)
            {
                materialSetting = new MaterialSetting(InputManage.Instance.alphaMaterial, mat.Key, false);
            }
            else
            {
                materialSetting = new MaterialSetting(InputManage.Instance.alphaMaterial, mat.Key, true);
            }

            foreach (var proprety in mat.Value)
            {
                if (materialPropers.ContainsKey(proprety.Key))
                    materialSetting.SetTexture(materialPropers[proprety.Key], proprety.Value);
            }
            if (materialSetting != null)
            {
                totalMaterials.Add(materialSetting);
            }
        }
        return totalMaterials;
    }
    
}
