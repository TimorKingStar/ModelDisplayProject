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
 */


public class AssetLoadManager : MonoSingleton<AssetLoadManager>
{
    string url ;
    string currentFilePath;
    private void Start()
    {   
        url = "file://"+ Application.dataPath + "/Art/Cube.zip";
        Debug.Log(url);
        DownModeFromWeb(url); 
    }

    void RenderModel()
    {
        if (File.Exists(currentFilePath))
        {
            var assetLoaderOptions = AssetLoader.CreateDefaultLoaderOptions();
            AssetLoader.LoadModelFromFile(currentFilePath, OnLoad, OnMaterialLoad, OnProgress,
                OnError, gameObject, assetLoaderOptions);

            Debug.Log(">>>>>>>>>:Load Model Finished");
        }
        else
        {
            Debug.Log(">>>>>>>>>: 缺少模型文件---");
        }
    }

    void DownModeFromWeb(string webUrl)
    {
        currentFilePath = Application.persistentDataPath + @"/" + FileUtils.GetFilenameWithoutExtension(url)
                          +"/Fbx/"+ FileUtils.GetFilenameWithoutExtension(url) +".fbx";

        if (!File.Exists(currentFilePath))
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
            FastZip zip = new FastZip();
            Debug.Log(pathDirectory);    
            if (FileUtils.GetFileExtension(filePath)==".zip")
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
        var go=  loaderContext.RootGameObject;
        go.SetActive(true);
    }

    private void OnProgress(AssetLoaderContext loaderContext, float progress)
    {
        Debug.Log(progress);
    }
    
    private void OnError(IContextualizedError obj)
    {
        
    }

    
}
