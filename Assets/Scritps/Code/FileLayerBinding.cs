using System;
using System.Collections;
using System.Collections.Generic;
using TriLibCore;
using UnityEngine;
using Valve.Newtonsoft.Json;

public class FileLayerBinding : MonoBehaviour
{

    const string muscle = "Muscles";
    const string skin = "Skin";
    const string skull = "Skull";

    public GameObject _muscleGo;
    public GameObject _skinGo;
    public GameObject _skullGo;

    public List<GameObject> allGoList = new List<GameObject>();

    public void InitLayer(AssetLoaderContext loaderContext)
    {
        var rootGoDict = loaderContext.GameObjects;
        foreach (var go in rootGoDict)
        {
            if (go.Key.Name== muscle)
            {
                _muscleGo = go.Value; 
            }
            else if(go.Key.Name == skin)
            {
                _skinGo = go.Value;
            }
            else if (go.Key.Name == skull)
            {
                _skullGo = go.Value;
            }

            allGoList.Add(go.Value);
        }

        GetHeadModelInfo();
        ReturnIosModelInfo();
    }

    public void ResetHeadActive()
    {
        foreach (var go in allGoList)
        {
            go.SetActive(true);
        }
    }

    [SerializeField]
    HeadModelInfo modelInfo = new HeadModelInfo();

    void GetHeadModelInfo()
    {
        if (_muscleGo != null)
        {
            modelInfo.muscleGo = new ModelActiveInfo(_muscleGo.name, _muscleGo.activeSelf);
            foreach (var r in _muscleGo.GetComponentsInChildren<Renderer>())
            {
                modelInfo.muscleGoList.Add(new ModelActiveInfo(r.gameObject.name, r.gameObject.activeSelf));
            }
        }
        if (_skinGo != null)
        {
            modelInfo.skinGo = new ModelActiveInfo(_skinGo.name, _skinGo.activeSelf);
            foreach (var r in _skinGo.GetComponentsInChildren<Renderer>())
            {
                modelInfo.skinGoList.Add(new ModelActiveInfo(r.gameObject.name, r.gameObject.activeSelf));
            }
        }
        if (_skullGo != null)
        {
            modelInfo.skullGo = new ModelActiveInfo(_skullGo.name, _skullGo.activeSelf);
            foreach (var r in _skullGo.GetComponentsInChildren<Renderer>())
            {
                modelInfo.skullGoList.Add(new ModelActiveInfo(r.gameObject.name, r.gameObject.activeSelf));
            }
        }
    }

    void SetHeadInit()
    {
        foreach (var g in allGoList)
        {
            if (g.name == name)
            {
                g.SetActive(true);
            }
        }
    }


    public void SetHeadActive(string name, bool state)
    {
        foreach (var g in allGoList)
        {
            if (g.name == name)
            {
                g.SetActive(state);
            }
        }
    }

    void ReturnIosModelInfo()
    {
        string info= JsonConvert.SerializeObject (modelInfo);
        Debug.Log("Model Info : "+info);
        GetMessageFromIOS.ReturnHeadModelInfo(info);
    }


}


[System.Serializable]
public class HeadModelInfo
{
    public HeadModelInfo()
    {
        muscleGoList = new List<ModelActiveInfo>();
        skinGoList = new List<ModelActiveInfo>();
        skullGoList = new List<ModelActiveInfo>();
    }

    public ModelActiveInfo muscleGo;
    public ModelActiveInfo skinGo;
    public ModelActiveInfo skullGo;

    public List<ModelActiveInfo> muscleGoList;
    public List<ModelActiveInfo> skinGoList;
    public List<ModelActiveInfo> skullGoList;
    
}

[System.Serializable]
public class ModelActiveInfo
{
    public ModelActiveInfo(string name,bool state)
    {
        modelName = name;
        ModelActive = state;
    }

    public string modelName;
    public bool ModelActive;
}