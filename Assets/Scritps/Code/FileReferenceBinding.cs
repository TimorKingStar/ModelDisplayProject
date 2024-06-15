using System;
using System.Collections.Generic;
using TriLibCore;
using TriLibCore.Interfaces;
using UnityEngine;


/*
    1. 速写默写，自带动画，放大缩小模型，旋转视窗，光源旋转，需要outLine效果。
    2. 单组静物，自带动画，多种材质，视窗旋转放大缩小，光源旋转，需要outLine效果，需要透明效果。
    3. 石膏结构，任意旋转，放大缩小观察，光源旋转，需要outLine效果。
    4. 头部结构，任意旋转，放大缩小观察，光源旋转，分皮肤，肌肉，骨骼层级进行观察，需要outLine效果。

*/


public class FileReferenceBinding : MonoBehaviour
{

    const string _cameraName = "CameraPosition";
    const string _depthModeName = "Depth";

    [SerializeField]
    public Transform _cameraTrans;

    [SerializeField]
    public GameObject _rootModel;

    //所有的动画数据
    public List<IAnimation> _allAnimClip;

    public bool _findAnim;
    public Animation _anim;

    public AnimationClip currentAnimClip;
    public float currentFrame;


    /*
     1. 展示不同层级在这里展示
     2. 播放动画也在这里播放
     3. UI层需要知道有多少个动画
         
     */
    
    private void OnEnable()
    {
        GameManager.Instance.inputManage.OutLineStateEvent.AddListener(SetOutLineState);
        GameManager.Instance.inputManage.ModelAlphaStateEvent.AddListener(SetAlphaState);
        GameManager.Instance.inputManage.MoveDirectionEvent.AddListener(ModelRotate);
        GameManager.Instance.inputManage.TurnOnModelRotateEvent.AddListener(SetOpenRotate);
        GameManager.Instance.inputManage.ResetModelRotateEvent.AddListener(ResetRotate);
    }

     Quaternion initQua;
    private void Start()
    {
        openModelRotate = true;
        initQua = _rootModel.transform.rotation;
        Debug.Log("angle: "+initQua.eulerAngles.ToString());
    }

    Vector3 eulerRotate = new Vector3();

    bool openModelRotate;
    void SetOpenRotate(bool state)
    {
        openModelRotate = state;
    }
    private void ModelRotate(Vector2 arg0)
    {
        if (openModelRotate)
        {
            eulerRotate.x = arg0.y;
            eulerRotate.y = -arg0.x;
            _rootModel.transform.Rotate(eulerRotate, Space.World);
        }
    }

    void ResetRotate()
    {
        _rootModel.transform.localRotation = initQua;
    }


    private void OnDisable()
    {
        try
        {
            GameManager.Instance.inputManage.OutLineStateEvent.RemoveListener(SetOutLineState);
            GameManager.Instance.inputManage.ModelAlphaStateEvent.RemoveListener(SetAlphaState);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    private void SetAlphaState(float arg0)
    {
        foreach (var mat in materialCreators)
        {
            mat.SetAlpha(arg0);
        }
    }

    private void SetOutLineState(float arg0)
    {
        foreach (var mat in materialCreators)
        {
            bool state= arg0 == 1 ? true : false;
            mat.SetOutLineState(state);
        }
    }

    public List<MaterialCreator> materialCreators= new List<MaterialCreator>();
    /// <summary>
    /// 不同层级的模型
    /// </summary>
    public List<GameObject> _depthModel =new List<GameObject>();

    public Vector3 euler;
    public void Init(AssetLoaderContext loaderContext, Texture baseColor, Texture normalColor, Texture roughNess)
    {
        _rootModel = loaderContext.RootGameObject;
       _allAnimClip = loaderContext.RootModel.AllAnimations;
        

        _findAnim = _rootModel.TryGetComponent(out _anim);
 
        foreach (var lt in loaderContext.LoadedTextures)
        {
            Debug.Log("texture: "+lt.Key.Name);
        }

        Debug.Log( "相机数量： "+loaderContext.RootModel.AllCameras.Count);
        foreach (var m in loaderContext.GameObjects)
        {
            if (m.Key.Name== _cameraName)
            {
                _cameraTrans = m.Value.transform;
            }


            if (m.Key.Name == _depthModeName)
            {
                //不同层级展示的模型
                _depthModel.Add(m.Value);
            }

            if (m.Value.GetComponent<Renderer>()!=null)
            {
                if (m.Value.name != "Ground1"&& m.Value.name != "Ground")
                {
                    var mat = m.Value.AddComponent<MaterialCreator>();
                    mat.InitMaterial(baseColor, normalColor, roughNess);
                    materialCreators.Add(mat);
                }
            }
        }

        ICamera tempCamera=null;
        if (loaderContext.RootModel.AllCameras.Count>0)
        {
            tempCamera = loaderContext.RootModel.AllCameras[0];
        }
        GameManager.Instance.cameraController.SetCameraInfo(_rootModel, _cameraTrans, tempCamera);

        if (tempCamera!=null)
        {
            var camera= GetComponentInChildren<Camera>();
            if (camera!=null)
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

}
