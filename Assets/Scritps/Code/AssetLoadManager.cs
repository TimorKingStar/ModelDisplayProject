using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TriLibCore;
using System;
using TriLibCore.Utils;
using System.IO;
using Unity.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Zip;
using TriLibCore.Samples;
using TriLibCore.Mappers;

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
    string currentFilePath;
    string currentTextureDirectory;
    string currentTexturePath;

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

    public void CancleDownload()
    {
        Debug.Log(">>>>>>>> Cancle loaded model");
        HttpManager.Instance.CancleDownload();
        haltTask = true;
    }

    public void DownModeFromWeb(string webUrl)
    {
        UnLoadGameobject();
        haltTask = false;

        currentFilePath = Application.persistentDataPath + @"/" + FileUtils.GetFilenameWithoutExtension(webUrl);

        currentModelPath = currentFilePath + "/Fbx/" + FileUtils.GetFilenameWithoutExtension(webUrl)+".fbx";

        currentTextureDirectory = currentFilePath + "/Texture";

        Debug.Log(currentFilePath); 
        
        if (!File.Exists(currentFilePath))
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

    public bool haltTask;
    
    AssetLoaderOptions assetLoaderOptions;
    void LoadModelMode()
    {   
        if (assetLoaderOptions == null)
        {
            assetLoaderOptions = AssetLoader.CreateDefaultLoaderOptions(true);
            assetLoaderOptions.ImportCameras = true;
            assetLoaderOptions.ImportMaterials = false;
            assetLoaderOptions.MaterialMappers = new MaterialMapper[] { ScriptableObject.CreateInstance<StandardMaterialMapper>()};
        }

        AssetLoader.LoadModelFromStream(File.OpenRead(currentModelPath), FileUtils.GetShortFilename(currentModelPath), FileUtils.GetFileExtension(currentModelPath), OnLoad, OnMaterialLoad, OnProgress,
        OnError, gameObject, assetLoaderOptions, null, haltTask);

        Debug.Log(">>>>>>>>>:Load Model Finished");
    }

    
    void RenderModel()
    {
        if (File.Exists(currentModelPath) )
        {
            LoadTexture();
            LoadModelMode(); 
        }
        else
        {
            Debug.Log(">>>>>>>>>: ȱ��ģ���ļ�---");
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
            string pathDirectory = Application.persistentDataPath; 
            string filePath = pathDirectory + "/" + FileUtils.GetFilename(url);
            if (!Directory.Exists(pathDirectory))
            {
                Directory.CreateDirectory(pathDirectory);
            }                        

            File.WriteAllBytes(filePath, data);         
            Debug.Log(">>>>>>>>>>>>>>>Write finished :"+ filePath);

            if (unZip)
            {
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
            }

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
/// ��ͼ��Ӧ��������
/// </summary>
public class TexturePropertyName
{
   
}