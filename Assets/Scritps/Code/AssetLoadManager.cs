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
 1. ��ģ���Լ���ͼ���ŵ�ѹ�������棬�Ƚ�����˵��ļ����ص����ء�
 3. ��ģ�ͽ�ѹ���������ڳ־û�·���ļ���
 2. ͨ��Trilib���ر����ļ��ķ�ʽ��ģ�ͼ��س���
 3. ����ģ�͵���ͼ
 4. ���������Ļ������ó�ʼ�����λ�á�
 5. ����ж����Ļ���������Ҳ�ŵ�����������

    1.����ĸ����壬�ĸ�����ͽ�����ת������


    1. ��дĬд���Դ��������Ŵ���Сģ�ͣ���ת�Ӵ�����Դ��ת����ҪoutLineЧ����
    2. ���龲��Դ����������ֲ��ʣ��Ӵ���ת�Ŵ���С����Դ��ת����ҪoutLineЧ������Ҫ͸��Ч����
    3. ʯ��ṹ��������ת���Ŵ���С�۲죬��Դ��ת����ҪoutLineЧ����
    4. ͷ���ṹ��������ת���Ŵ���С�۲죬��Դ��ת����Ƥ�������⣬�����㼶���й۲죬��ҪoutLineЧ����

 */


/*
  ģ�����������Ļ�����Ϊ��Camera
  ģ����Ҫ��ͼ�������һ�� mainTexture
  ģ��Ƥ�������⣬�����㼶���й۲죬ÿ������Ϊ Depth������ Depth_model�������������ĸ�����
  ģ�Ϳ��Դ�����
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
            Debug.Log(">>>>>>>>>: ȱ��ģ���ļ�---");
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

            #region ��ѹ��
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
