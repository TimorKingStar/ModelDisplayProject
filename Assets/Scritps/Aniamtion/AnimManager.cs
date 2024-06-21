using System;
using System.Collections;
using System.Collections.Generic;
using TriLibCore;
using TriLibCore.Extensions;
using UnityEditor.Animations;
using UnityEngine;

public class AnimManager : MonoBehaviour
{
    public Animation m_Animator;
    public RuntimeAnimatorController m_RuntimeAnimatorController;
    string mOriginClipName = "Play";

    public string path="";

    private void OnGUI()
    {
        if (GUILayout.Button(">>>>>>>>>>走你"))
        {
            var assetLoaderOptions = AssetLoader.CreateDefaultLoaderOptions();
            assetLoaderOptions.AnimationType = TriLibCore.General.AnimationType.Humanoid;

            var assetLoaderFilePicker = AssetLoaderFilePicker.Create();
            assetLoaderFilePicker.LoadModelFromFilePickerAsync("Select a File", OnLoad, OnMaterialLoad, OnProgress,
                OnBeginLoad, OnError, gameObject, assetLoaderOptions);

        }

        //if (GUILayout.Button("<<<<<<<<<<<<走我"))
        //{
        //    UpDateAnimator(animationClips[0]);
        //}


        if (GUILayout.Button("<<<<<<<<<<<<播放"))
        {
            m_Animator = GetComponent<Animation>();
            m_Animator.AddClip(animationClips[0],"1");
            m_Animator.clip = animationClips[0];
            m_Animator.Play();
        }

    }

    private void OnBeginLoad(bool obj)
    {
        
    }

    private void OnError(IContextualizedError obj)
    {
    }

    private void OnProgress(AssetLoaderContext arg1, float arg2)
    {
    }
    public AnimationClip anim;
    public List<AnimationClip>  animationClips;
    private void OnMaterialLoad(AssetLoaderContext obj)
    {
        animationClips = new List<AnimationClip>();
        var a = obj.RootGameObject.GetComponent<Animation>();

        if (a != null)
        {
            animationClips = a.GetAllAnimationClips();
            //if (animationClips.Count > 0)
            //{
            //    m_Animator.clip = animationClips[0];
            //    m_Animator.Play();
            //}
        }
    }

    private void OnLoad(AssetLoaderContext obj)
    {

    }

    void UpDateAnimator(AnimationClip animClip)
    {
        //var tOverrideController = new AnimatorOverrideController();
        //tOverrideController.runtimeAnimatorController = m_RuntimeAnimatorController;
        //tOverrideController[mOriginClipName] = animClip;
        //m_Animator.runtimeAnimatorController = null;
        //m_Animator.runtimeAnimatorController = tOverrideController;
        //m_Animator.Play(mOriginClipName, 0, 0); 
    }

}
