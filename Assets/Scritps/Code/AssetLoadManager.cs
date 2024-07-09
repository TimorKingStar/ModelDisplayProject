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
using UnityEngine.SceneManagement;
using System.ComponentModel;


public class AssetLoadManager : MonoSingleton<AssetLoadManager>
{
    string baseModelPath;
    string currentModelPath;
    string currentFilePath;
    string currentTextureDirectory;
    string currentTexturePath;
    public GameObject currentModel;

    AssetLoaderOptions assetLoaderOptions;
    AssetLoaderContext assetLoaderContext;

    private void Start()
    {
        baseModelPath =Path.Combine(Application.persistentDataPath,"Model") ;
        if (!Directory.Exists(baseModelPath))
        {
            Directory.CreateDirectory(baseModelPath);
        } 

        Debug.Log("baseModelPath: "+baseModelPath);
    }

    private void OnEnable()
    {
        InputManage.Instance.CancleLoadedModelEvent.AddListener(CancleDownload);
    }

     
    void UnLoadGameobject()
    {
        if (currentModel != null)
        { 
            
            Destroy(currentModel);
           
        }

         for(int i=0;i<totalTexture.Count;i++)
         {
            Destroy(totalTexture[i]);
         }
        totalTexture.Clear();
               
 try{
        foreach( var texDict in allModelTexture)   
        {
             foreach(var tex in texDict.Value)
             {
                 if(tex.Value!=null)
                 {
                    Destroy(tex.Value);
                 }
            }
        }

        }
    catch(Exception e)
  {
    Debug.Log(e);
}
        allModelTexture.Clear();
        System.GC.Collect();
        Resources.UnloadUnusedAssets();
    }

    public void CancleDownload()
    {
        Debug.Log(">>>>>>>> Cancle loaded model");
        HttpManager.Instance.CancleDownload();
        assetLoaderContext.CancellationTokenSource.Cancel();

        UnLoadGameobject();
    }
    
    
    public void LoadAnimation(string animPath)
    {
        if (File.Exists(animPath))
        {

               assetLoaderOptions = AssetLoader.CreateDefaultLoaderOptions(true);
                assetLoaderOptions.ImportCameras = true;
                //assetLoaderOptions.ImportNormals=true;
            assetLoaderContext = AssetLoader.LoadModelFromStream(File.OpenRead(animPath), FileUtils.GetShortFilename(animPath), FileUtils.GetFileExtension(animPath), LoadAnimation, null, OnProgress,
            OnError, gameObject, assetLoaderOptions, null, false);
        }
    }

   
    public void DownModeFromWeb(string webUrl)
    {
        UnLoadGameobject();
       
        currentFilePath = Path.Combine(baseModelPath , FileUtils.GetFilenameWithoutExtension(webUrl));
        currentModelPath =Path.Combine(currentFilePath,"Fbx",FileUtils.GetFilenameWithoutExtension(webUrl)+".fbx");
        currentTextureDirectory = Path.Combine(currentFilePath,"Texture");
        
        Debug.Log(currentModelPath); 
        
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
            
        }

        assetLoaderContext = AssetLoader.LoadModelFromStream(File.OpenRead(currentModelPath), FileUtils.GetShortFilename(currentModelPath), FileUtils.GetFileExtension(currentModelPath), OnLoad, OnMaterialLoad, OnProgress,
        OnError, gameObject, assetLoaderOptions, null, false);

        Debug.Log(">>>>>>>>>:Load Model Finished");

    }

    void LoadAnimation(AssetLoaderContext loaderContext)
    {
        GameObject animationContainer = loaderContext.RootGameObject;
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
            LoadTexture();        // we need to download textures
            LoadModelMode(); 
        }
        else
        {
            Debug.Log(">>>>>>>>>:Can't find current model "+currentModelPath);
        }
    }

    Dictionary<string, Dictionary<string, Texture2D>> allModelTexture = new Dictionary<string, Dictionary<string, Texture2D>>();

    public List<Texture2D> totalTexture = new List<Texture2D>();

    void LoadTexture()
    {
         for(int i=0;i<totalTexture.Count;i++)
         {
            Destroy(totalTexture[i]);
         }
        totalTexture.Clear();
               
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
        loaderContext.RootGameObject.transform.localPosition=Vector3.zero;
        loaderContext.RootGameObject.SetActive(false);
    }
    
    private void OnMaterialLoad(AssetLoaderContext loaderContext)
    {
        //We  need to set materials
        currentModel = loaderContext.RootGameObject;
      
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
