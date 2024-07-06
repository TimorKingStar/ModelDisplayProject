using System;
using System.Collections.Generic;
using System.IO;
using TriLibCore;
using TriLibCore.General;
using TriLibCore.Interfaces;
using UnityEngine;
using UnityEngine.Events;

public class FileReferenceBinding : MonoBehaviour
{


    FileLayerBinding  fileLayerBinding;

    public void SetFileReferenceBinding(FileLayerBinding binding)
    {
        fileLayerBinding = binding;
       InputManage.Instance.SetHeadLayerShowEvent.AddListener(fileLayerBinding.SetHeadActive );
        InputManage.Instance.ResetHeadLayerShowEvent.AddListener(fileLayerBinding.ResetHeadActive);
    }

    [SerializeField]
    public Transform _cameraTrans;

    [SerializeField]
    public GameObject _rootModel;

    private void OnEnable()
    {
        InputManage.Instance.OutLineStateEvent.AddListener(SetOutLineState);
        InputManage.Instance.SetOutlineWidthEvent.AddListener(SetOutLineWidth);
        InputManage.Instance.AlphaStateEvent.AddListener(SetAlphaState);
    }

    private void SetOutLineWidth(float arg0)
    {
        foreach (var mat in listMaterials)
        {
            mat.SetOutlineWidth(arg0);
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
                Debug.Log("xxxxxxxxx: "+_cameraTrans.localPosition.x);
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
          
         CameraManager.Instance.SetCameraInfo(_rootModel.transform,_cameraTrans,tempCamera);

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
     

     public Transform GetBrither(Transform camera)
     {
         if(camera.parent!=null)
         {
           foreach (Transform child in camera.parent)
           {
            
            if (child != camera)
            {
               return child;
            }
            }
         }
        return _rootModel.transform;
     }
}

 