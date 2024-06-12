using System;
using System.Collections;
using System.Collections.Generic;
using TriLibCore;
using TriLibCore.Interfaces;
using UnityEngine;

/*
    1. ��дĬд���Դ��������Ŵ���Сģ�ͣ���ת�Ӵ�����Դ��ת����ҪoutLineЧ����
    2. ���龲��Դ����������ֲ��ʣ��Ӵ���ת�Ŵ���С����Դ��ת����ҪoutLineЧ������Ҫ͸��Ч����
    3. ʯ��ṹ��������ת���Ŵ���С�۲죬��Դ��ת����ҪoutLineЧ����
    4. ͷ���ṹ��������ת���Ŵ���С�۲죬��Դ��ת����Ƥ�������⣬�����㼶���й۲죬��ҪoutLineЧ����

*/


public class FileReferenceBinding : MonoBehaviour
{
    const string _cameraName = "Camera";
    const string _depthModeName = "Depth";

    [SerializeField]
    public Transform _cameraTrans;
    [SerializeField]
    public GameObject _rootModel;

    //���еĶ�������
    public List<IAnimation> _allAnimClip;

    public bool _findAnim;
    public Animation _anim;

    public AnimationClip currentAnimClip;
    public float currentFrame;

    /*
     1. չʾ��ͬ�㼶������չʾ
     2. ���Ŷ���Ҳ�����ﲥ��
     3. UI����Ҫ֪���ж��ٸ�����
         
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
    /// ��ͬ�㼶��ģ��
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
                //��ͬ�㼶չʾ��ģ��
                _depthModel.Add(m.Value);
            }

            if (m.Value.GetComponent<Renderer>()!=null)
            {
                var mat= m.Value.AddComponent<MaterialCreator>();

                //����дnull
                mat.InitMaterial(_mainTexture); 
                materialCreators.Add(mat);
            }
        }

        GameManager.Instance.cameraController.SetCameraInfo(_rootModel, _cameraTrans);
        SetAlphaState(1);
        SetOutLineState(0);
    }

}
