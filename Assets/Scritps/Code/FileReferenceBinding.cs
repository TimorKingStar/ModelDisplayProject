using System;
using System.Collections.Generic;
using System.IO;
using TriLibCore;
using TriLibCore.General;
using TriLibCore.Interfaces;
using UnityEngine;
using UnityEngine.Events;


/*
    1. ��дĬд���Դ��������Ŵ���Сģ�ͣ���ת�Ӵ�����Դ��ת����ҪoutLineЧ����
    2. ���龲��Դ����������ֲ��ʣ��Ӵ���ת�Ŵ���С����Դ��ת����ҪoutLineЧ������Ҫ͸��Ч����
    3. ʯ��ṹ��������ת���Ŵ���С�۲죬��Դ��ת����ҪoutLineЧ����
    4. ͷ���ṹ��������ת���Ŵ���С�۲죬��Դ��ת����Ƥ�������⣬�����㼶���й۲죬��ҪoutLineЧ����

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

 