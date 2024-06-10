using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialCreator : MonoBehaviour
{
    public Shader shader;
    Material currentMat;
    public GameObject cube;
    public Texture texture;
    private void Start()
    {
        cube.GetComponent<Renderer>().material = CreateMaterialFactor(texture);
    }

    public Material CreateMaterialFactor(Texture texture)
    {
        currentMat = new Material(shader);
        currentMat.SetTexture("_MainTex", texture);
        SetOutLineState(true);
        SetAlpha(0.5f);
        return currentMat;
    }

    public void SetOutLineState(bool state, float width = 0.2f)
    {
        Debug.Log(state);
        currentMat.SetInt("_EnableOutLine", state ? 1 : 0);
        currentMat.SetFloat("_OutLineWidth", width);
        currentMat.SetColor("_OutLineColor", Color.black);
    }

    public void SetAlpha(float alpha)
    {
        Debug.Log(alpha);
        currentMat.SetFloat("_AlphaScale", alpha);
    }

    public float alpha;
    public bool state;
    private void OnGUI()
    {
        if (GUILayout.Button("Alpha"))
        {
            SetAlpha(alpha);
        }

        if (GUILayout.Button("OutLine"))
        {
            SetOutLineState(state);
        }
    }

}
