using System;
using System.Collections;
using System.Collections.Generic;
using TriLibCore;
using UnityEngine;
using Valve.Newtonsoft.Json;

public class FileLayerBinding : MonoBehaviour
{
    
    public GameObject _muscleGo;
    public GameObject _skinGo;
    public GameObject _skullGo;
    public GameObject _fatGo;
    public GameObject _insideGo;

    public List<GameObject> allGoList = new List<GameObject>();

    public void InitLayer(AssetLoaderContext loaderContext)
    {
        var rootGoDict = loaderContext.GameObjects;
        foreach (var go in rootGoDict)
        {
            if (go.Key.Name== Utils.Muscle)
            {
                _muscleGo = go.Value; 
            }
            else if(go.Key.Name == Utils.Skin)
            {
                _skinGo = go.Value;
            }
            else if (go.Key.Name == Utils.Skull)
            {
                _skullGo = go.Value;
            }else if(go.Key.Name == Utils.Fat)
            {
                _fatGo =go.Value;

            }else if(go.Key.Name == Utils.Inside)
            {
               _insideGo=go.Value;
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

        if (_fatGo != null)
        {
            modelInfo.fatGo = new ModelActiveInfo(_fatGo.name, _fatGo.activeSelf);
            foreach (var r in _fatGo.GetComponentsInChildren<Renderer>())
            {
                modelInfo.fatGoList.Add(new ModelActiveInfo(r.gameObject.name, r.gameObject.activeSelf));
            }
        }
        
        if (_insideGo != null)
        {   
            modelInfo.insideGo = new ModelActiveInfo(_insideGo.name, _insideGo.activeSelf);
            foreach (var r in _insideGo.GetComponentsInChildren<Renderer>())
            {
                modelInfo.insideGoList.Add(new ModelActiveInfo(r.gameObject.name, r.gameObject.activeSelf));
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
        fatGoList=new List<ModelActiveInfo>();
        insideGoList=new List<ModelActiveInfo>();
    }

    public ModelActiveInfo muscleGo;
    public ModelActiveInfo skinGo;
    public ModelActiveInfo skullGo;
     public ModelActiveInfo fatGo;
    public ModelActiveInfo insideGo;
    public List<ModelActiveInfo> muscleGoList;
    public List<ModelActiveInfo> skinGoList;
    public List<ModelActiveInfo> skullGoList;
    public List<ModelActiveInfo> fatGoList;
    public List<ModelActiveInfo> insideGoList;
    
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