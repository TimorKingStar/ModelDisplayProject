using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TriLibCore;
using System;
using TriLibCore.Utils;
using System.IO;
using Unity.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Zip;

/*
 1. 将模型以及贴图都放到压缩包里面，先将服务端的文件下载到本地。
 3. 将模型解压出来放置在持久化路径文件下
 2. 通过Trilib加载本地文件的方式将模型加载出来
 3. 设置模型的贴图
 4. 如果有相机的话，配置初始相机的位置。
 5. 如果有动画的话，将动画也放到场景中来。

    1.点击哪个物体，哪个物体就进行旋转？？？

    1. 速写默写，自带动画，放大缩小模型，旋转视窗，光源旋转，需要outLine效果。
    2. 单组静物，自带动画，多种材质，视窗旋转放大缩小，光源旋转，需要outLine效果，需要透明效果。
    3. 石膏结构，任意旋转，放大缩小观察，光源旋转，需要outLine效果。
    4. 头部结构，任意旋转，放大缩小观察，光源旋转，分皮肤，肌肉，骨骼层级进行观察，需要outLine效果。

 */


/*
  模型里面带相机的话命名为：Camera
  模型需要贴图，里面带一张 mainTexture
  模型皮肤，肌肉，骨骼层级进行观察，每层命名为 Depth，或者 Depth_model，看以上两种哪个方便
  模型可以带动画
     */

public class AssetLoadManager : MonoSingleton<AssetLoadManager>
{

    string currentModelPath;
    string currentTexturePath;
    
    const string baseColor = "_BaseColor.png";
    const string roughNess = "_Roughness.png";
    const string normalColor = "_NormalTexture.png";

    public Texture2D _baseColor;
    public Texture2D _roughNess;
    public Texture2D _normalColor;

    public List<UnityEngine.GameObject> allGameObjects=new List<GameObject>();
    void UnLoadGameobject()
    {
        if (allGameObjects.Count > 0)
        {
            foreach (var go in allGameObjects)
            {
                Destroy(go);
            }

            allGameObjects.Clear();
        }
    }

    public  void DownModeFromWeb(string webUrl)
    {
        UnLoadGameobject();
        Debug.Log(webUrl);
        currentModelPath = Application.persistentDataPath + @"/" + FileUtils.GetFilenameWithoutExtension(webUrl)
                         +"/Fbx/"+ FileUtils.GetFilenameWithoutExtension(webUrl) +".fbx";

        Debug.Log(currentModelPath);
        currentTexturePath = Application.persistentDataPath + @"/" + FileUtils.GetFilenameWithoutExtension(webUrl) +
                           "/Texture/";
         
        if (!File.Exists(currentModelPath))
        {
            HttpManager.Instance.DownLoadAssets(webUrl).OnComplate(c =>
            {
                Debug.Log(">>>>>>>>>> Download file surrce");
                WriteFile(c, webUrl);

            }).OnError(e => { Debug.LogError(e); });
        }
        else
        {
            RenderModel();
        }
    }

    void RenderModel()
    {
        if (File.Exists(currentModelPath) )
        {
            if (File.Exists(currentTexturePath + baseColor))
            {
                var b= File.ReadAllBytes(currentTexturePath + baseColor);
                _baseColor = new Texture2D(1024,1024);
                _baseColor.LoadImage(b);
            }
            else
            {
                Debug.Log("没有找到baseColor");
                _baseColor = null;
            }

            if (File.Exists(currentTexturePath + normalColor))
            {
                var b = File.ReadAllBytes(currentTexturePath + normalColor);
                _normalColor = new Texture2D(1024, 1024);
                _normalColor.LoadImage(b);
            }
            else
            {
                _normalColor = null;
            }

            if (File.Exists(currentTexturePath + roughNess))
            {
                var b = File.ReadAllBytes(currentTexturePath + roughNess);
                _roughNess = new Texture2D(1024, 1024);
                _roughNess.LoadImage(b);
            }
            else
            {
                _roughNess = null;
            }

            var assetLoaderOptions = AssetLoader.CreateDefaultLoaderOptions(true, false);
            assetLoaderOptions.ImportCameras = true;
            AssetLoader.LoadModelFromFile(currentModelPath, OnLoad, OnMaterialLoad, OnProgress,
            OnError, gameObject, assetLoaderOptions);

            Debug.Log(">>>>>>>>>:Load Model Finished");
        }
        else
        {
            Debug.Log(">>>>>>>>>: 缺少模型文件---");
        }
    }

    void WriteFile(byte[]data,string url)
    {
        if (data != null) 
        {  
            string pathDirectory = Application.persistentDataPath; 
            string filePath = pathDirectory + "/" + FileUtils.GetFilename(url);
            if (!Directory.Exists(pathDirectory))
            {
                Directory.CreateDirectory(pathDirectory);
            }                        

            File.WriteAllBytes(filePath, data);         
            Debug.Log(">>>>>>>>>>>>>>>Write finished :"+ filePath);


            #region 解压缩
            FastZip zip = new FastZip();
            Debug.Log(pathDirectory);
            if (FileUtils.GetFileExtension(filePath) == ".zip")
            {
                zip.ExtractZip(filePath, pathDirectory, "");
            }
            else
            {
                Debug.LogError(">>>>>>>>>>> FileExtension dont is zip");
            }

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            #endregion

            RenderModel();
        }
        else
        {
            Debug.LogError(">>>>>>>>>>>>>>>Write file faill");
        }
    }

    public GameObject currentModel;
    private void OnLoad(AssetLoaderContext  loaderContext)
    {
        loaderContext.RootGameObject.SetActive(false);
    }
    
    private void OnMaterialLoad(AssetLoaderContext loaderContext)
    {
        
        currentModel = loaderContext.RootGameObject;
        foreach (var g in loaderContext.GameObjects)
        {
            allGameObjects.Add(g.Value);
        }

        var AllAnimation= loaderContext.RootModel.AllAnimations;
       var currentMode=  loaderContext.RootGameObject;


        var fileReference= currentMode.AddComponent<FileReferenceBinding>();
        fileReference.Init(loaderContext,_baseColor,_normalColor,_roughNess);
        //currentMode.transform.position = Vector3.zero;
        currentMode.SetActive(true);
    } 

    private void OnProgress(AssetLoaderContext loaderContext, float progress)
    {
        GetMessageFromIOS.LoadModelProgress(FileUtils.GetFilenameWithoutExtension(loaderContext.Filename), progress.ToString()); 
    }
    
    private void OnError(IContextualizedError obj)
    {
        Debug.Log(obj);
    }

    
}
