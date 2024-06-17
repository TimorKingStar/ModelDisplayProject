using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialCreator : MonoBehaviour
{
    public Dictionary<string, string> materialPropers = new Dictionary<string, string>();
    private void AddProperKeyValue()
    {
        materialPropers.Add("BaseColor", "_BaseMap");
        materialPropers.Add("Normal", "_BumpMap");
        materialPropers.Add("Roughness", "_RoughnessMap");
    }

    public string ground = "Ground";

    public List<MaterialSetting> InitMaterial(Dictionary<string, Dictionary<string, Texture2D>> materialDice)
    {
        AddProperKeyValue();

        List<MaterialSetting> totalMaterials = new List<MaterialSetting>();
        
        foreach (var mat in materialDice)
        {
            MaterialSetting materialSetting=null;
            if (mat.Key == ground)
            {
                 materialSetting = new MaterialSetting(GameManager.Instance.alphaMaterial, mat.Key,false);
            }
            else
            {
                 materialSetting = new MaterialSetting(GameManager.Instance.alphaMaterial, mat.Key,true);
            }

            foreach (var proprety in mat.Value)
            {
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
