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
                Debug.Log("û���ҵ�baseColor");
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
            Debug.Log(">>>>>>>>>: ȱ��ģ���ļ�---");
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


            #region ��ѹ��
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
