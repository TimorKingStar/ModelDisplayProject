using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TriLibCore;
using TriLibCore.Extensions;
using System;
using TriLibCore.Utils;
using System.IO;
using Unity.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Zip;
using TriLibCore.Samples;
using TriLibCore.Mappers;
using System.Threading;

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
    string baseModelPath;
    string currentModelPath;
    string currentFilePath;
    string currentTextureDirectory;
    string currentTexturePath;
    public GameObject currentModel;
    public List<UnityEngine.GameObject> allGameObjects=new List<GameObject>();

    public bool haltTask;

    AssetLoaderOptions assetLoaderOptions;
    AssetLoaderContext assetLoaderContext;

    private void Start()
    {
        baseModelPath = Application.persistentDataPath + "/Model";
        if (!Directory.Exists(baseModelPath))
        {
            Directory.CreateDirectory(baseModelPath);
        }
    }

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

    public void CancleDownload()
    {
        Debug.Log(">>>>>>>> Cancle loaded model");
        HttpManager.Instance.CancleDownload();
        assetLoaderContext.CancellationTokenSource.Cancel();
        UnLoadGameobject();
    }

    public void DownModeFromWeb(string webUrl)
    {
        UnLoadGameobject();
        haltTask = false;

        currentFilePath = baseModelPath +"/"+ FileUtils.GetFilenameWithoutExtension(webUrl);

        currentModelPath = currentFilePath + "/Fbx/" + FileUtils.GetFilenameWithoutExtension(webUrl)+".fbx";

        currentTextureDirectory = currentFilePath + "/Texture";

        Debug.Log(currentFilePath); 
        
        if (!File.Exists(currentModelPath))
        {
            HttpManager.Instance.DownLoadAssets(webUrl).OnComplate(c =>
            {
                Debug.Log(">>>>>>>>>> Download file surrce");
                WriteFile(c, webUrl,true);

            }).OnError(e => { Debug.LogError(e); }).
            OnProgress(p=> 
            {
                GetMessageFromIOS.DownloadModelProgress(FileUtils.GetFilenameWithoutExtension(webUrl), p.ToString());

            });
        }
        else
        {
            RenderModel();
        }
    }



    void LoadModelMode()
    {   
        if (assetLoaderOptions == null)
        {
            assetLoaderOptions = AssetLoader.CreateDefaultLoaderOptions(true);
            assetLoaderOptions.ImportCameras = true;
            assetLoaderOptions.ImportMaterials = false;
            
            assetLoaderOptions.MaterialMappers = new MaterialMapper[] { ScriptableObject.CreateInstance<StandardMaterialMapper>()};
        }

        assetLoaderContext = AssetLoader.LoadModelFromStream(File.OpenRead(currentModelPath), FileUtils.GetShortFilename(currentModelPath), FileUtils.GetFileExtension(currentModelPath), OnLoad, OnMaterialLoad, OnProgress,
        OnError, gameObject, assetLoaderOptions, null, haltTask);

        Debug.Log(">>>>>>>>>:Load Model Finished");

//FREDERIK:
        LoadAnimation();
    }

    void LoadAnimation()
    {
        GameObject animationContainer = assetLoaderContext.RootGameObject;
        if (animationContainer.TryGetComponent(out Animation a))
        {
            var l_animationClips = a.GetAllAnimationClips();
            if (l_animationClips.Count > 0)
            {
                GetComponent<AnimationManager>().StoreAnimation(l_animationClips[0]);
            }
        }
        else
            print("WARNING: there was no animation on the loaded fbx");
    }
    //END

    void RenderModel()
    {
        if (File.Exists(currentModelPath) )
        {
            //LoadTexture();        // we don't need to download textures
            LoadModelMode(); 
        }
        else
        {
            Debug.Log(">>>>>>>>>: 缺少模型文件---");
        }
    }

    Dictionary<string, Dictionary<string, Texture2D>> allModelTexture = new Dictionary<string, Dictionary<string, Texture2D>>();

    public List<Texture2D> totalTexture = new List<Texture2D>();

    void LoadTexture()
    {

        allModelTexture.Clear();
        if (!Directory.Exists(currentTextureDirectory))
            return;

        var files = Directory.GetFiles(currentTextureDirectory);
        foreach (var tex in files)
        {
            var texPath = tex.Replace(@"\", "/");
            var texName = FileUtils.GetFilenameWithoutExtension(texPath);
            string[] TexNames = texName.Split('_');
            byte[] data = File.ReadAllBytes(texPath);
            Texture2D texture = new Texture2D(1024, 1024);
            texture.name = FileUtils.GetFilenameWithoutExtension(texPath);
            if (texture.LoadImage(data)) 
            {
                if (TexNames.Length == 2)
                {
                    var modelName = TexNames[0]; 
                    var property = TexNames[1];
                    if (!allModelTexture.ContainsKey(modelName))
                    { 
                        var d = new Dictionary<string, Texture2D>();
                        allModelTexture.Add(modelName, d);
                    }
                    allModelTexture[modelName].Add(property, texture);
                    totalTexture.Add(texture);
                }
                else
                {
                    Debug.LogError(texture.name);
                }
            }
            else
            {
                Debug.LogError(texture.name);
            }
            
        }
    }

    void WriteFile(byte[]data,string url,bool unZip)
    {
        if (data != null) 
        {  
            string filePath = baseModelPath +"/" + FileUtils.GetFilename(url);
            if (!Directory.Exists(baseModelPath))
            {
                Debug.LogError(">>>>>>>>>>baseModelPath Faill:");
                Directory.CreateDirectory(baseModelPath);
            }                        

            File.WriteAllBytes(filePath, data);         
            Debug.Log(">>>>>>>>>>>>>>>Write finished :"+ filePath);

            if (unZip)
            {
                #region 解压缩
                FastZip zip = new FastZip();
               
                if (FileUtils.GetFileExtension(filePath) == ".zip")
                {
                    zip.ExtractZip(filePath, baseModelPath, "");

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
            }

            RenderModel();
        }
        else
        {
            Debug.LogError(">>>>>>>>>>>>>>>Write file faill");
        }
    }


    private void OnLoad(AssetLoaderContext  loaderContext)
    {
        loaderContext.RootGameObject.SetActive(false);
    }
    
    private void OnMaterialLoad(AssetLoaderContext loaderContext)
    {
        return;
        //We don't need to download materials
        currentModel = loaderContext.RootGameObject;
        foreach (var g in loaderContext.GameObjects)
        {
            allGameObjects.Add(g.Value);
        }

        var fileReference= currentModel.AddComponent<FileReferenceBinding>();
        fileReference.Init(loaderContext,allModelTexture);

        currentModel.SetActive(true);
    } 

    private void OnProgress(AssetLoaderContext loaderContext, float progress)
    {
        Debug.Log(progress);
        GetMessageFromIOS.LoadModelProgress(FileUtils.GetFilenameWithoutExtension(loaderContext.Filename), progress.ToString()); 
    }
    
    private void OnError(IContextualizedError obj)
    {
        Debug.Log(obj);
    }
    
}

/// <summary>
/// 贴图对应的属性名
/// </summary>
public class TexturePropertyName
{
   
}