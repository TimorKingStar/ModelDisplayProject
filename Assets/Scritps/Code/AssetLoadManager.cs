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
 */


public class AssetLoadManager : MonoSingleton<AssetLoadManager>
{
    string url = @"C:\Users\Administrator\Desktop\WorkSpace\Cube\Cube.zip";

    private void Start()
    {
        LoadModel(url);
    }

    public void DownModeFromWeb(string webUrl)
    {
        HttpManager.Instance.DownLoadAssets(webUrl).OnComplate(c=>
        {
            Debug.Log(">>>>>>>>>> Download file surrce");
            WriteFile(c,webUrl);

        }).OnError(e=> { Debug.LogError(e); });
    }

    void LoadModel(string url)
    {
        string fileName = Application.persistentDataPath + @"/" + FileUtils.GetFilenameWithoutExtension(url)
                          +"/Fbx/"+ FileUtils.GetFilenameWithoutExtension(url) +".fbx";
        if (File.Exists(fileName))
        {
            var assetLoaderOptions = AssetLoader.CreateDefaultLoaderOptions();
            AssetLoader.LoadModelFromFile(fileName, OnLoad, OnMaterialLoad, OnProgress, 
                OnError, gameObject, assetLoaderOptions);
        }
        else
        {
            DownModeFromWeb(url);
        }
    }

    void WriteFile(byte[]data,string url)
    {
        if (data != null)
        {
            string pathDirectory = Application.persistentDataPath + @"/" + FileUtils.GetFilenameWithoutExtension(url);
            string filePath = pathDirectory + "/" + FileUtils.GetFilename(url);
            if (!Directory.Exists(pathDirectory))
            {
                Directory.CreateDirectory(pathDirectory);
            }                        

            File.WriteAllBytes(filePath, data);
            Debug.Log(">>>>>>>>>>>>>>>Write finished");
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
