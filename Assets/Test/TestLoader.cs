using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TriLibCore;
using TriLibCore.General;
using System;
using TriLibCore.Extensions;

public class TestLoader : MonoBehaviour
{
    public Material mainTexture;

    public string filePath;
    void OnGUI()
    {
        LoadModelFromFilePicker();
        LoadModelFromFilePath();
        LoadAnimation();
        ResoucrcesLoad();
        LoadMainTexture();

        /*
        1. 所有的模型都需要outline效果嘛?
        2. 所有的模型都需要透明效果吗？
        3. 需要有按钮控制 outline/透明 效果吗？
        4. 在头部结构中提到的三种视窗模式是指不同的窗口吗？比如整个屏幕展示模型，or 上半层展示图片，下半层展示模型？


        1. 测试加载模型之后给模型贴贴图，看能否配置相关的贴图。  可以配置相关的shader，可以配置相关的material
        2. 测试加载模型的时候更换shader。
         */
        

    }

    void ResoucrcesLoad()
    {
        if (GUILayout.Button("Resources load model"))
        {
           var go= Resources.Load("hello");
        }
    }

 
    void LoadModelFromFilePicker()
    {
        if (GUILayout.Button("Load model from filePicker"))
        {
            var assetLoaderOption = AssetLoader.CreateDefaultLoaderOptions(true, true);
            assetLoaderOption.UseUnityNativeTextureLoader = true;
            var assetLoaderFilePicker = AssetLoaderFilePicker.Create();

            assetLoaderFilePicker.LoadModelFromFilePickerAsync("Select a File", OnLoad, OnMaterialsLoad, OnProgress, OnBeginLoad, OnError, null, assetLoaderOption);
        }
    }

    void LoadModelFromFilePath()
    {
        if (GUILayout.Button("Load model from filePath"))
        {
            var assetLoaderOption = AssetLoader.CreateDefaultLoaderOptions();
            AssetLoader.LoadModelFromFile(filePath, OnLoad, OnMaterialsLoad, OnProgress, OnError, null, assetLoaderOption);
        }
    }


    private void OnError(IContextualizedError obj)
    {
         
    }

    private void OnBeginLoad(bool obj)
    {
         
    }

    private void OnProgress(AssetLoaderContext arg1, float arg2)
    {
        Debug.Log("OnProgress: " + arg2);
    }

    Animation _animation;
    List<AnimationClip> animationClips;
    GameObject currentGameobject;
    private void OnMaterialsLoad(AssetLoaderContext assetLoaderContext)
    {
        var obj = assetLoaderContext.RootGameObject;
        currentGameobject = obj;
     
        obj.SetActive(true);
    }

    public List<Texture> allTexture;
    public string[] allProperty;
    void LoadMainTexture()
    {
        if (GUILayout.Button("切换模型贴图"))
        { 
            var render = currentGameobject.GetComponentInChildren<Renderer>();
           var shder = Shader.Find("AutodeskInteractive");
            allProperty= mainTexture.GetTexturePropertyNames();

             
            mainTexture.SetTexture("_MainTex", allTexture[0]);
            mainTexture.SetTexture("_MetallicGlossMap", allTexture[1]);
            mainTexture.SetTexture("_SpecGlossMap", allTexture[2]);
            mainTexture.SetTexture("_BumpMap", allTexture[3]);
            mainTexture.SetTexture("_ParallaxMap", allTexture[4]);
            mainTexture.SetTexture("_OcclusionMap", allTexture[5]);
            render.material=mainTexture; 
        }
    } 

    string currentAnimName;
    void LoadAnimation()
    {
        if (animationClips != null)
        {
            foreach (var anim in animationClips)
            {
                if (GUILayout.Button(anim.name))
                {
                    currentAnimName = anim.name;
                    PlayCurrentAnimation();
                }
            }
        }
    }

    void PlayCurrentAnimation()
    {
        if (_animation.GetClip(currentAnimName)!=null )
        {
            _animation.clip = _animation.GetClip(currentAnimName);
            _animation.Play(PlayMode.StopAll);
        }
    }

    private void OnLoad(AssetLoaderContext  assetLoaderContext)
    {
       
        Debug.Log("IsZipFile: "+ assetLoaderContext.IsZipFile);
        Debug.Log("LoadedTextures.Count: " + assetLoaderContext.LoadedTextures.Count);
        Debug.Log("LoadedMaterials.Count" + assetLoaderContext.LoadedMaterials.Count);

        var obj = assetLoaderContext.RootGameObject;
        obj.SetActive(false);
    }
}
