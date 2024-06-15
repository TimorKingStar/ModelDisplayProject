using System;
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

    const string _cameraName = "CameraPosition";
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
    /// ��ͬ�㼶��ģ��
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

        Debug.Log( "��������� "+loaderContext.RootModel.AllCameras.Count);
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
