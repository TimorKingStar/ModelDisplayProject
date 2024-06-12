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
    string url ;
    string currentFilePath;
    private void Start()
    {   
        url = @"file://"+Application.streamingAssetsPath + "/Cube.fbx";
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
                         +"/"+ FileUtils.GetFilename(url);

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
            string pathDirectory = Application.persistentDataPath+ "/"+ FileUtils.GetFilenameWithoutExtension(url); 
            string filePath = pathDirectory + "/" + FileUtils.GetFilename(url);
            if (!Directory.Exists(pathDirectory))
            {
                Directory.CreateDirectory(pathDirectory);
            }                        

            File.WriteAllBytes(filePath, data);
            Debug.Log(">>>>>>>>>>>>>>>Write finished :"+ filePath);

            #region 解压缩
            //FastZip zip = new FastZip();  
            //Debug.Log(pathDirectory);    
            //if (FileUtils.GetFileExtension(filePath)==".zip")
            //{
            //    zip.ExtractZip(filePath, pathDirectory, "");
            //}
            //else
            //{
            //    Debug.LogError(">>>>>>>>>>> FileExtension dont is zip");
            //}

            //if (File.Exists(filePath))
            //{
            //    File.Delete(filePath);
            //}
            #endregion

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
        var AllAnimation= loaderContext.RootModel.AllAnimations;
        var currentMode=  loaderContext.RootGameObject;

        var fileReference= currentMode.AddComponent<FileReferenceBinding>();
        fileReference.Init(loaderContext);
        currentMode.transform.position = new Vector3(0, 1, 0);
        currentMode.transform.rotation = Quaternion.Euler(45, 45, 45);
        currentMode.SetActive(true);
    }

    private void OnProgress(AssetLoaderContext loaderContext, float progress)
    {
        Debug.Log(progress);
    }
    
    private void OnError(IContextualizedError obj)
    {
        
    }

    
}
