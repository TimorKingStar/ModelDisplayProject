using System;
using System.Collections.Generic;
using System.IO;
using TriLibCore;
using TriLibCore.General;
using TriLibCore.Interfaces;
using UnityEngine;
using UnityEngine.Events;


/*
    1. 速写默写，自带动画，放大缩小模型，旋转视窗，光源旋转，需要outLine效果。
    2. 单组静物，自带动画，多种材质，视窗旋转放大缩小，光源旋转，需要outLine效果，需要透明效果。
    3. 石膏结构，任意旋转，放大缩小观察，光源旋转，需要outLine效果。
    4. 头部结构，任意旋转，放大缩小观察，光源旋转，分皮肤，肌肉，骨骼层级进行观察，需要outLine效果。

*/


public class FileReferenceBinding : MonoBehaviour
{


    FileLayerBinding  fileLayerBinding;

    public void SetFileReferenceBinding(FileLayerBinding binding)
    {
        fileLayerBinding = binding;
        GameManager.Instance.inputManage.SetHeadLayerShowEvent.AddListener(fileLayerBinding.SetHeadActive );
        GameManager.Instance.inputManage.ResetHeadLayerShowEvent.AddListener(fileLayerBinding.ResetHeadActive);
    }

    [SerializeField]
    public Transform _cameraTrans;

    [SerializeField]
    public GameObject _rootModel;

    private void OnEnable()
    {
        GameManager.Instance.inputManage.OutLineStateEvent.AddListener(SetOutLineState);
        GameManager.Instance.inputManage.AlphaStateEvent.AddListener(SetAlphaState);
    }

    private void OnDisable()
    {
        try
        {
            GameManager.Instance.inputManage.OutLineStateEvent.RemoveListener(SetOutLineState);
            GameManager.Instance.inputManage.AlphaStateEvent.RemoveListener(SetAlphaState);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    private void SetAlphaState(float arg0)
    {
        foreach (var mat in listMaterials)
        {
            mat.SetAlpha(arg0);
        }
    }

    private void SetOutLineState(float arg0)
    {
        bool state = arg0 == 1;
        foreach (var mat in listMaterials)
        {
            mat.SetOutlineState(state);
        }
    }

    public List<MaterialSetting> listMaterials = new List<MaterialSetting>();


    public void Init(AssetLoaderContext loaderContext, Dictionary<string, Dictionary<string, Texture2D>> allModelTexture)
    {
        _rootModel = loaderContext.RootGameObject;

        var layerBind = _rootModel.AddComponent<FileLayerBinding>();
        layerBind.InitLayer(loaderContext);
        SetFileReferenceBinding(layerBind); 

        var mat = _rootModel.AddComponent<MaterialCreator>();
        listMaterials = mat.InitMaterial(allModelTexture);

        foreach (var m in loaderContext.GameObjects)
        {
            if (m.Key.Name== Utils.CameraPosition)
            {
                _cameraTrans = m.Value.transform;
            }

            var render = m.Value.GetComponent<Renderer>();
            if (render != null) 
            {
                var name = m.Value.name.Split('_')[0];

                var tempMat = listMaterials.Find(mt => { return mt.name == name; });
                if (tempMat != null)
                {
                    render.material = tempMat.GetMaterial();
                    
                }
            }
        }

        ICamera tempCamera=null;
        if (loaderContext.RootModel.AllCameras.Count>0)
        {
           tempCamera = loaderContext.RootModel.AllCameras[0];
        }

        GameManager.Instance.cameraController.SetCameraInfo(_rootModel, _cameraTrans, tempCamera);


        var camera = GetComponentInChildren<Camera>();
        if (camera != null)
        {
            Destroy(camera);

        }
        var lod = GetComponentInChildren<LODGroup>();
        if (lod != null)
        {
            Destroy(lod);
        }
    }

}

 