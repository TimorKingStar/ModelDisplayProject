using System;
using System.Collections;
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
    const string _cameraName = "Camera";
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
        GameManager.Instance.inputManage.outLineStateEvent.AddListener(SetOutLineState);
        GameManager.Instance.inputManage.modelAlphaStateEvent.AddListener(SetAlphaState);
    }
    private void OnDisable()
    {
        try
        {
            GameManager.Instance.inputManage.outLineStateEvent.RemoveListener(SetOutLineState);
            GameManager.Instance.inputManage.modelAlphaStateEvent.RemoveListener(SetAlphaState);
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

    public void Init(AssetLoaderContext loaderContext)
    {
        _rootModel = loaderContext.RootGameObject;
       _allAnimClip = loaderContext.RootModel.AllAnimations;
        _findAnim = _rootModel.TryGetComponent(out _anim);

        Texture _mainTexture = null;
        foreach (var lt in loaderContext.LoadedTextures)
        {
            if (lt.Key.Name=="_MainTexture")
            {
                _mainTexture = lt.Value.UnityTexture;   
            }
        }

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
                var mat= m.Value.AddComponent<MaterialCreator>();

                //暂且写null
                mat.InitMaterial(_mainTexture); 
                materialCreators.Add(mat);
            }
        }

        GameManager.Instance.cameraController.SetCameraInfo(_rootModel, _cameraTrans);
        SetAlphaState(1);
        SetOutLineState(0);
    }

}
